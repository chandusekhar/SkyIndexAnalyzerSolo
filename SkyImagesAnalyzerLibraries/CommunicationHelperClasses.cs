using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nsoftware.IPWorks;

namespace SkyImagesAnalyzerLibraries
{
    public class FileTransfer_SenderChecklist
    {
        #region file itself
        public bool SendingFileMarkerSent { get; set; }
        public bool SendingFileMarkerSentConfirmed { get; set; }

        public bool SendingFile { get; set; }
        public bool FileSent { get; set; }
        public bool FileReceivedBytesConfirmed { get; set; }

        public bool FileSendingFinishedMarkerSent { get; set; }
        public bool FileSendingFinishedMarkerSentConfirmed { get; set; }
        #endregion file itself

        #region filename
        public bool FilenameSendingMarkerSent { get; set; }
        public bool FilenameSendingMarkerSentConfirmed { get; set; }

        public bool FilenameSent { get; set; }
        public bool FilenameSentConfirmed { get; set; }
        #endregion filename

        #region file MD5 hash
        public bool MD5hashSendingMarkerSent { get; set; }
        public bool MD5hashSendingMarkerSentConfirmed { get; set; }

        public bool MD5hashSent { get; set; }
        public bool MD5hashSentConfirmed { get; set; }

        public bool MD5EqualityConfirmationRequestSent { get; set; }
        public bool MD5EqualityReplied { get; set; }
        public bool MD5EqualityOK { get; set; }

        #endregion file MD5 hash




        bool[] bData
        {
            get
            {
                return new bool[]
                {
                        SendingFileMarkerSent,
                        SendingFileMarkerSentConfirmed,
                        SendingFile,
                        FileSent,
                        FileReceivedBytesConfirmed,
                        FileSendingFinishedMarkerSent,
                        FileSendingFinishedMarkerSentConfirmed,
                        FilenameSendingMarkerSent,
                        FilenameSendingMarkerSentConfirmed,
                        FilenameSent,
                        FilenameSentConfirmed,
                        MD5hashSendingMarkerSent,
                        MD5hashSendingMarkerSentConfirmed,
                        MD5hashSent,
                        MD5hashSentConfirmed,
                        MD5EqualityConfirmationRequestSent,
                        MD5EqualityReplied,
                        MD5EqualityOK
                };
            }
        }



        public FileTransfer_SenderChecklist() { }



        public int ToNumeral()
        {
            BitArray binary = new BitArray(bData);
            if (binary == null)
                throw new ArgumentNullException("binary");
            if (binary.Length > 32)
                throw new ArgumentException("must be at most 32 bits long");

            var result = new int[1];
            binary.CopyTo(result, 0);
            return result[0];
        }



        public string ToString()
        {
            List<bool> bDataList = bData.ToList();
            return string.Join("", bDataList.ConvertAll(bVal => ((bVal) ? ("1") : ("0"))));
        }
    }





    public class FileTransfer_ReceiverChecklist
    {

        #region file
        public bool SendingFileMarkerReceived { get; set; }
        public bool SendingFileMarkerReceivedConfirmed { get; set; }

        public bool ReceivingFile { get; set; }
        public bool FileReceived { get; set; }

        public bool FileSendingFinishedMarkerReceived { get; set; }
        public bool FileSendingFinishedMarkerReceivedConfirmed { get; set; }
        #endregion  file

        #region  filename
        public bool FilenameSendingMarkerReceived { get; set; }
        public bool FilenameSendingMarkerReceivedConfirmed { get; set; }

        public bool FilenameReceived { get; set; }
        public bool FilenameReceivedConfirmed { get; set; }
        #endregion  filename

        #region  file MD5 hash
        public bool MD5hashSendingMarkerReceived { get; set; }
        public bool MD5hashSendingMarkerReceivedConfirmed { get; set; }

        public bool MD5hashReceived { get; set; }
        public bool MD5hashReceivedConfirmed { get; set; }

        public bool MD5EqualityConfirmationRequestReceived { get; set; }
        public bool MD5EqualityReplied { get; set; }
        public bool MD5EqualityOK { get; set; }

        #endregion  file MD5 hash

        public bool FileSuccessfullyReceived { get; set; }


