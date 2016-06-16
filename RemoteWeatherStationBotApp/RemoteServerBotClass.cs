using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkyImagesAnalyzerLibraries;
using SolarPositioning;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;


namespace RemoteWeatherStationBotApp
{
    public class RemoteServerBotClass
    {
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";

        private string DataStoreDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                   "data" + Path.DirectorySeparatorChar;

        private string tgrm_token = "";
        private bool NeedToStopFlag = false;
        private string logFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          ".log";




        public void Start()
        {
            readDefaultProperties();

            Run().Wait();
        }





        private async Task Run()
        {
            if (tgrm_token == "")
            {
                Console.WriteLine("telegram token for channel is not set. Processing will not proceed.");
                Console.WriteLine("Finished. Press any key...");
                Console.ReadKey();
                return;
            }
            var Bot = new Api(tgrm_token);

            var me = await Bot.GetMe();

            Console.WriteLine("Hello my name is {0}", me.Username);

            var offset = 0;

            while (true)
            {
                var updates = await Bot.GetUpdates(offset);

                foreach (var update in updates)
                {
                    if (update.Message.Type == MessageType.TextMessage)
                    {
                        Console.WriteLine("{0} from {1} ({2})", update.Message.Text, update.Message.Chat.Id, update.Message.Chat.Username);


                        if (ServiceTools.CheckIfDirectoryExists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs"))
                        {
                            ServiceTools.logToTextFile(logFilename,
                                "" + update.Message.Chat.Id + " ("+ update.Message.Chat.Username + ") : " + update.Message.Text + Environment.NewLine, true, true);
                        }



                        if (update.Message.Text == "/current_image")
                        {
                            Task<string> taskA = Task.Run(() => CurrentImagesCoupleImageFilename());
                            Task continuation = taskA.ContinueWith(antecedent =>
                            {
                                string FilenameToSend = antecedent.Result;
                                var fileStream = File.Open(FilenameToSend, FileMode.Open, FileAccess.Read, FileShare.Read);
                                Bot.SendPhoto(update.Message.Chat.Id, new FileToSend(FilenameToSend, fileStream));
                            });
                        }
                        else if (update.Message.Text == "/concurrent_info")
                        {
                            Task<string> taskA = Task.Run(() => ReadLastConcurrentInfo());
                            Task continuation = taskA.ContinueWith(antecedent =>
                            {
                                string strReply = antecedent.Result;
                                Bot.SendTextMessage(update.Message.Chat.Id, strReply, true, false,
                                    update.Message.MessageId);
                            });
                        }
                        else if (update.Message.Text == "/current_cc")
                        {
                            Task<string> taskA = Task.Run(() => ReadCurrentCCinfo());
                            Task continuation = taskA.ContinueWith(antecedent =>
                            {
                                string strReply = antecedent.Result;
                                Bot.SendTextMessage(update.Message.Chat.Id, strReply, true, false,
                                    update.Message.MessageId);
                            });
                        }
                        else if (update.Message.Text == "/meteo_info")
                        {
                            Task<string> taskA = Task.Run(() => ObtainLatestMeteoParameters());
                            Task continuation = taskA.ContinueWith(antecedent =>
                            {
                                string strReply = antecedent.Result;
                                Bot.SendTextMessage(update.Message.Chat.Id, strReply, true, false,
                                        update.Message.MessageId);
                            });
                        }
                        else if (update.Message.Text == "/start")
                        {
                            Task taskA = Task.Run(() =>
                            {
                                string strStartMessage =
                                ServiceTools.ReadTextFromFile(Directory.GetCurrentDirectory() +
                                                              Path.DirectorySeparatorChar + "settings" +
                                                              Path.DirectorySeparatorChar + "BotStartMessage.txt");
                                Bot.SendTextMessage(update.Message.Chat.Id, strStartMessage, true, false, update.Message.MessageId);
                                Console.WriteLine("Echo Message: {0}", update.Message.Text);
                            });
                            
                        }
                        else
                        {
                            // await Bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                            // await Task.Delay(200);
                            var t =
                                await
                                    Bot.SendTextMessage(update.Message.Chat.Id,
                                        "Sorry, I can`t understand you. Please try again using the following list of commands:" +
                                        Environment.NewLine + "/current_image" +
                                        Environment.NewLine + "/concurrent_info" +
                                        Environment.NewLine + "/current_cc" +
                                        Environment.NewLine + "/meteo_info");
                        }
                    }
                    
                    offset = update.Id + 1;
                }

                await Task.Delay(200);

                if (NeedToStopFlag)
                {
                    break;
                }
            }
        }



        
        private string CurrentImagesCoupleImageFilename()
        {
            DirectoryInfo dir = new DirectoryInfo(DataStoreDirectory);
            List<FileInfo> lImagesFiles = dir.GetFiles("*.jpg").ToList();
            lImagesFiles.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            string strLastImageFilename = lImagesFiles.Last().FullName;

            return strLastImageFilename;
        }




        private string ReadLastConcurrentInfo()
        {
            DirectoryInfo dir = new DirectoryInfo(DataStoreDirectory);
            List<FileInfo> lXMLFilesInfo =
                dir.GetFiles(ConventionalTransitions.ImageConcurrentDataFilesNamesPattern(), SearchOption.AllDirectories)
                    .ToList();

            lXMLFilesInfo.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));

