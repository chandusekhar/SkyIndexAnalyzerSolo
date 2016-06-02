using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SkyImagesAnalyzerLibraries;
using nsoftware.IPWorks;
using nsoftware.IPWorksZip;


namespace ImageStatsCalculatorServer
{
    public class ClaculatorServer
    {
        private bool NeedToStopFlag = false;
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private string IncomingFilesBasePath = "";

        private string logFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          ".log";

        private string errorLogFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "logs" +
                                          Path.DirectorySeparatorChar +
                                          Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                          "-error.log";

        private string ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "settings" +
                                          Path.DirectorySeparatorChar +
                                          "ImagesRoundMasksXMLfilesMappingList.csv";

        private List<ServerDataExchangeConnectionDescription> lConnectedClientsDescriptors = new List<ServerDataExchangeConnectionDescription>();

        private string strLocalHostIP = "192.168.192.200";
        private int ipDaemonPort = 43020;




        private Ipdaemon ipd = null;
        public void Start()
        {
            readDefaultProperties();
            
            ipd = new Ipdaemon()
            {
                LocalHost = strLocalHostIP,
                LocalPort = ipDaemonPort,
                DefaultTimeout = 180
            };
            ipd.Config("InBufferSize=4096");
            ipd.OnConnectionRequest += Ipd_OnConnectionRequest;
            ipd.OnConnected += Ipd_OnConnected;
            ipd.OnDataIn += Ipd_OnDataIn;
            ipd.OnError += Ipd_OnError;
            ipd.OnDisconnected += Ipd_OnDisconnected;
            ipd.Listening = true;



            Loop().Wait();


            ipd.Listening = false;
            ipd.Dispose();
        }





        #region server main behaviour

        private void Ipd_OnDisconnected(object sender, IpdaemonDisconnectedEventArgs e)
        {
            
        }