        bool[] bData
        {
            get
            {
                return new bool[]
                {
                        SendingFileMarkerReceived,
                        SendingFileMarkerReceivedConfirmed,
                        ReceivingFile,
                        FileReceived,
                        FileSendingFinishedMarkerReceived,
                        FileSendingFinishedMarkerReceivedConfirmed,
                        FilenameSendingMarkerReceived,
                        FilenameSendingMarkerReceivedConfirmed,
                        FilenameReceived,
                        FilenameReceivedConfirmed,
                        MD5hashSendingMarkerReceived,
                        MD5hashSendingMarkerReceivedConfirmed,
                        MD5hashReceived,
                        MD5hashReceivedConfirmed,
                        MD5EqualityConfirmationRequestReceived,
                        MD5EqualityReplied,
                        MD5EqualityOK,
                };
            }
        }



        public FileTransfer_ReceiverChecklist()
        {
        }



        public int ToNumeral()
        {
            BitArray binary = new BitArray(bData);
            if (binary == null)
                throw new ArgumentNullException("binary");
            if (binary.Length > 32)
                throw new ArgumentException("must be at most 32 bits long");

            var result = new int[1];
            binary.CopyTo(result, 0);
            return result[0];
        }



        public string ToString()
        {
            List<bool> bDataList = bData.ToList();
            return string.Join("", bDataList.ConvertAll(bVal => ((bVal) ? ("1") : ("0"))));
        }
    }






    public class Communication_ClientChecklist
    {
        public bool imageFileSent { get; set; }
        public bool WaitingForStatsFile { get; set; }
        public bool statsXMLfileReceived { get; set; }



        bool[] bData
        {
            get
            {
                return new bool[]
                {
                        imageFileSent,
                        WaitingForStatsFile,
                        statsXMLfileReceived
                };
            }
        }



        public Communication_ClientChecklist() { }



        public int ToNumeral()
        {
            BitArray binary = new BitArray(bData);
            if (binary == null)
                throw new ArgumentNullException("binary");
            if (binary.Length > 32)
                throw new ArgumentException("must be at most 32 bits long");

            var result = new int[1];
            binary.CopyTo(result, 0);
            return result[0];
        }



        public string ToString()
        {
            List<bool> bDataList = bData.ToList();
            return string.Join("", bDataList.ConvertAll(bVal => ((bVal) ? ("1") : ("0"))));
        }
    }





    public class Communication_ServerChecklist
    {
        public bool imageFileReceived { get; set; }
        public bool ClientIsWaitingForStatsFile { get; set; }
        public bool statsXMLfileSent { get; set; }


        bool[] bData
        {
            get
            {
                return new bool[]
                {
                        imageFileReceived,
                        ClientIsWaitingForStatsFile,
                        statsXMLfileSent
                };
            }
        }



        public Communication_ServerChecklist()
        {
        }



        public int ToNumeral()
        {
            BitArray binary = new BitArray(bData);
            if (binary == null)
                throw new ArgumentNullException("binary");
            if (binary.Length > 32)
                throw new ArgumentException("must be at most 32 bits long");

            var result = new int[1];
            binary.CopyTo(result, 0);
            return result[0];
        }



        public string ToString()
        {
            List<bool> bDataList = bData.ToList();
            return string.Join("", bDataList.ConvertAll(bVal => ((bVal) ? ("1") : ("0"))));
        }
    }





    public class ServerDataExchangeConnectionDescription
    {
        public string ConnectionID { get; set; }
        public Communication_ServerChecklist serverCommunicationChecklist { get; set; }
        public string ImageFilename { get; set; }
        public IPWorksFileSenderReceiver fileSenderReceiverDescription { get; set; }
    }




    public class ClientDataExchangeConnectionDescription
    {
        public string ConnectionID { get; set; }
        public Communication_ClientChecklist clientCommunicationChecklist { get; set; }
        public string statsXMLfilename { get; set; }
        public IPWorksFileSenderReceiver fileSenderReceiverDescription { get; set; }
    }






    public class FileReceivingConnectionDescription
    {
        public string ConnectionID { get; set; }
        public FileTransfer_ReceiverChecklist FileReceiverCommunicationChecklist { get; set; }
        public MemoryStream incomingFileMemoryStream { get; set; }
        public int IncomingFileSize { get; set; }
        public string IncomingFilename { get; set; }
        public string IncomingFileMD5hash { get; set; }
    }




    public class FileSendingConnectionDescription
    {
        public string ConnectionID { get; set; }
        public FileTransfer_SenderChecklist FileSenderCommunicationChecklist { get; set; }
    }





    public enum FileSenderReceiverRole
    {
        sender,
        receiver
    }





    public delegate void IPWorksFileSenderReceiver_FileTransferFinishedEventHandler(object sender, FileTransferFinishedEventArgs e);