            Dictionary<string, object> dictReadXMLfileData =
                ServiceTools.ReadDictionaryFromXML(lXMLFilesInfo.Last().FullName);

            DateTime utcNow = DateTime.UtcNow;
            DateTime concurrentDataDateTime = DateTime.Parse(dictReadXMLfileData["DateTime"] as string);
            concurrentDataDateTime = concurrentDataDateTime.ToUniversalTime();

            string strDataToReport = "Current server date/time UTC: " + utcNow.ToString("dd.MM.yyyy HH:mm:ss") +
                                     Environment.NewLine;
            strDataToReport += "Concurrent data date/time UTC: " +
                               concurrentDataDateTime.ToString("dd.MM.yyyy HH:mm:ss") +
                               Environment.NewLine;
            strDataToReport += "time elapsed since last shot: " +
                               Math.Round((utcNow - concurrentDataDateTime).TotalSeconds) + "s" + Environment.NewLine;


            GPSdata gps = new GPSdata((string)dictReadXMLfileData["GPSdata"], GPSdatasources.CloudCamArduinoGPS,
                concurrentDataDateTime);

            strDataToReport += "GPS: " + gps.HRString(2) + Environment.NewLine;
            SPA spaCalcObject = null;
            AzimuthZenithAngle angle = gps.SunZenithAzimuth(out spaCalcObject);


            DateTime dtSunriseUTC = utcNow;
            TimeOfDay todSunriseUTC = new TimeOfDay(spaCalcObject.spa.sunrise);
            dtSunriseUTC = new DateTime(dtSunriseUTC.Year, dtSunriseUTC.Month, dtSunriseUTC.Day, todSunriseUTC.hour,
                todSunriseUTC.minute, todSunriseUTC.second, DateTimeKind.Utc);


            // TimeZoneInfo mowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            TimeZoneInfo mowTimeZone = TimeZoneInfo.Local;
            DateTime dtSunriseMOW = TimeZoneInfo.ConvertTimeFromUtc(dtSunriseUTC, mowTimeZone);

            strDataToReport += "Sunrise Moscow time: " + (dtSunriseMOW.TimeOfDay.ToString()) + Environment.NewLine;

            DateTime dtSunsetUTC = utcNow;
            TimeOfDay todSunsetUTC = new TimeOfDay(spaCalcObject.spa.sunset);
            dtSunsetUTC = new DateTime(dtSunsetUTC.Year, dtSunsetUTC.Month, dtSunsetUTC.Day, todSunsetUTC.hour,
                todSunsetUTC.minute, todSunsetUTC.second, DateTimeKind.Utc);
            DateTime dtSunsetMOW = TimeZoneInfo.ConvertTimeFromUtc(dtSunsetUTC, mowTimeZone);

            strDataToReport += "Sunset Moscow time: " + (dtSunsetMOW.TimeOfDay.ToString()) + Environment.NewLine;

            return strDataToReport;
        }




