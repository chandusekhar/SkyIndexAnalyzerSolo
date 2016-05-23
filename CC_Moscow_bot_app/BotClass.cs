using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using SkyImagesAnalyzerLibraries;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;


namespace CC_Moscow_bot_app
{
    public class BotClass
    {
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private string ImagesBasePath = "";
        private string ConcurrentDataXMLfilesBasePath = "";
        private string YRGBstatsXMLdataFilesDirectory = "";
        private string IncomingImagesBasePath = "";

        private string R2SLufftUMBappPath = "";
        private string VentusLufftUMBappPath = "";
        private string WSLufftUMBappPath = "";

        private string tgrm_token = "";
        private bool NeedToStopFlag = false;
        private string logFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          ".log";




        public BotClass()
        {
        }




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
                        if (ServiceTools.CheckIfDirectoryExists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs"))
                        {
                            ServiceTools.logToTextFile(logFilename,
                                "" + update.Message.Chat.Id + " : " + update.Message.Text + Environment.NewLine, true, true);
                        }



                        if (update.Message.Text == "/current_image")
                        {
                            string FilenameToSend = "";
                            Image<Bgr, byte> lastImagesCouple = CurrentImagesCouple(out FilenameToSend);
                            string tmpFNameToSave = Path.GetTempPath();
                            tmpFNameToSave += (tmpFNameToSave.Last() == Path.DirectorySeparatorChar)
                                ? ("")
                                : (Path.DirectorySeparatorChar.ToString());
                            tmpFNameToSave += FilenameToSend;
                            lastImagesCouple.Save(tmpFNameToSave);
                            var fileStream = File.Open(tmpFNameToSave, FileMode.Open);
                            Message sentMessage = await Bot.SendPhoto(update.Message.Chat.Id, new FileToSend(FilenameToSend, fileStream));

                            File.Delete(tmpFNameToSave);
                        }
                        else if (update.Message.Text == "/concurrent_info")
                        {
                            string lastConcurrentInfoFileName = GetLastConcurrentInfo();

                            Message sentMessage =
                                await
                                    Bot.SendTextMessage(update.Message.Chat.Id, lastConcurrentInfoFileName, true, false,
                                        update.Message.MessageId);
                        }
                        //else if (update.Message.Text == "/current_cc")
                        //{
                        //    Task<string> tskObtainCC = ObtainLastImageCC();

                        //    Message sentMessage1 =
                        //        await
                        //            Bot.SendTextMessage(update.Message.Chat.Id,
                        //                "Started calculations. Be patient please, it may take a few minutes.", true,
                        //                false, update.Message.MessageId);

                        //    string strReply = await tskObtainCC;

                        //    Message sentMessage2 =
                        //        await
                        //            Bot.SendTextMessage(update.Message.Chat.Id, strReply, true, false,
                        //                update.Message.MessageId);
                        //}
                        else if (update.Message.Text == "/meteo_info")
                        {
                            string strCurrentMeteoParametersString = await ObtainLatestMeteoParameters();
                            Message sentMessage =
                                await
                                    Bot.SendTextMessage(update.Message.Chat.Id, strCurrentMeteoParametersString, true, false,
                                        update.Message.MessageId);

                        }
                        else if (update.Message.Text == "/start")
                        {
                            string strStartMessage =
                                ServiceTools.ReadTextFromFile(Directory.GetCurrentDirectory() +
                                                              Path.DirectorySeparatorChar + "settings" +
                                                              Path.DirectorySeparatorChar + "BotStartMessage.txt");
                            Message sentMessage =
                                await
                                    Bot.SendTextMessage(update.Message.Chat.Id, strStartMessage, true, false,
                                        update.Message.MessageId);
                            Console.WriteLine("Echo Message: {0}", update.Message.Text);
                        }
                        else
                        {
                            // await Bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                            // await Task.Delay(200);
                            var t = await Bot.SendTextMessage(update.Message.Chat.Id, "I can`t get your request. Please try again.");
                            Console.WriteLine("Echo Message: {0}", update.Message.Text);
                        }
                    }