    public class FileTransferFinishedEventArgs
    {
        public bool fileReceivingSuccess { get; set; }
        public string fileReceivedFullName { get; set; }
    }





    public class IPWorksFileSenderReceiver
    {
        public string IncomingsFilesBasePath { get; set; }

        private Ipdaemon ipd = null;
        private Ipport ipclient = null;
        private FileSenderReceiverRole role;
        public FileReceivingConnectionDescription fileReceiverConnection = null;
        public FileSendingConnectionDescription fileSenderConnection = null;
        public bool IsConsoleApp = false;
        public LogWindow theLogWindow = null;

        public event IPWorksFileSenderReceiver_FileTransferFinishedEventHandler FileReceivingFinished;
        public event IPWorksFileSenderReceiver_FileTransferFinishedEventHandler FileSendingFinished;





        public IPWorksFileSenderReceiver(Ipdaemon _ipd, FileSenderReceiverRole _role)
        {
            ipd = _ipd;
            role = _role;

            IsConsoleApp = CommonTools.console_present();
        }


        public IPWorksFileSenderReceiver(Ipport _ipclient, FileSenderReceiverRole _role)
        {
            ipclient = _ipclient;
            role = _role;

            IsConsoleApp = CommonTools.console_present();
        }



        private void LogMessage(string message)
        {
            if (IsConsoleApp) Console.WriteLine(message);
            else if (theLogWindow != null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, message);
            }
        }




        #region file transfer methods


        public async void Ipd_OnDataIn(object sender, IpdaemonDataInEventArgs e)
        {
            if (fileReceiverConnection.ConnectionID != e.ConnectionId)
            {
                return;
            }


            LogMessage("incoming data from " + e.ConnectionId);


            string TextReceived = e.Text;
            string FirstLine = TextReceived.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries).First();
            FirstLine = FirstLine.Replace(Environment.NewLine, "");




