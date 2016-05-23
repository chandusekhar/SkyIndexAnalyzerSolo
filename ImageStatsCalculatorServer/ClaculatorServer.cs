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


namespace ImageStatsCalculatorServer
{
    public class ClaculatorServer
    {
        private bool NeedToStopFlag = false;
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private string IncomingImagesBasePath = "";

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
        


        private Ipdaemon ipd = null;
        public void Start()
        {
            readDefaultProperties();
            
            ipd = new Ipdaemon()
            {
                LocalHost = "192.168.192.200",
                LocalPort = 24,
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
                receiver.IncomingsFilesBasePath = IncomingImagesBasePath;
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

            if (e.fileReceivingSuccess)
            {
                currentCommunication.fileSenderReceiverDescription = null;
                currentCommunication.serverCommunicationChecklist.imageFileReceived = true;
                currentCommunication.serverCommunicationChecklist.ClientIsWaitingForStatsFile = true;
                currentCommunication.ImageFilename = e.fileReceivedFullName;

                Task.Run(() => CalculateAndSendImageStats(currentCommunication));
                return;
            }
            else
            {
                Connection currIPdaemonConnection =
                    ipd.Connections.Values.First(cnt => cnt.ConnectionId == currentCommunication.ConnectionID);
                Console.WriteLine("ERROR. Failed incoming image file transfer for connection " +
                                  currentCommunication.ConnectionID + " from IP=" + currIPdaemonConnection.RemoteHost);
                currentCommunication.serverCommunicationChecklist.imageFileReceived = false;
                ipd.Disconnect(currentCommunication.ConnectionID);
            }

        }



        private async void CalculateAndSendImageStats(ServerDataExchangeConnectionDescription currentConnection)
        {
            SkyImageIndexesStatsData receivedFileStats = await CalculateimageStats(currentConnection.ImageFilename);
            string strStatsXMLfilename =
                ConventionalTransitions.ImageGrIxYRGBstatsDataFileName(currentConnection.ImageFilename,
                    IncomingImagesBasePath, true) + "1";
            ServiceTools.WriteObjectToXML(receivedFileStats, strStatsXMLfilename);


            ipd.SendFile(currentConnection.ConnectionID, strStatsXMLfilename);
            ipd.Disconnect(currentConnection.ConnectionID);
            lConnectedClientsDescriptors.Remove(currentConnection);
            currentConnection = null;
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
                IncomingImagesBasePath = (string)defaultProperties["IncomingImagesBasePath"];
            }
            else
            {
                IncomingImagesBasePath = CurDir + Path.DirectorySeparatorChar + "IncomingImages" + Path.DirectorySeparatorChar;
                defaultProperties.Add("IncomingImagesBasePath", IncomingImagesBasePath);

                ServiceTools.CheckIfDirectoryExists(IncomingImagesBasePath);

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




        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            NeedToStopFlag = true;
        }
    }
}