        private void Ipd_OnError(object sender, IpdaemonErrorEventArgs e)
        {
            Console.WriteLine("ERROR: " + Environment.NewLine + e.Description);
        }


        
        private async void Ipd_OnDataIn(object sender, IpdaemonDataInEventArgs e)
        {
            Console.WriteLine("incoming data from " + e.ConnectionId);

            // selecting the connection by connectionID
            ServerDataExchangeConnectionDescription currentCommunication =
                lConnectedClientsDescriptors.First(cn => cn.ConnectionID == e.ConnectionId);
            
            string TextReceived = e.Text;
            string FirstLine = TextReceived.Split(new string[] {Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries).First();
            FirstLine = FirstLine.Replace(Environment.NewLine, "");

            if (FirstLine == "<SendingFile>")
            {
                // перевести в режим приема файла
                IPWorksFileSenderReceiver receiver = new IPWorksFileSenderReceiver(ipd, FileSenderReceiverRole.receiver);
                receiver.IncomingsFilesBasePath = IncomingFilesBasePath;
                receiver.fileReceiverConnection = new FileReceivingConnectionDescription()
                {
                    ConnectionID = currentCommunication.ConnectionID,
                    FileReceiverCommunicationChecklist = new FileTransfer_ReceiverChecklist()
                };
                receiver.FileReceivingFinished += Receiver_FileReceivingFinished;

                // перенаправлять события с этиим ConnectionId на созданный receiver
                ipd.OnDataIn += receiver.Ipd_OnDataIn;

                currentCommunication.fileSenderReceiverDescription = receiver;
            }
            
            #region sending XML stats file reports

            if (currentCommunication.serverCommunicationChecklist.ClientIsWaitingForStatsFile)
            {
                Console.WriteLine(FirstLine);
                return;
            }

            #endregion sending XML stats file reports
        }




        private void Receiver_FileReceivingFinished(object sender, FileTransferFinishedEventArgs e)
        {
            IPWorksFileSenderReceiver receiver = sender as IPWorksFileSenderReceiver;
            ServerDataExchangeConnectionDescription currentCommunication =
                lConnectedClientsDescriptors.First(cn => cn.ConnectionID == receiver.fileReceiverConnection.ConnectionID);

            // отменить подписку на событие
            ipd.OnDataIn -= receiver.Ipd_OnDataIn;

            if (e.fileTransferSuccess)
            {
                currentCommunication.fileSenderReceiverDescription = null;
                currentCommunication.serverCommunicationChecklist.imageFileReceived = true;
                currentCommunication.serverCommunicationChecklist.ClientIsWaitingForStatsFile = true;
                currentCommunication.IncomingFilename = e.fileTransferredFullName;

                Task.Run(() => CalculateAndSendImageStats(currentCommunication));
                return;
            }
            else
            {
                Connection currIPdaemonConnection =
                    ipd.Connections.Values.First(cnt => cnt.ConnectionId == currentCommunication.ConnectionID);
                Console.WriteLine("ERROR. Failed incoming file transfer for connection " +
                                  currentCommunication.ConnectionID + " from IP=" + currIPdaemonConnection.RemoteHost);
                currentCommunication.serverCommunicationChecklist.imageFileReceived = false;
                ipd.Disconnect(currentCommunication.ConnectionID);
            }

        }



        private async void CalculateAndSendImageStats(ServerDataExchangeConnectionDescription currentConnection)
        {
            // unzip incoming file
            Zip zipExtractor = new Zip();
            zipExtractor.ArchiveFile = currentConnection.IncomingFilename;
            zipExtractor.Scan();
            string ExtractToPath = Path.GetDirectoryName(currentConnection.IncomingFilename);
            ExtractToPath += ((ExtractToPath.Last() == Path.DirectorySeparatorChar)
                ? ("")
                : (Path.DirectorySeparatorChar.ToString())) +
                             Path.GetFileNameWithoutExtension(currentConnection.IncomingFilename) +
                             Path.DirectorySeparatorChar;
            ServiceTools.CheckIfDirectoryExists(ExtractToPath);
            zipExtractor.ExtractToPath = ExtractToPath;
            zipExtractor.ExtractAll();
            
            List<string> filesExtracted = Directory.GetFiles(ExtractToPath).ToList();
            string imageFilename = filesExtracted.First(fName => Path.GetExtension(fName).ToLower() == ".jpg");


            string strStatsXMLfilename =
                ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(imageFilename, ExtractToPath, true);

            if (!File.Exists(strStatsXMLfilename))
            {
                SkyImageIndexesStatsData receivedFileStats = await CalculateimageStats(imageFilename);
                ServiceTools.WriteObjectToXML(receivedFileStats, strStatsXMLfilename);
            }


            zipExtractor.Files.Clear();
            zipExtractor.Files.Add(new ZIPFile(strStatsXMLfilename));
            zipExtractor.AppendFiles();
            zipExtractor.Dispose();
            
            Zip zip = new Zip();
            string tempZipFilename = Path.GetDirectoryName(currentConnection.IncomingFilename);
            tempZipFilename += ((tempZipFilename.Last() == Path.DirectorySeparatorChar)
                ? ("")
                : (Path.DirectorySeparatorChar.ToString())) + Path.GetFileNameWithoutExtension(strStatsXMLfilename) +
                               ".zip";
            zip.ArchiveFile = tempZipFilename;
            zip.IncludeFiles(strStatsXMLfilename);
            zip.Compress();
            zip.Dispose();

            // Directory.Delete(ExtractToPath, true);

            Console.WriteLine("zip file created: " + Environment.NewLine + tempZipFilename);



            ipd.SendLine(currentConnection.ConnectionID, "<SendingFile>");
            Thread.Sleep(200);

            IPWorksFileSenderReceiver imageFileSender = new IPWorksFileSenderReceiver(ipd,
                FileSenderReceiverRole.sender);
            imageFileSender.fileSenderConnection = new FileSendingConnectionDescription()
            {
                ConnectionID = currentConnection.ConnectionID,
                FileSenderCommunicationChecklist = new FileTransfer_SenderChecklist()
            };
            imageFileSender.FileSendingFinished += ImageFileSender_FileSendingFinished;

            imageFileSender.SendFile(tempZipFilename, Path.GetFileName(tempZipFilename));
            
            
        }





        private async Task<SkyImageIndexesStatsData> CalculateimageStats(string ImgFilename)
        {
            Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
            optionalParameters.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            optionalParameters.Add("Stopwatch", sw);
            optionalParameters.Add("logFileName", errorLogFilename);

            Console.WriteLine(DateTime.Now.ToString("s") + " : started processing file " + Environment.NewLine + ImgFilename);

            ImageStatsDataCalculationResult currImageProcessingResult =
                ImageProcessing.CalculateImageStatsData(ImgFilename, optionalParameters);

            currImageProcessingResult.stopwatch.Stop();

            #region log performance
            string strPerformanceCountersStatsFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                     "logs" + Path.DirectorySeparatorChar +
                                                     Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) + "-perf-data.csv";
            string currentFullFileName = currImageProcessingResult.imgFilename;
            string strPerfCountersData = currentFullFileName + ";" +
                                         currImageProcessingResult.stopwatch.ElapsedMilliseconds + ";" +
                                         (currImageProcessingResult.procTotalProcessorTimeEnd -
                                          currImageProcessingResult.procTotalProcessorTimeStart).TotalMilliseconds +
                                         Environment.NewLine;
            ServiceTools.logToTextFile(strPerformanceCountersStatsFile, strPerfCountersData, true);
            #endregion log performance

            return currImageProcessingResult.grixyrgbStatsData;
        }
        