        private async Task<string> ReadCurrentCCinfo()
        {
            string retStr = "";

            if (!Directory.Exists(DataStoreDirectory))
            {
                throw new DirectoryNotFoundException("unable to locate directory: " + DataStoreDirectory);
            }

            DirectoryInfo dir = new DirectoryInfo(DataStoreDirectory);
            List<FileInfo> lXMLFilesInfo =
                dir.GetFiles(ConventionalTransitions.ImageProcessedAndPredictedDataFileNamesPattern(),
                    SearchOption.AllDirectories).ToList();

            if (lXMLFilesInfo.Count == 0)
            {
                return "No snapshots has been analyzed yet. Please wait for a couple of minutes.";
            }

            lXMLFilesInfo.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            
            SkyImagesProcessedAndPredictedData data = null;
            try
            {
                data =
                    (SkyImagesProcessedAndPredictedData)
                        ServiceTools.ReadObjectFromXML(lXMLFilesInfo.Last().FullName,
                            typeof(SkyImagesProcessedAndPredictedData));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (data != null)
            {
                retStr += "Please note that this finction is still in BETA version!" + Environment.NewLine +
                          "date of snapshot analyzed (UTC): " + data.imageShootingDateTimeUTC.ToString("u") +
                          Environment.NewLine +
                          "Sun disk condition: " + data.PredictedSDC.ToString() + Environment.NewLine +
                          "Total cloud cover: " + data.PredictedCC.CloudCoverTotal + " (of 8)";
            }
            

            return retStr;
        }




        private async Task<string> ObtainLatestMeteoParameters()
        {
            string retStr = "";

            if (!Directory.Exists(DataStoreDirectory))
            {
                throw new DirectoryNotFoundException("unable to locate directory: " + DataStoreDirectory);
            }

            #region WS

            DirectoryInfo dir = new DirectoryInfo(DataStoreDirectory);
            List<FileInfo> lWSxmlFilesInfo =
                dir.GetFiles(ConventionalTransitions.WSUMBdataFileNamePattern(),
                    SearchOption.AllDirectories).ToList();

            if (lWSxmlFilesInfo.Count == 0)
            {
                return "No data processed yet. Please wait for a couple of minutes.";
            }

            lWSxmlFilesInfo.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));

            LufftWSdata WSdata = null;
            try
            {
                WSdata =
                    (LufftWSdata) ServiceTools.ReadObjectFromXML(lWSxmlFilesInfo.Last().FullName, typeof(LufftWSdata));
            }
            catch (Exception ex)
            {
                return "No data processed yet. Please wait for a couple of minutes.";
            }

            if (WSdata != null)
            {
                retStr += WSdata.ToString() + Environment.NewLine + Environment.NewLine;
            }

            #endregion WS
            



            #region R2S

            List<FileInfo> lR2SxmlFilesInfo =
                dir.GetFiles(ConventionalTransitions.R2SUMBdataFileNamePattern(),
                    SearchOption.AllDirectories).ToList();

            if (lR2SxmlFilesInfo.Count == 0)
            {
                return "No data processed yet. Please wait for a couple of minutes.";
            }

