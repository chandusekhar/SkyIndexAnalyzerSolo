using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkyImagesAnalyzerLibraries;
using SolarPositioning;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using System.Xml.Serialization;


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

        private string errorFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                       Path.DirectorySeparatorChar +
                                       Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                       "_error.log";

        private string strUserAllowedSDCfixing_IDs_CSVfilename = Directory.GetCurrentDirectory() +
                                                                 Path.DirectorySeparatorChar + "settings" +
                                                                 Path.DirectorySeparatorChar +
                                                                 "UserAllowedSDCfixing_IDs.csv";

        private string strUserAllowedTCCfixing_IDs_CSVfilename = Directory.GetCurrentDirectory() +
                                                                 Path.DirectorySeparatorChar + "settings" +
                                                                 Path.DirectorySeparatorChar +
                                                                 "UserAllowedTCCfixing_IDs.csv";

        private static TelegramBotClient Bot = null;



        public void Start()
        {
            readDefaultProperties();

            Task.Run(() => Run()).Wait();
            // Run().Wait();
        }





        // private async Task Run()
        private void Run()
        {
            if (tgrm_token == "")
            {
                Console.WriteLine("telegram token for channel is not set. Processing will not proceed.");
                Console.WriteLine("Finished. Press any key...");
                Console.ReadKey();
                return;
            }
            Bot = new TelegramBotClient(tgrm_token);

            User me = Bot.GetMeAsync().Result;

            Console.WriteLine("Hello my name is {0}", me.Username);

            Bot.OnMessage += Bot_OnMessage;
            Bot.OnReceiveError += BotOnReceiveError;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }



        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            Debugger.Break();
            ApiRequestException ex = e.ApiRequestException;
            Console.WriteLine(ex.Message);
            ServiceTools.logToTextFile(errorFilename,
                "got API exception: " + ex.Message + Environment.NewLine + "Error code: " + ex.ErrorCode +
                Environment.NewLine + ex.StackTrace, true, true);
        }




        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Message currMsg = e.Message;


            if (currMsg.Type == MessageType.TextMessage)
            {
                Console.WriteLine("{0} from {1} ({2})", currMsg.Text, currMsg.Chat.Id, currMsg.Chat.Username);


                if (ServiceTools.CheckIfDirectoryExists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs"))
                {
                    ServiceTools.logToTextFile(logFilename,
                        "" + currMsg.Chat.Id + " (" + currMsg.Chat.Username + ") : " + currMsg.Text + Environment.NewLine, true, true);
                }



                if ((currMsg.Text == "/current_image") || (currMsg.Text == "Image"))
                {
                    Task<string> taskA = Task.Run(() => CurrentImagesCoupleImageFilename());
                    Task continuation = taskA.ContinueWith(antecedent =>
                    {
                        string FilenameToSend = antecedent.Result;
                        var fileStream = File.Open(FilenameToSend, FileMode.Open, FileAccess.Read, FileShare.Read);
                        Bot.SendPhotoAsync(currMsg.Chat.Id, new FileToSend(FilenameToSend, fileStream));
                    });
                }
                else if ((currMsg.Text == "/concurrent_info") || (currMsg.Text == "Info"))
                {
                    Task<string> taskA = Task.Run(() => ReadLastConcurrentInfo());
                    Task continuation = taskA.ContinueWith(antecedent =>
                    {
                        string strReply = antecedent.Result;
                        Bot.SendTextMessageAsync(currMsg.Chat.Id, strReply, true, false,
                            currMsg.MessageId, BasicKeyboard());
                    });
                }
                else if ((currMsg.Text == "/current_cc") || (currMsg.Text == "TCC_SDC"))
                {
                    Task<string> taskA = Task.Run(ReadCurrentCCinfo);
                    Task continuation = taskA.ContinueWith(antecedent =>
                    {
                        string strReply = antecedent.Result;
                        Bot.SendTextMessageAsync(currMsg.Chat.Id, strReply, true, false,
                            currMsg.MessageId, BasicKeyboard());
                    });
                }
                else if ((currMsg.Text == "/meteo_info") || (currMsg.Text == "Meteo"))
                {
                    Task<string> taskA = Task.Run(ObtainLatestMeteoParameters);
                    Task continuation = taskA.ContinueWith(antecedent =>
                    {
                        string strReply = antecedent.Result;
                        Bot.SendTextMessageAsync(currMsg.Chat.Id, strReply, true, false,
                                currMsg.MessageId, BasicKeyboard());
                    });
                }
                else if (currMsg.Text == "/start")
                {
                    Task taskA = Task.Run(() =>
                    {
                        string strStartMessage =
                        ServiceTools.ReadTextFromFile(Directory.GetCurrentDirectory() +
                                                      Path.DirectorySeparatorChar + "settings" +
                                                      Path.DirectorySeparatorChar + "BotStartMessage.txt");
                        Bot.SendTextMessageAsync(currMsg.Chat.Id, strStartMessage, true, false,
                            currMsg.MessageId, BasicKeyboard());
                        Console.WriteLine("Echo Message: {0}", currMsg.Text);
                    });

                }
                else if (currMsg.Text.Contains("/fix"))
                {
                    Console.WriteLine("Echo Message: {0}", currMsg.Text);

                    Task<bool> taskA = Task.Run(() => RegisterFixStage1(currMsg));
                }
                else if (currMsg.Text.Contains("/help") || currMsg.Text == "Help")
                {
                    Task taskA = Task.Run(() =>
                    {
                        string strStartMessage =
                        ServiceTools.ReadTextFromFile(Directory.GetCurrentDirectory() +
                                                      Path.DirectorySeparatorChar + "settings" +
                                                      Path.DirectorySeparatorChar + "BotHelpMessage.txt");
                        Bot.SendTextMessageAsync(currMsg.Chat.Id, strStartMessage, true, false,
                            currMsg.MessageId, BasicKeyboard());
                        Console.WriteLine("Echo Message: {0}", currMsg.Text);
                    });
                }
                else
                {
                    // await Bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                    // await Task.Delay(200);

                    Task taskA = Task.Run(() =>
                    {
                        Bot.SendTextMessageAsync(currMsg.Chat.Id,
                            "Sorry, I can`t understand you. Please try again using the following list of commands:" +
                            Environment.NewLine + "/current_image" +
                            Environment.NewLine + "/concurrent_info" +
                            Environment.NewLine + "/current_cc" +
                            Environment.NewLine + "/meteo_info" +
                            Environment.NewLine + "/fix",
                            replyMarkup: BasicKeyboard());

                        Console.WriteLine("Echo Message: {0}", currMsg.Text);
                    });
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
                          "Total cloud cover: " + data.PredictedCC.CloudCoverTotal + " (of 8)" + Environment.NewLine +
                          Environment.NewLine +
                          "SDC predictions probabilities:" + Environment.NewLine;

                string strToShowSDCs = Environment.NewLine +
                                       "|  NoSun  |  Sun0   |  Sun1   |  Sun2   |" + Environment.NewLine + "" +
                                       "|" +
                                       String.Format("{0,9}",
                                           (data.sdcDecisionProbabilities.First(
                                               prob => prob.sdc == SunDiskCondition.NoSun).sdcDecisionProbability * 100.0d)
                                               .ToString("F2") + "%") + "|" +
                                       String.Format("{0,9}",
                                           (data.sdcDecisionProbabilities.First(
                                               prob => prob.sdc == SunDiskCondition.Sun0).sdcDecisionProbability * 100.0d)
                                               .ToString("F2") + "%") + "|" +
                                       String.Format("{0,9}",
                                           (data.sdcDecisionProbabilities.First(
                                               prob => prob.sdc == SunDiskCondition.Sun1).sdcDecisionProbability * 100.0d)
                                               .ToString("F2") + "%") + "|" +
                                       String.Format("{0,9}",
                                           (data.sdcDecisionProbabilities.First(
                                               prob => prob.sdc == SunDiskCondition.Sun2).sdcDecisionProbability * 100.0d)
                                               .ToString("F2") + "%") + "|";
                retStr += strToShowSDCs;
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
                    (LufftWSdata)ServiceTools.ReadObjectFromXML(lWSxmlFilesInfo.Last().FullName, typeof(LufftWSdata));
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






        #region data fixing mechs

        private List<UserDataFixingDialogs> lDialogs = new List<UserDataFixingDialogs>();



        private async Task<bool> RegisterFixStage1(Message msg)
        {
            //check if this user is able to fix SDC

            Chat currChat = msg.Chat;

            InlineKeyboardMarkup kb = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton("SDC", "varnameSDC"),
                    new InlineKeyboardButton("TCC", "varnameTCC")
                }
            });

            UserDataFixingDialogs currDialog = new UserDataFixingDialogs()
            {
                origMessage = msg,
                userID = msg.Chat.Id,
                userName = msg.Chat.Username,
                userFirstName = msg.Chat.FirstName,
                userLastName = msg.Chat.LastName,
                chatTitle = msg.Chat.Title
            };

            Message sentMsg = await Bot.SendTextMessageAsync(msg.Chat.Id, "Please choose what would you like to fix",
                replyMarkup: kb);

            currDialog.varChoosingMessage = sentMsg;
            lDialogs.Add(currDialog);

            return true;
        }




        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            CallbackQuery currQuery = callbackQueryEventArgs.CallbackQuery;

            //search for dialog var choosing message
            bool bFoundByVarChoosingMessage =
                lDialogs.Any(dlg => dlg.varChoosingMessage.MessageId == currQuery.Message.MessageId);
            List<UserDataFixingDialogs> lDialogsWithValueRequestMessages =
                lDialogs.Where(dlg => dlg.valueRequestMessage != null).ToList();
            bool bFoundByValueRequestMessage =
                lDialogsWithValueRequestMessages.Any(
                    dlg => dlg.valueRequestMessage.MessageId == currQuery.Message.MessageId);

            if (!bFoundByVarChoosingMessage && !bFoundByValueRequestMessage)
            {
                string replMessage = "Извините, кажется, я все забыл." + Environment.NewLine +
                                     "Попробуйте заново запросить снимок, информацию о нем и исправить эти данные.";
                await Bot.SendTextMessageAsync(currQuery.Message.Chat.Id, replMessage, true, false,
                    currQuery.Message.MessageId, BasicKeyboard());
                return;
            }

            UserDataFixingDialogs currDialog = null;
            if (bFoundByVarChoosingMessage)
            {
                currDialog =
                    lDialogs.First(
                        dlg => dlg.varChoosingMessage.MessageId == currQuery.Message.MessageId);
            }
            else if (bFoundByValueRequestMessage)
            {
                currDialog =
                    lDialogs.First(
                        dlg => dlg.valueRequestMessage.MessageId == currQuery.Message.MessageId);
            }
            


            if (currDialog.varname == null)
            {
                if ((currQuery.Data == "varnameSDC") || (currQuery.Data == "varnameTCC"))
                {
                    currDialog.varname = currQuery.Data;

                    if (currDialog.varname == "varnameSDC")
                    {
                        InlineKeyboardMarkup kb = new InlineKeyboardMarkup(new[]
                        {
                            new InlineKeyboardButton[]
                            {
                                new InlineKeyboardButton("Cloudy", "SDC_Cloudy"),
                                new InlineKeyboardButton("Sun_0", "SDC_Sun_0"),
                                new InlineKeyboardButton("Sun_1", "SDC_Sun_1"),
                                new InlineKeyboardButton("Sun_2", "SDC_Sun_2"),
                            },
                            new InlineKeyboardButton[]
                            {
                                new InlineKeyboardButton("Defect", "SDC_Defect"),
                            }
                        });

                        Message sentMsg = await Bot.SendTextMessageAsync(currQuery.Message.Chat.Id, "Please choose value",
                            replyMarkup: kb);

                        currDialog.valueRequestMessage = sentMsg;
                        return;
                    }
                    else if (currDialog.varname == "varnameTCC")
                    {
                        InlineKeyboardMarkup kb = new InlineKeyboardMarkup(new[]
                        {
                            new InlineKeyboardButton[]
                            {
                                new InlineKeyboardButton("0", "TCC_value_0"),
                                new InlineKeyboardButton("1", "TCC_value_1"),
                                new InlineKeyboardButton("2", "TCC_value_2"),
                                new InlineKeyboardButton("3", "TCC_value_3"),
                            },
                            new InlineKeyboardButton[]
                            {
                                new InlineKeyboardButton("4", "TCC_value_4"),
                                new InlineKeyboardButton("5", "TCC_value_5"),
                                new InlineKeyboardButton("6", "TCC_value_6"),
                                new InlineKeyboardButton("7", "TCC_value_7"),
                            },
                            new InlineKeyboardButton[]
                            {
                                new InlineKeyboardButton("8", "TCC_value_8"),
                                new InlineKeyboardButton("Defect", "TCC_Defect"),
                            }
                        });

                        Message sentMsg = await Bot.SendTextMessageAsync(currQuery.Message.Chat.Id, "Please choose value",
                            replyMarkup: kb);

                        currDialog.valueRequestMessage = sentMsg;
                        return;
                    }
                    return;
                }
                else
                {
                    string replMessage = "Извините, меня еще не научили разбирать такие сложные команды.";
                    await Bot.SendTextMessageAsync(currQuery.Message.Chat.Id, replMessage, true, false, currQuery.Message.MessageId, BasicKeyboard());
                    return;
                }
            }
            else
            {
                if (currDialog.varname == "varnameSDC")
                {
                    currDialog.varvalue = currQuery.Data;


                    string replMessage =
                        "Благодарим Вас за участие в обучении нашей модели (SDC). Ваше исправление отправлено на ревизию и будет учтено при дообучении.";

                    await
                        Bot.SendTextMessageAsync(currQuery.Message.Chat.Id, replMessage, true, false,
                            currQuery.Message.MessageId, BasicKeyboard());

                    ServiceTools.WriteObjectToXML(currDialog, Directory.GetCurrentDirectory() +
                                                              Path.DirectorySeparatorChar + "logs" +
                                                              Path.DirectorySeparatorChar +
                                                              "model_fix_" + currDialog.varname + "_" + currDialog.varChoosingMessage.MessageId +
                                                              ".xml");

                    lDialogs.Remove(currDialog);

                    return;
                }
                else if (currDialog.varname == "varnameTCC")
                {
                    currDialog.varvalue = currQuery.Data;


                    string replMessage =
                        "Благодарим Вас за участие в обучении нашей модели (TCC). Ваше исправление отправлено на ревизию и будет учтено при дообучении.";

                    await
                        Bot.SendTextMessageAsync(currQuery.Message.Chat.Id, replMessage, true, false,
                            currQuery.Message.MessageId, BasicKeyboard());

                    ServiceTools.WriteObjectToXML(currDialog, Directory.GetCurrentDirectory() +
                                                              Path.DirectorySeparatorChar + "logs" +
                                                              Path.DirectorySeparatorChar +
                                                              "model_fix_" + currDialog.varname + "_" + currDialog.varChoosingMessage.MessageId +
                                                              ".xml");

                    lDialogs.Remove(currDialog);

                    return;
                }
                else
                {
                    string replMessage = "Извините, меня еще не научили разбирать такие сложные команды.";
                    await Bot.SendTextMessageAsync(currQuery.Message.Chat.Id, replMessage, true, false, currQuery.Message.MessageId, BasicKeyboard());
                    return;
                }
            }
        }

        #endregion data fixing mechs




        #region basic keyboard

        private ReplyKeyboardMarkup BasicKeyboard()
        {
            ReplyKeyboardMarkup kb = new ReplyKeyboardMarkup(new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Image"),
                    new KeyboardButton("Info"),
                    new KeyboardButton("TCC_SDC"),
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Help"),
                    new KeyboardButton("Meteo"),
                }
            });
            kb.ResizeKeyboard = true;
            return kb;
        }

        #endregion basic keyboard




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
                DataStoreDirectory = (string)defaultProperties["CurrentDataStoreDirectory"];
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


        //public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        //{
        //    NeedToStopFlag = true;
        //}
    }



    public class UserDataFixingDialogs
    {
        [XmlIgnore]
        public Message origMessage { get; set; }
        [XmlIgnore]
        public Message varChoosingMessage { get; set; }
        [XmlIgnore]
        public Message valueRequestMessage { get; set; }
        public long userID { get; set; }
        public string userName { get; set; }
        public string userFirstName { get; set; }
        public string userLastName { get; set; }
        public string chatTitle { get; set; }
        public string varname { get; set; }
        public string varvalue { get; set; }

        public UserDataFixingDialogs() { }
    }
}