        private void ImageFileSender_FileSendingFinished(object sender, FileTransferFinishedEventArgs e)
        {
            ///TODO: проверить, передалось ли все нормально


            IPWorksFileSenderReceiver currentImageFileSender = sender as IPWorksFileSenderReceiver;
            ServerDataExchangeConnectionDescription currentConnection =
                lConnectedClientsDescriptors.First(
                    cn => cn.ConnectionID == currentImageFileSender.fileSenderConnection.ConnectionID);
            ipd.Disconnect(currentImageFileSender.fileSenderConnection.ConnectionID);
            lConnectedClientsDescriptors.Remove(currentConnection);
            currentConnection = null;

            File.Delete(e.fileTransferredFullName);
        }



        private void Ipd_OnConnected(object sender, IpdaemonConnectedEventArgs e)
        {
            Console.WriteLine("client connected: " + e.Description);

            lConnectedClientsDescriptors.Add(new ServerDataExchangeConnectionDescription()
            {
                ConnectionID = e.ConnectionId,
                serverCommunicationChecklist = new Communication_ServerChecklist()
            });
        }



        private void Ipd_OnConnectionRequest(object sender, IpdaemonConnectionRequestEventArgs e)
        {
            Console.WriteLine("Connection requested: " + e.Address + ":" + e.Port);
            e.Accept = true;
        }

        #endregion server main behaviour




        #region Default properties mechs

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



            #region IncomingImagesBasePath

            if (defaultProperties.ContainsKey("IncomingImagesBasePath"))
            {
                IncomingFilesBasePath = (string)defaultProperties["IncomingImagesBasePath"];
            }
            else
            {
                IncomingFilesBasePath = CurDir + Path.DirectorySeparatorChar + "ImageStatsCalculatorServer_IncomingDirectory" + Path.DirectorySeparatorChar;
                defaultProperties.Add("IncomingImagesBasePath", IncomingFilesBasePath);

                ServiceTools.CheckIfDirectoryExists(IncomingFilesBasePath);

                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion IncomingImagesBasePath



            #region ImagesRoundMasksXMLfilesMappingList

            if (defaultProperties.ContainsKey("ImagesRoundMasksXMLfilesMappingList"))
            {
                ImagesRoundMasksXMLfilesMappingList = (string)defaultProperties["ImagesRoundMasksXMLfilesMappingList"];
            }
            else
            {
                ImagesRoundMasksXMLfilesMappingList = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
                                                      "settings" +
                                                      Path.DirectorySeparatorChar +
                                                      "ImagesRoundMasksXMLfilesMappingList.csv";
                defaultProperties.Add("ImagesRoundMasksXMLfilesMappingList", ImagesRoundMasksXMLfilesMappingList);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion ImagesRoundMasksXMLfilesMappingList



            #region strLocalHostIP

            if (defaultProperties.ContainsKey("strLocalHostIP"))
            {
                strLocalHostIP = (string)defaultProperties["strLocalHostIP"];
            }
            else
            {
                strLocalHostIP = "192.168.192.200";
                defaultProperties.Add("strLocalHostIP", strLocalHostIP);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion strLocalHostIP



            #region ipDaemonPort

            if (defaultProperties.ContainsKey("ipDaemonPort"))
            {
                ipDaemonPort = Convert.ToInt32(defaultProperties["ipDaemonPort"]);
            }
            else
            {
                ipDaemonPort = 43020;
                defaultProperties.Add("ipDaemonPort", ipDaemonPort);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion ipDaemonPort




            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }



        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }

        #endregion Default properties mechs




        private async Task Loop()
        {




            while (true)
            {
                Thread.Sleep(100);

                if (NeedToStopFlag)
                {
                    break;
                }
            }
        }









        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            NeedToStopFlag = true;
        }
    }
}