            lR2SxmlFilesInfo.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));

            LufftR2Sdata R2Sdata = null;
            try
            {
                R2Sdata =
                    (LufftR2Sdata)ServiceTools.ReadObjectFromXML(lR2SxmlFilesInfo.Last().FullName, typeof(LufftR2Sdata));
            }
            catch (Exception ex)
            {
                return "No data processed yet. Please wait for a couple of minutes.";
            }

            if (R2Sdata != null)
            {
                retStr += R2Sdata.ToString() + Environment.NewLine + Environment.NewLine;
            }

            #endregion R2S
            



            #region Ventus

            List<FileInfo> lVentusXMLfilesInfo =
                dir.GetFiles(ConventionalTransitions.VentusUMBdataFileNamePattern(),
                    SearchOption.AllDirectories).ToList();

            if (lVentusXMLfilesInfo.Count == 0)
            {
                return "No data processed yet. Please wait for a couple of minutes.";
            }

            lVentusXMLfilesInfo.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));

            LufftVentusdata VentusData = null;
            try
            {
                VentusData =
                    (LufftVentusdata)ServiceTools.ReadObjectFromXML(lVentusXMLfilesInfo.Last().FullName, typeof(LufftVentusdata));
            }
            catch (Exception ex)
            {
                return "No data processed yet. Please wait for a couple of minutes.";
            }

            if (VentusData != null)
            {
                retStr += VentusData.ToString() + Environment.NewLine + Environment.NewLine;
            }

            #endregion Ventus

            return retStr;
        }



        #region default properties

        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";
            if (!File.Exists(defaultPropertiesXMLfileName))
            {
                Console.WriteLine("couldn`t find settings XML file: " + Environment.NewLine +
                                  defaultPropertiesXMLfileName);
            }
            else
            {
                defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            }


            bool bDefaultPropertiesHasBeenUpdated = false;

            string CurDir = Directory.GetCurrentDirectory();


            #region CurrentDataStoreDirectory

            if (defaultProperties.ContainsKey("CurrentDataStoreDirectory"))
            {
                DataStoreDirectory = (string) defaultProperties["CurrentDataStoreDirectory"];
            }
            else
            {
                defaultProperties.Add("CurrentDataStoreDirectory", DataStoreDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion


            #region // obsolete
            // IncomingImagesBasePath
            //if (defaultProperties.ContainsKey("IncomingImagesBasePath"))
            //{
            //    IncomingImagesBasePath = (string)defaultProperties["IncomingImagesBasePath"];
            //}
            //else
            //{
            //    IncomingImagesBasePath = CurDir + Path.DirectorySeparatorChar + "IncomingImages" + Path.DirectorySeparatorChar;
            //    defaultProperties.Add("IncomingImagesBasePath", IncomingImagesBasePath);

            //    ServiceTools.CheckIfDirectoryExists(IncomingImagesBasePath);

            //    bDefaultPropertiesHasBeenUpdated = true;
            //}



            // ConcurrentDataXMLfilesBasePath
            //if (defaultProperties.ContainsKey("ConcurrentDataXMLfilesBasePath"))
            //{
            //    ConcurrentDataXMLfilesBasePath = (string)defaultProperties["ConcurrentDataXMLfilesBasePath"];
            //}
            //else
            //{
            //    ConcurrentDataXMLfilesBasePath = "";
            //    defaultProperties.Add("ConcurrentDataXMLfilesBasePath", ConcurrentDataXMLfilesBasePath);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}


            // YRGBstatsXMLdataFilesDirectory
            //if (defaultProperties.ContainsKey("YRGBstatsXMLdataFilesDirectory"))
            //{
            //    YRGBstatsXMLdataFilesDirectory = (string)defaultProperties["YRGBstatsXMLdataFilesDirectory"];
            //}
            //else
            //{
            //    YRGBstatsXMLdataFilesDirectory = "";
            //    defaultProperties.Add("YRGBstatsXMLdataFilesDirectory", YRGBstatsXMLdataFilesDirectory);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}



            //RadiometersDataPath
            //if (defaultProperties.ContainsKey("RadiometersDataPath"))
            //{
            //    RadiometersDataPath = (string)defaultProperties["RadiometersDataPath"];
            //}
            //else
            //{
            //    RadiometersDataPath = @"C:\Program Files (x86)\Theodor Friedrichs\Comgraph32\";
            //    defaultProperties.Add("RadiometersDataPath", RadiometersDataPath);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            

            //// R2SLufftUMBappPath
            //if (defaultProperties.ContainsKey("R2SLufftUMBappPath"))
            //{
            //    R2SLufftUMBappPath = (string)defaultProperties["R2SLufftUMBappPath"];
            //}
            //else
            //{
            //    R2SLufftUMBappPath = "";
            //    defaultProperties.Add("R2SLufftUMBappPath", R2SLufftUMBappPath);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}


            //// VentusLufftUMBappPath
            //if (defaultProperties.ContainsKey("VentusLufftUMBappPath"))
            //{
            //    VentusLufftUMBappPath = (string)defaultProperties["VentusLufftUMBappPath"];
            //}
            //else
            //{
            //    VentusLufftUMBappPath = "";
            //    defaultProperties.Add("VentusLufftUMBappPath", VentusLufftUMBappPath);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}




            //// WSLufftUMBappPath
            //if (defaultProperties.ContainsKey("WSLufftUMBappPath"))
            //{
            //    WSLufftUMBappPath = (string)defaultProperties["WSLufftUMBappPath"];
            //}
            //else
            //{
            //    WSLufftUMBappPath = "";
            //    defaultProperties.Add("WSLufftUMBappPath", WSLufftUMBappPath);
            //    bDefaultPropertiesHasBeenUpdated = true;
            //}

            #endregion



            // tgrm_token
            if (defaultProperties.ContainsKey("tgrm_token"))
            {
                tgrm_token = (string)defaultProperties["tgrm_token"];
            }
            else
            {
                tgrm_token = "";
                defaultProperties.Add("tgrm_token", tgrm_token);
                bDefaultPropertiesHasBeenUpdated = true;
            }



            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }



        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }

        #endregion default properties


        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            NeedToStopFlag = true;
        }
    }
}
