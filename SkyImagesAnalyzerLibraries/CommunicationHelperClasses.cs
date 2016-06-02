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
    public delegate void FileTransfer_SenderChecklist_ValuesModified_Handler(object sender, FileTransfer_SenderChecklist_ValuesModified_EventArgs e);
    public class FileTransfer_SenderChecklist_ValuesModified_EventArgs
    {
        public string VarNameModified { get; set; }
    }

    public class FileTransfer_SenderChecklist
    {
        public event FileTransfer_SenderChecklist_ValuesModified_Handler ValueModified;

        #region file itself
        public bool SendingFileMarkerSent { get; set; }
        private bool _SendingFileMarkerSentConfirmed = false;
        public bool SendingFileMarkerSentConfirmed
        {
            get { return _SendingFileMarkerSentConfirmed; }
            set
            {
                _SendingFileMarkerSentConfirmed = value;
                ValueModified(this, new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                {
                    VarNameModified = "SendingFileMarkerSentConfirmed"
                });
            }
        }
        
        public bool SendingFile { get; set; }
        public bool FileSent { get; set; }
        private bool _FileReceivedBytesConfirmed = false;
        public bool FileReceivedBytesConfirmed
        {
            get { return _FileReceivedBytesConfirmed; }
            set
            {
                _FileReceivedBytesConfirmed = value;
                ValueModified(this, new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                {
                    VarNameModified = "FileReceivedBytesConfirmed"
                });
            }
        }

        public bool FileSendingFinishedMarkerSent { get; set; }
        private bool _FileSendingFinishedMarkerSentConfirmed = false;
        public bool FileSendingFinishedMarkerSentConfirmed {
            get { return _FileSendingFinishedMarkerSentConfirmed; }
            set
            {
                _FileSendingFinishedMarkerSentConfirmed = value;
                ValueModified(this, new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                {
                    VarNameModified = "FileSendingFinishedMarkerSentConfirmed"
                });
            } }
        #endregion file itself

        #region filename
        public bool FilenameSendingMarkerSent { get; set; }
        private bool _FilenameSendingMarkerSentConfirmed = false;
        public bool FilenameSendingMarkerSentConfirmed
        {
            get { return _FilenameSendingMarkerSentConfirmed; }
            set
            {
                _FilenameSendingMarkerSentConfirmed = value;
                ValueModified(this, new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                {
                    VarNameModified = "FilenameSendingMarkerSentConfirmed"
                });
            }
        }

        public bool FilenameSent { get; set; }
        private bool _FilenameSentConfirmed = false;
        public bool FilenameSentConfirmed {
            get { return _FilenameSentConfirmed; }
            set
            {
                _FilenameSentConfirmed = value;
                ValueModified(this,
                    new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                    {
                        VarNameModified = "FilenameSentConfirmed"
                    });
            } }
        #endregion filename

        #region file MD5 hash
        public bool MD5hashSendingMarkerSent { get; set; }
        private bool _MD5hashSendingMarkerSentConfirmed = false;
        public bool MD5hashSendingMarkerSentConfirmed
        {
            get { return _MD5hashSendingMarkerSentConfirmed; }
            set
            {
                _MD5hashSendingMarkerSentConfirmed = value;
                ValueModified(this, new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                {
                    VarNameModified = "MD5hashSendingMarkerSentConfirmed"
                });
            }
        }

        public bool MD5hashSent { get; set; }
        private bool _MD5hashSentConfirmed = false;
        public bool MD5hashSentConfirmed
        {
            get { return _MD5hashSentConfirmed; }
            set
            {
                _MD5hashSentConfirmed = value;
                ValueModified(this, new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                {
                    VarNameModified = "MD5hashSentConfirmed"
                });
            }
        }

        public bool MD5EqualityConfirmationRequestSent { get; set; }
        private bool _MD5EqualityReplied = false;
        public bool MD5EqualityReplied
        {
            get { return _MD5EqualityReplied; }
            set
            {
                _MD5EqualityReplied = value;
                ValueModified(this, new FileTransfer_SenderChecklist_ValuesModified_EventArgs()
                {
                    VarNameModified = "MD5EqualityReplied"
                });
            }
        }
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
        public bool SourceFileSent { get; set; }
        public bool WaitingForResponceFile { get; set; }
        public bool responceFileReceived { get; set; }

        public string strImageFilename { get; set; }
        public string strReturnedStatsDataXMLfilename { get; set; }
        public string strConcurrentDataXMLfilename { get; set; }


        bool[] bData
        {
            get
            {
                return new bool[]
                {
                        SourceFileSent,
                        WaitingForResponceFile,
                        responceFileReceived
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
        public string IncomingFilename { get; set; }
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
        public bool fileTransferSuccess { get; set; }
        public string fileTransferredFullName { get; set; }
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





        
        private int bytesToSendFileSize = 0;
        private string _filenameToSend = "";
        private string _proposedFilename = "";
        public async void SendFile(string filenameToSend, string proposedFilename)
        {
            _filenameToSend = filenameToSend;
            _proposedFilename = proposedFilename;
            FileInfo f = new FileInfo(filenameToSend);
            bytesToSendFileSize = Convert.ToInt32(f.Length);
            
            if (ipclient != null) ipclient.OnDataIn += Ipclient_OnDataIn;
            else ipd.OnDataIn += Ipd_OnDataIn;

            fileSenderConnection.FileSenderCommunicationChecklist.ValueModified += FileSenderCommunicationChecklist_ValueModified;


            #region start sending file

            if (ipclient != null) ipclient.SendLine("<SendingFile=" + bytesToSendFileSize.ToString() + ">");
            else ipd.SendLine(fileSenderConnection.ConnectionID, "<SendingFile=" + bytesToSendFileSize.ToString() + ">");

            fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSent = true;

            #endregion start sending file
        }







        private void FileSenderCommunicationChecklist_ValueModified(object sender, FileTransfer_SenderChecklist_ValuesModified_EventArgs e)
        {
            LogMessage(e.VarNameModified);


            #region sending file
            if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSent && (e.VarNameModified == "SendingFileMarkerSentConfirmed"))
            {
                Thread.Sleep(100);
                

                string filenameToSend = _filenameToSend;

                if (ipclient != null) ipclient.SendFile(filenameToSend);
                else ipd.SendFile(fileSenderConnection.ConnectionID, filenameToSend);
                fileSenderConnection.FileSenderCommunicationChecklist.SendingFile = true;
                fileSenderConnection.FileSenderCommunicationChecklist.FileSent = true;
                return;
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFile && fileSenderConnection.FileSenderCommunicationChecklist.FileSent && (e.VarNameModified == "FileReceivedBytesConfirmed"))
            {
                Thread.Sleep(100);

                if (ipclient != null) ipclient.SendLine("<FileSendingFinished>");
                else ipd.SendLine(fileSenderConnection.ConnectionID, "<FileSendingFinished>");
                fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSent = true;

                return;
            }

            #endregion sending file



            #region sending filename

            if (fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSent && (e.VarNameModified == "FileSendingFinishedMarkerSentConfirmed"))
            {
                Thread.Sleep(100);
                if (ipclient != null) ipclient.SendLine("<SendingFilename>");
                else ipd.SendLine(fileSenderConnection.ConnectionID, "<SendingFilename>");
                fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSent = true;

                return;
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSent && (e.VarNameModified == "FilenameSendingMarkerSentConfirmed"))
            {
                Thread.Sleep(100);

                string filenameToSend = _filenameToSend;
                string proposedFilename = _proposedFilename;
                if (proposedFilename == "")
                {
                    proposedFilename = Path.GetFileName(filenameToSend);
                }

                if (ipclient != null) ipclient.SendLine(proposedFilename);
                else ipd.SendLine(fileSenderConnection.ConnectionID, proposedFilename);
                fileSenderConnection.FileSenderCommunicationChecklist.FilenameSent = true;

                return;
            }

            #endregion sending filename



            #region sending MD5

            if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSent && (e.VarNameModified == "FilenameSentConfirmed"))
            {
                Thread.Sleep(100);

                if (ipclient != null) ipclient.SendLine("<SendingFileMD5Hash>");
                else ipd.SendLine(fileSenderConnection.ConnectionID, "<SendingFileMD5Hash>");
                fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSent = true;

                return;
            }




            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSent && (e.VarNameModified == "MD5hashSendingMarkerSentConfirmed"))
            {
                Thread.Sleep(100);

                string filenameToSend = _filenameToSend;
                string fileMD5hashString = ServiceTools.CalculateMD5hashString(filenameToSend);
                if (ipclient != null) ipclient.SendLine(fileMD5hashString);
                else ipd.SendLine(fileSenderConnection.ConnectionID, fileMD5hashString);

                fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSent = true;

                return;
            }

            #endregion sending MD5



            #region check if MD5 hash equality confirmed

            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSent && (e.VarNameModified == "MD5hashSentConfirmed"))
            {
                Thread.Sleep(100);

                if (ipclient != null) ipclient.SendLine("<MD5EqualityConfirmationRequest>");
                else ipd.SendLine(fileSenderConnection.ConnectionID, "<MD5EqualityConfirmationRequest>");
                fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityConfirmationRequestSent = true;

                return;
            }



            if (fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityConfirmationRequestSent && (e.VarNameModified == "MD5EqualityReplied"))
            {
                Thread.Sleep(100);

                if (!fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK)
                {
                    LogMessage("File transferred MD5 checksum doesnt match. Will not proceed.");
                    return;
                }
                else
                {
                    FileSendingFinished(this, new FileTransferFinishedEventArgs()
                    {
                        fileTransferSuccess = true,
                        fileTransferredFullName = _filenameToSend
                    });
                }



                if (ipclient != null) ipclient.OnDataIn -= Ipclient_OnDataIn;
                else ipd.OnDataIn -= Ipd_OnDataIn;

                return;
            }
            
            #endregion check if MD5 hash equality confirmed

        }






        public async void Ipd_OnDataIn(object sender, IpdaemonDataInEventArgs e)
        {
            if (role == FileSenderReceiverRole.receiver)
            {
                if (fileReceiverConnection.ConnectionID != e.ConnectionId)
                {
                    return;
                }
            }
            else
            {
                if (fileSenderConnection.ConnectionID != e.ConnectionId)
                {
                    return;
                }
            }


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
                    if (FirstLine.Contains("<SendingFile="))
                    {
                        LogMessage(FirstLine);

                        // extract image file size
                        // IncomingImageFileSize
                        string IncomingFileSizeStr = FirstLine.Replace("<SendingFile=", "").Replace(">", "");
                        fileReceiverConnection.IncomingFileSize = Convert.ToInt32(IncomingFileSizeStr);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceived = true;
                        fileReceiverConnection.incomingFileMemoryStream = new MemoryStream();
                        Thread.Sleep(100);
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceivedConfirmed = true;
                        fileReceiverConnection.FileReceiverCommunicationChecklist.ReceivingFile = true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceived &&
                    fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceivedConfirmed &&
                    fileReceiverConnection.FileReceiverCommunicationChecklist.ReceivingFile)
                {
                    if (FirstLine == "<FileSendingFinished>")
                    {
                        LogMessage(FirstLine);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.FileSendingFinishedMarkerReceived = true;
                        Thread.Sleep(100);
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FileSendingFinishedMarkerReceivedConfirmed
                            = true;
                        return;
                    }
                    else if (!fileReceiverConnection.FileReceiverCommunicationChecklist.FileReceived)
                    {
                        fileReceiverConnection.incomingFileMemoryStream.Write(e.TextB, 0, e.TextB.Length);
                        double perc =
                            Math.Round(
                                ((double) fileReceiverConnection.incomingFileMemoryStream.Length/
                                 (double) fileReceiverConnection.IncomingFileSize)*100.0d, 2);
                        LogMessage("bytes received " + perc.ToString("F2") + "% : " +
                                   fileReceiverConnection.incomingFileMemoryStream.Length + " of " +
                                   fileReceiverConnection.IncomingFileSize);

                        if (fileReceiverConnection.incomingFileMemoryStream.Length == fileReceiverConnection.IncomingFileSize)
                        {
                            fileReceiverConnection.FileReceiverCommunicationChecklist.FileReceived = true;
                            ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        }
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
                        LogMessage(FirstLine);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceived = true;
                        Thread.Sleep(100);
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceivedConfirmed =
                            true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceived)
                {
                    LogMessage(FirstLine);

                    fileReceiverConnection.IncomingFilename = FirstLine;
                    fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceived = true;
                    Thread.Sleep(100);
                    ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                    fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceivedConfirmed = true;


                    fileReceiverConnection.IncomingFilename = IncomingsFilesBasePath + fileReceiverConnection.IncomingFilename;
                    FileStream file = new FileStream(fileReceiverConnection.IncomingFilename, FileMode.OpenOrCreate,
                        FileAccess.Write);
                    fileReceiverConnection.incomingFileMemoryStream.WriteTo(file);
                    file.Close();
                    fileReceiverConnection.incomingFileMemoryStream.Close();
                    LogMessage("file received: " + fileReceiverConnection.IncomingFilename);


                    return;
                }

                #endregion receiving filename




                #region receiving file MD5 hash

                if (fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceived)
                {
                    if (FirstLine == "<SendingFileMD5Hash>")
                    {
                        LogMessage(FirstLine);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceived = true;
                        Thread.Sleep(100);
                        ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceivedConfirmed =
                            true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceived)
                {
                    LogMessage(FirstLine);

                    fileReceiverConnection.IncomingFileMD5hash = FirstLine;
                    fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceived = true;
                    Thread.Sleep(100);
                    ipd.SendLine(fileReceiverConnection.ConnectionID, "OK");
                    fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceivedConfirmed = true;

                    return;
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityConfirmationRequestReceived)
                {
                    if (FirstLine == "<MD5EqualityConfirmationRequest>")
                    {
                        LogMessage(FirstLine);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityConfirmationRequestReceived =
                            true;
                        Thread.Sleep(100);

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
                                fileTransferSuccess = true,
                                fileTransferredFullName = fileReceiverConnection.IncomingFilename
                            });

                            return;
                        }
                        else
                        {
                            ipd.SendLine(fileReceiverConnection.ConnectionID, "MD5failed");
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityReplied = true;
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityOK = false;

                            FileReceivingFinished(this, new FileTransferFinishedEventArgs()
                            {
                                fileTransferSuccess = false,
                                fileTransferredFullName = fileReceiverConnection.IncomingFilename
                            });
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

                LogMessage(FirstLine);

                #region file sending

                if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed = true;
                        return;
                    }
                }


                if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFile &&
                    fileSenderConnection.FileSenderCommunicationChecklist.FileSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed = true;
                    }
                    return;
                }


                if (fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed =
                            true;
                        return;
                    }
                }

                #endregion file sending



                #region filename sending

                if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed = true;
                        return;
                    }
                }



                if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSentConfirmed)
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

                if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed = true;
                        return;
                    }
                }



                if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSentConfirmed)
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

                if (fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityConfirmationRequestSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied)
                {
                    if (FirstLine == "MD5OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK = true;
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied = true;
                        return;
                    }
                    else if (FirstLine == "MD5failed")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK = false;
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied = true;
                        return;
                    }
                }

                #endregion  check if MD5 hash equality confirmed


                #endregion file sender behaviour

            }
        }





        public void Ipclient_OnDataIn(object sender, IpportDataInEventArgs e)
        {
            string TextReceived = e.Text;
            string FirstLine = TextReceived.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries).First();
            FirstLine = FirstLine.Replace(Environment.NewLine, "");

            
            if (role == FileSenderReceiverRole.sender)
            {

                #region file sending behaviour

                LogMessage(FirstLine);

                #region file sending

                if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.SendingFileMarkerSentConfirmed = true;
                        return;
                    }
                }


                if (fileSenderConnection.FileSenderCommunicationChecklist.SendingFile &&
                    fileSenderConnection.FileSenderCommunicationChecklist.FileSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.FileReceivedBytesConfirmed = true;
                    }
                    return;
                }


                if (fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.FileSendingFinishedMarkerSentConfirmed =
                            true;
                        return;
                    }
                }

                #endregion file sending



                #region filename sending

                if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.FilenameSendingMarkerSentConfirmed = true;
                        return;
                    }
                }



                if (fileSenderConnection.FileSenderCommunicationChecklist.FilenameSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.FilenameSentConfirmed)
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

                if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed)
                {
                    if (FirstLine == "OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSendingMarkerSentConfirmed = true;
                        return;
                    }
                }



                if (fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.MD5hashSentConfirmed)
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

                if (fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityConfirmationRequestSent &&
                    !fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied)
                {
                    if (FirstLine == "MD5OK")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK = true;
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied = true;
                        
                        return;
                    }
                    else if (FirstLine == "MD5failed")
                    {
                        Thread.Sleep(100);
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityOK = false;
                        fileSenderConnection.FileSenderCommunicationChecklist.MD5EqualityReplied = true;
                        
                        return;
                    }
                }

                #endregion  check if MD5 hash equality confirmed

                #endregion file sending behaviour

            }
            else
            {

                #region file receiver behaviour

                #region receiving file

                if (!fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceived)
                {
                    if (FirstLine.Contains("<SendingFile="))
                    {
                        LogMessage(FirstLine);

                        string IncomingFileSizeStr = FirstLine.Replace("<SendingFile=", "").Replace(">", "");
                        fileReceiverConnection.IncomingFileSize = Convert.ToInt32(IncomingFileSizeStr);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.SendingFileMarkerReceived = true;
                        fileReceiverConnection.incomingFileMemoryStream = new MemoryStream();
                        ipclient.SendLine("OK");
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
                    if (FirstLine == "<FileSendingFinished>")
                    {
                        LogMessage(FirstLine);

                        //currentConnection.FileReceiverCommunicationChecklist.ImageFileReceived = true;
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FileSendingFinishedMarkerReceived = true;
                        ipclient.SendLine("OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FileSendingFinishedMarkerReceivedConfirmed
                            = true;
                        return;
                    }
                    else if (!fileReceiverConnection.FileReceiverCommunicationChecklist.FileReceived)
                    {
                        fileReceiverConnection.incomingFileMemoryStream.Write(e.TextB, 0, e.TextB.Length);
                        double perc =
                            Math.Round(
                                ((double)fileReceiverConnection.incomingFileMemoryStream.Length /
                                 (double)fileReceiverConnection.IncomingFileSize) * 100.0d, 2);
                        LogMessage("bytes received " + perc.ToString("F2") + "% : " +
                                   fileReceiverConnection.incomingFileMemoryStream.Length + " of " +
                                   fileReceiverConnection.IncomingFileSize);
                        if (fileReceiverConnection.incomingFileMemoryStream.Length == fileReceiverConnection.IncomingFileSize)
                        {
                            Thread.Sleep(100);
                            ipclient.SendLine("OK");
                            fileReceiverConnection.FileReceiverCommunicationChecklist.FileReceived = true;
                        }
                        
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
                        LogMessage(FirstLine);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceived = true;
                        ipclient.SendLine("OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceivedConfirmed =
                            true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameSendingMarkerReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceived)
                {
                    LogMessage(FirstLine);

                    fileReceiverConnection.IncomingFilename = FirstLine;
                    fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceived = true;
                    ipclient.SendLine("OK");
                    fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceivedConfirmed = true;


                    fileReceiverConnection.IncomingFilename = IncomingsFilesBasePath + fileReceiverConnection.IncomingFilename;
                    FileStream file = new FileStream(fileReceiverConnection.IncomingFilename, FileMode.OpenOrCreate,
                        FileAccess.Write);
                    fileReceiverConnection.incomingFileMemoryStream.WriteTo(file);
                    file.Close();
                    fileReceiverConnection.incomingFileMemoryStream.Close();
                    LogMessage("file received: " + fileReceiverConnection.IncomingFilename);


                    return;
                }

                #endregion receiving filename




                #region receiving file MD5 hash

                if (fileReceiverConnection.FileReceiverCommunicationChecklist.FilenameReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceived)
                {
                    if (FirstLine == "<SendingFileMD5Hash>")
                    {
                        LogMessage(FirstLine);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceived = true;
                        ipclient.SendLine("OK");
                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceivedConfirmed =
                            true;
                        return;
                    }
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashSendingMarkerReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceived)
                {
                    LogMessage(FirstLine);

                    fileReceiverConnection.IncomingFileMD5hash = FirstLine;
                    fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceived = true;
                    ipclient.SendLine("OK");
                    fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceivedConfirmed = true;

                    return;
                }


                if (fileReceiverConnection.FileReceiverCommunicationChecklist.MD5hashReceivedConfirmed &&
                    !fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityConfirmationRequestReceived)
                {
                    if (FirstLine == "<MD5EqualityConfirmationRequest>")
                    {
                        LogMessage(FirstLine);

                        fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityConfirmationRequestReceived =
                            true;

                        string fileReceivedMD5hashString =
                            ServiceTools.CalculateMD5hashString(fileReceiverConnection.IncomingFilename);
                        if (fileReceivedMD5hashString == fileReceiverConnection.IncomingFileMD5hash)
                        {
                            ipclient.SendLine("MD5OK");
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityOK = true;
                            fileReceiverConnection.FileReceiverCommunicationChecklist.FileSuccessfullyReceived = true;
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityReplied = true;

                            FileReceivingFinished(this, new FileTransferFinishedEventArgs()
                            {
                                fileTransferSuccess = true,
                                fileTransferredFullName = fileReceiverConnection.IncomingFilename
                            });

                            return;
                        }
                        else
                        {
                            ipclient.SendLine("MD5failed");
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityOK = false;
                            fileReceiverConnection.FileReceiverCommunicationChecklist.MD5EqualityReplied = true;

                            FileReceivingFinished(this, new FileTransferFinishedEventArgs()
                            {
                                fileTransferSuccess = false,
                                fileTransferredFullName = fileReceiverConnection.IncomingFilename
                            });
                        }
                        return;
                    }
                }

                #endregion receiving file MD5 hash

                #endregion file receiver behaviour

            }

        }

        


        #endregion file transfer methods


    }
}