            if (role == FileSenderReceiverRole.receiver)
            {
                #region file receiver behaviour

                #region receiving file

                if (!fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceived)
                {
                    if (FirstLine.Contains("<SendingImageFile="))
                    {
                        Console.WriteLine(FirstLine);

                        // extract image file size
                        // IncomingImageFileSize
                        string IncomingFileSizeStr = FirstLine.Replace("<SendingImageFile=", "").Replace(">", "");
                        fileReceiverConnection.IncomingFileSize = Convert.ToInt32(IncomingFileSizeStr);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceived = true;
                        fileReceiverConnection.incomingFileMemoryStream = new MemoryStream();
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceivedConfirmed = true;
                        fileReceiverConnection.incomingFileMemoryStream = new MemoryStream();
                        fileReceiverConnection.FileReceiverCommunicationChecklist.ReceivingFile = true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceived &&
                    fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceivedConfirmed &&
                    fileReceiverConnection.FileReceiverCommunicationChecklist.ReceivingFile)
                {
                    if (FirstLine == "<ImageFileSendingFinished>")
                    {
                        Console.WriteLine(FirstLine);
                        //currentConnection.FileReceiverCommunicationChecklist.ImageFileReceived = true;
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FileSendingFinishedMarkerReceived = true;
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FileSendingFinishedMarkerReceivedConfirmed
                            = true;
                        return;
                    }
                    else if (!fileReceiverConnection.FileReceiverCommunicationChecklist.FileReceived)
                    {
                        fileReceiverConnection.incomingFileMemoryStream.Write(e.TextB, 0, e.TextB.Length);
                        if (fileReceiverConnection.incomingFileMemoryStream.Length == fileReceiverConnection.IncomingFileSize)
                        {
                            fileReceiverConnection.FileReceiverCommunicationChecklist.FileReceived = true;
                        }
                        ipd.SendLine(fileReceiverConnection.ConnectionID,
                            "<BytesReceived=" + fileReceiverConnection.incomingFileMemoryStream.Length + ">");
                        return;
                    }
                }

                #endregion receiving file




                #region receiving filename

                if (fileReceiverConnection.FileReceiverCommunicationChecklist.FileSendingFinishedMarkerReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceived)
                {
                    if (FirstLine == "<SendingFilename>")
                    {
                        Console.WriteLine(FirstLine);
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceived = true;
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceivedConfirmed =
                            true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceived)
                {
                    fileReceiverConnection.IncomingFilename = FirstLine;
                    fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceived = true;
                    ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                    fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceivedConfirmed = true;


                    fileReceiverConnection.IncomingFilename = IncomingsFilesBasePath + fileReceiverConnection.IncomingFilename;
                    FileStream file = new FileStream(fileReceiverConnection.IncomingFilename, FileMode.OpenOrCreate,
                        FileAccess.Write);
                    fileReceiverConnection.incomingFileMemoryStream.WriteTo(file);
                    file.Close();
                    fileReceiverConnection.incomingFileMemoryStream.Close();
                    Console.WriteLine("file received: " + fileReceiverConnection.IncomingFilename);


                    return;
                }

                #endregion receiving filename




                #region receiving file MD5 hash

                if (fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceived)
                {
                    if (FirstLine == "<SendingFileMD5Hash>")
                    {
                        Console.WriteLine(FirstLine);
                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceived = true;
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceivedConfirmed =
                            true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceived)
                {
                    fileReceiverConnection.IncomingFileMD5hash = FirstLine;
                    fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceived = true;
                    ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                    fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceivedConfirmed = true;

                    return;
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityConfirmationRequestReceived)
                {
                    if (FirstLine == "<ImageMD5EqualityConfirmationRequest>")
                    {
                        Console.WriteLine(FirstLine);
                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityConfirmationRequestReceived =
                            true;

                        string fileReceivedMD5hashString =
                            ServiceTools.CalculateMD5hashString(fileReceiverConnection.IncomingFilename);
                        if (fileReceivedMD5hashString == fileReceiverConnection.IncomingFileMD5hash)
                        {
                            ipd.SendLine(fileReceiverConnection.ConnectionID, "MD5OK");
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityReplied = true;
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityOK = true;

                            fileReceiverConnection.FileReceiverCommunicationChecklist.FileSuccessfullyReceived = true;

                            FileReceivingFinished(this, new FileTransferFinishedEventArgs()
                            {
                                fileReceivingSuccess = true,
                                fileReceivedFullName = fileReceiverConnection.IncomingFilename
                            });

                            return;
                        }
                        else
                        {
                            ipd.SendLine(fileReceiverConnection.ConnectionID, "MD5failed");
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityReplied = true;
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityOK = false;
                        }
                        return;
                    }
                }

                #endregion receiving file MD5 hash

                #endregion file receiver behaviour
            }
            else
            {
                #region file sender behaviour



                #endregion file sender behaviour

            }
        }




        private int bytesToSendFileSize = 0;
        public async void SendFile(string filenameToSend, string proposedFilename)
        {
            FileInfo f = new FileInfo(filenameToSend);
            bytesToSendFileSize = Convert.ToInt32(f.Length);
            string fileMD5hashString = ServiceTools.CalculateMD5hashString(filenameToSend);


            ipclient.OnDataIn += Ipclient_OnDataIn;


            #region sending file

            ipclient.SendLine("<SendingImageFile=" + bytesToSendFileSize.ToString() + ">");
            fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSent = true;
            Task<bool> taskSendingFileMarker = Task.Run(WaitForServerResponce);
            // dctCommunicationChecklist.ServerReadyToReceiveImageFile = true
            if (!(await taskSendingFileMarker))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server ready-to-receive-file responce timeout");
                return;
            }



            ipclient.SendFile(filenameToSend);
            fileSenderConnection.FileSenderCommunicationChecklist.SendingFile = true;
            Task<bool> taskSentFileReceivedResponce = Task.Run(WaitForServerResponce);
            fileSenderConnection.FileSenderCommunicationChecklist.FileSent = true;
            if (!(await taskSentFileReceivedResponce))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server file-received-ready responce timeout");
                return;
            }

            ipclient.SendLine("<ImageFileSendingFinished>");
            fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSent = true;
            Task<bool> taskSendingFileFinishedMarker = Task.Run(WaitForServerResponce);
            if (!(await taskSendingFileFinishedMarker))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server ready responce timeout");
                return;
            }

            #endregion sending file




            #region sending filename