                    if (update.Message.Type == MessageType.PhotoMessage)
                    {
                        var file = await Bot.GetFile(update.Message.Photo.LastOrDefault()?.FileId);

                        Console.WriteLine("Received Photo: {0}", file.FilePath);

                        var filename = IncomingImagesBasePath + file.FileId + "." + file.FilePath.Split('.').Last();

                        using (var profileImageStream = File.Open(filename, FileMode.Create))
                        {
                            await file.FileStream.CopyToAsync(profileImageStream);
                        }

                        Console.WriteLine("Photo saved to: {0}", filename);

                        if (ServiceTools.CheckIfDirectoryExists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs"))
                        {
                            ServiceTools.logToTextFile(logFilename,
                                "" + update.Message.Chat.Id + " : Received Photo: " + file.FilePath + Environment.NewLine + "\t\tSaved to file: " + filename + Environment.NewLine, true, true);
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




        private string CurrentImageFilename()
        {
            DirectoryInfo dir = new DirectoryInfo(ImagesBasePath);
            List<FileInfo> lImagesFilesID1 = dir.GetFiles("*devID1.jpg", SearchOption.AllDirectories).ToList();
            lImagesFilesID1.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            string strLastImageID1Fname = lImagesFilesID1.Last().FullName;


            return strLastImageID1Fname;
        }




        private Image<Bgr, byte> CurrentImagesCouple(out string FilenameToSend)
        {
            DirectoryInfo dir = new DirectoryInfo(ImagesBasePath);
            List<FileInfo> lImagesFilesID1 = dir.GetFiles("*devID1.jpg", SearchOption.AllDirectories).ToList();
            lImagesFilesID1.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            string strLastImageID1Fname = lImagesFilesID1.Last().FullName;

            FilenameToSend = strLastImageID1Fname.Replace("devID1", "");
            FilenameToSend = Path.GetFileName(FilenameToSend);

            List<FileInfo> lImagesFilesID2 = dir.GetFiles("*devID2.jpg", SearchOption.AllDirectories).ToList();
            lImagesFilesID2.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
            string strLastImageID2Fname = lImagesFilesID2.Last().FullName;

            Image<Bgr, byte> img1 = new Image<Bgr, byte>(strLastImageID1Fname);
            Image<Bgr, byte> img2 = new Image<Bgr, byte>(strLastImageID2Fname);

            Size img1Size = img1.Size;
            Size resimgSize = new Size(img1Size.Width * 2, img1Size.Height);

            Image<Bgr, byte> resImg = new Image<Bgr, byte>(resimgSize);
            Graphics g = Graphics.FromImage(resImg.Bitmap);
            g.DrawImage(img1.Bitmap, new Point(0, 0));
            g.DrawImage(img2.Bitmap, new Point(img1Size.Width, 0));

            resImg = resImg.Resize(0.25d, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            return resImg;
        }




        private string GetLastConcurrentInfo()
        {
            DirectoryInfo dir = new DirectoryInfo(ConcurrentDataXMLfilesBasePath);
            List<FileInfo> lXMLFilesInfo = dir.GetFiles("*.xml", SearchOption.AllDirectories).ToList();
            lXMLFilesInfo.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));

            Dictionary<string, object> dictReadXMLfileData =
                ServiceTools.ReadDictionaryFromXML(lXMLFilesInfo.Last().FullName);

            DateTime utcNow = DateTime.UtcNow;
            DateTime concurrentDataDateTime = DateTime.Parse(dictReadXMLfileData["DateTime"] as string);

            string strDataToReport = "Current server date/time UTC: " + utcNow.ToString("u") + Environment.NewLine;
            strDataToReport += "Concurrent data date/time UTC: " +
                               concurrentDataDateTime.ToString("u") +
                               Environment.NewLine;
            strDataToReport += "time elapsed since last shot: " + (utcNow - concurrentDataDateTime).ToString("G") + Environment.NewLine;
            GPSdata gps = new GPSdata((string)dictReadXMLfileData["GPSdata"], GPSdatasources.CloudCamArduinoGPS,
                concurrentDataDateTime);
            strDataToReport += "GPS: " + gps.HRString() + Environment.NewLine;

            return strDataToReport;
        }




        private async Task<string> ObtainLastImageCC()
        {
            string retStr = "";

            SDCpredictorNN predSDC = new SDCpredictorNN(CurrentImageFilename(), ConcurrentDataXMLfilesBasePath,
                YRGBstatsXMLdataFilesDirectory);
            SunDiskCondition sdc = SunDiskCondition.Undefined;
            try
            {
                sdc = await
                    predSDC.CalcSDC_NN(
                        Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                        Path.DirectorySeparatorChar + "NNconfig.csv",
                        Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                        Path.DirectorySeparatorChar + "NNtrainedParameters.csv",
                        Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                        Path.DirectorySeparatorChar + "NormMeans.csv",
                        Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                        Path.DirectorySeparatorChar + "NormRange.csv");

                retStr = "Sun disk condition obtained using last available snapshot: " + sdc.ToString();
            }
            catch (Exception ex)
            {
                retStr = "Error obtaining SDC. Please refer to the support: krinitky@sail.msk.ru";
            }


            return retStr;
        }





        private async Task<string> ObtainLatestMeteoParameters()
        {
            string retStr = "";

            // R2SLufftUMBappPath
            if (Directory.Exists(R2SLufftUMBappPath))
            {
                List<FileInfo> lTXTdataFilesInfoList =
                    ((new DirectoryInfo(R2SLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
                        SearchOption.AllDirectories)).ToList();
                lTXTdataFilesInfoList.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
                FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
                List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true, ";");
                retStr +=
                    "R2S:" + Environment.NewLine +
                    "Date time ; Precipitation absol. [mm] ; Precipitation type ; Ambient temperature [°C] ; Precipitat.intensity [mil/h]" + Environment.NewLine;
                retStr += string.Join(" ; ", Contents.Last()) + Environment.NewLine;
            }



            // VentusLufftUMBappPath
            // Date time ; Virtual temperature [°C] ; Wind speed [m/s] ; Wind speed [m/s] Vect. ; Wind direction [°] ; Wind direction [°] Vect. ; Abs. air pressure [hPa] ; Wind value quality [%]
            if (Directory.Exists(VentusLufftUMBappPath))
            {
                List<FileInfo> lTXTdataFilesInfoList =
                    ((new DirectoryInfo(VentusLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
                        SearchOption.AllDirectories)).ToList();
                lTXTdataFilesInfoList.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
                FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
                List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true, ";");
                retStr +=
                    "Ventus:" + Environment.NewLine +
                    "Date time ; Virtual temperature [°C] ; Wind speed [m/s] ; Wind speed [m/s] Vect. ; Wind direction [°] ; Wind direction [°] Vect. ; Abs. air pressure [hPa] ; Wind value quality [%]" + Environment.NewLine;
                retStr += string.Join(" ; ", Contents.Last()) + Environment.NewLine;
            }



            // WSLufftUMBappPath
            // Date time ; Temperature [°C] ; Abs. air pressure [hPa] ; Relative humidity [%] ; Abs. humidity [g/m³]
            if (Directory.Exists(WSLufftUMBappPath))
            {
                List<FileInfo> lTXTdataFilesInfoList =
                    ((new DirectoryInfo(WSLufftUMBappPath)).GetFiles("????-??-??Values.Txt",
                        SearchOption.AllDirectories)).ToList();
                lTXTdataFilesInfoList.Sort((finfo1, finfo2) => finfo1.CreationTimeUtc.CompareTo(finfo2.CreationTimeUtc));
                FileInfo lastTXTdataFileInfo = lTXTdataFilesInfoList.Last();
                List<List<string>> Contents = ServiceTools.ReadDataFromCSV(lastTXTdataFileInfo.FullName, 2, true, ";");
                retStr +=
                    "WS:" + Environment.NewLine +
                    "Date time ; Temperature [°C] ; Abs. air pressure [hPa] ; Relative humidity [%] ; Abs. humidity [g/m³]" + Environment.NewLine;
                retStr += string.Join(" ; ", Contents.Last()) + Environment.NewLine;
            }


            return retStr;
        }



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

            // ImagesBasePath
            if (defaultProperties.ContainsKey("ImagesBasePath"))
            {
                ImagesBasePath = (string)defaultProperties["ImagesBasePath"];
            }
            else
            {
                ImagesBasePath = CurDir + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
                defaultProperties.Add("ImagesBasePath", ImagesBasePath);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // IncomingImagesBasePath
            if (defaultProperties.ContainsKey("IncomingImagesBasePath"))
            {
                IncomingImagesBasePath = (string)defaultProperties["IncomingImagesBasePath"];
            }
            else
            {
                IncomingImagesBasePath = CurDir + Path.DirectorySeparatorChar + "IncomingImages" + Path.DirectorySeparatorChar;
                defaultProperties.Add("IncomingImagesBasePath", IncomingImagesBasePath);

                ServiceTools.CheckIfDirectoryExists(IncomingImagesBasePath);

                bDefaultPropertiesHasBeenUpdated = true;
            }



            // ConcurrentDataXMLfilesBasePath
            if (defaultProperties.ContainsKey("ConcurrentDataXMLfilesBasePath"))
            {
                ConcurrentDataXMLfilesBasePath = (string)defaultProperties["ConcurrentDataXMLfilesBasePath"];
            }
            else
            {
                ConcurrentDataXMLfilesBasePath = "";
                defaultProperties.Add("ConcurrentDataXMLfilesBasePath", ConcurrentDataXMLfilesBasePath);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // YRGBstatsXMLdataFilesDirectory
            if (defaultProperties.ContainsKey("YRGBstatsXMLdataFilesDirectory"))
            {
                YRGBstatsXMLdataFilesDirectory = (string)defaultProperties["YRGBstatsXMLdataFilesDirectory"];
            }
            else
            {
                YRGBstatsXMLdataFilesDirectory = "";
                defaultProperties.Add("YRGBstatsXMLdataFilesDirectory", YRGBstatsXMLdataFilesDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }



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




            // R2SLufftUMBappPath
            if (defaultProperties.ContainsKey("R2SLufftUMBappPath"))
            {
                R2SLufftUMBappPath = (string)defaultProperties["R2SLufftUMBappPath"];
            }
            else
            {
                R2SLufftUMBappPath = "";
                defaultProperties.Add("R2SLufftUMBappPath", R2SLufftUMBappPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }


            // VentusLufftUMBappPath
            if (defaultProperties.ContainsKey("VentusLufftUMBappPath"))
            {
                VentusLufftUMBappPath = (string)defaultProperties["VentusLufftUMBappPath"];
            }
            else
            {
                VentusLufftUMBappPath = "";
                defaultProperties.Add("VentusLufftUMBappPath", VentusLufftUMBappPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }




            // WSLufftUMBappPath
            if (defaultProperties.ContainsKey("WSLufftUMBappPath"))
            {
                WSLufftUMBappPath = (string)defaultProperties["WSLufftUMBappPath"];
            }
            else
            {
                WSLufftUMBappPath = "";
                defaultProperties.Add("WSLufftUMBappPath", WSLufftUMBappPath);
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




        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            NeedToStopFlag = true;
        }
    }
}