            ipclient.SendLine("<SendingFilename>");
            fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSent = true;
            Task<bool> taskSendingFilenameMarker = Task.Run(WaitForServerResponce);
            if (!(await taskSendingFilenameMarker))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server ready-to-receive-filename responce timeout");
                return;
            }


            // ipclient.SendLine("img-2015-06-12T17-09-18devID1.jpg");
            if (proposedFilename == "")
            {
                proposedFilename = Path.GetFileName(filenameToSend);
            }
            ipclient.SendLine(proposedFilename);
            fileSenderConnection.FileSenderCommunicationChecklist.FilenameSent = true;
            Task<bool> taskSendingFilename = Task.Run(WaitForServerResponce);
            if (!(await taskSendingFilename))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server filename-received responce timeout");
                return;
            }

            #endregion sending filename



            #region sending MD5

            ipclient.SendLine("<SendingFileMD5Hash>");
            fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSent = true;
            Task<bool> taskSendingMD5marker = Task.Run(WaitForServerResponce);
            if (!(await taskSendingMD5marker))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server ready-to-receive-MD5 responce timeout");
                return;
            }



            ipclient.SendLine(fileMD5hashString);
            fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSent = true;
            Task<bool> taskSendingMD5string = Task.Run(WaitForServerResponce);
            if (!(await taskSendingMD5string))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server MD5-received responce timeout");
                return;
            }

            #endregion sending MD5



            #region check if MD5 hash equality confirmed

            ipclient.SendLine("<ImageMD5EqualityConfirmationRequest>");
            fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityConfirmationRequestSent = true;
            Task<bool> taskimageMD5EqualityConfirmationRequestWaiting = Task.Run(WaitForServerResponce);
            if (!(await taskimageMD5EqualityConfirmationRequestWaiting))
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "Server Image MD5 Equality Confirmation responce timeout");
                return;
            }

            if (!fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow,
                    "Image file transferred MD5 checksum doesnt match. Will not proceed.");
                return;
            }
            else
            {
                FileSendingFinished(this, new FileTransferFinishedEventArgs()
                {
                    fileReceivingSuccess = true,
                    fileReceivedFullName = filenameToSend
                });
            }
            
            #endregion check if MD5 hash equality confirmed


            ipclient.OnDataIn -= Ipclient_OnDataIn;
        }






        public void Ipclient_OnDataIn(object sender, IpportDataInEventArgs e)
        {
            string TextReceived = e.Text;
            string FirstLine = TextReceived.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries).First();
            FirstLine = FirstLine.Replace(Environment.NewLine, "");


            #region file sending

            if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed)
            {
                if (FirstLine == "OK")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed = true;
                    return;
                }
            }


            if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFile && fileSenderConnection.FileSenderCommunicationChecklist.FileSent && !fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed)
            {
                if (FirstLine.Contains("<BytesReceived="))
                {
                    string ReceivedBytesReportedStr = FirstLine.Replace("<BytesReceived=", "").Replace(">", "");
                    int ReceivedBytesReported = Convert.ToInt32(ReceivedBytesReportedStr);
                    if (ReceivedBytesReported == bytesToSendFileSize)
                    {
                        fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed = true;
                    }
                }
                return;
            }


            if (fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed)
            {
                if (FirstLine == "OK")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed = true;
                    return;
                }
            }

            #endregion file sending



            #region filename sending

            if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed)
            {
                if (FirstLine == "OK")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed = true;
                    return;
                }
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSent && !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSentConfirmed)
            {
                if (FirstLine == "OK")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.FilenameSentConfirmed = true;
                    return;
                }
            }

            #endregion filename sending



            #region MD5 hash sending

            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed)
            {
                if (FirstLine == "OK")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed = true;
                    return;
                }
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSent && !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSentConfirmed)
            {
                if (FirstLine == "OK")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSentConfirmed = true;
                    return;
                }
            }

            #endregion MD5 hash sending



            #region  check if MD5 hash equality confirmed

            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityConfirmationRequestSent && !fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied)
            {
                if (FirstLine == "MD5OK")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied = true;
                    fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK = true;
                    return;
                }
                else if (FirstLine == "MD5failed")
                {
                    Thread.Sleep(100);
                    fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied = true;
                    fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK = false;
                    return;
                }
            }

            #endregion  check if MD5 hash equality confirmed

        }







        private async Task<bool> WaitForServerResponce()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }


            if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFile && fileSenderConnection.FileSenderCommunicationChecklist.FileSent && !fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }




            if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSent && !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSentConfirmed)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.FilenameSentConfirmed)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSent && !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSent && !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSentConfirmed)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSentConfirmed)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }




            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityConfirmationRequestSent && !fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied)
            {
                while (!fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied)
                {
                    if (sw.Elapsed.TotalSeconds > 60)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
                return true;
            }

            return false;
        }







        #endregion file transfer methods


    }
}
