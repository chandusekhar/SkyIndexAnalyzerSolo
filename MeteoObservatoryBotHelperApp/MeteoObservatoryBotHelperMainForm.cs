using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using nsoftware.IPWorksSSH;
using nsoftware.IPWorksZip;
using SkyImagesAnalyzerLibraries;

namespace MeteoObservatoryBotHelperApp
{
    public partial class MeteoObservatoryBotHelperMainForm : Form
    {
        private LogWindow theLogWindow = null;

        #region periodical images processing

        private TimeSpan snapshotsProcessingPeriod = new TimeSpan(0, 5, 0);
        private bool restrictSnapshotsProcessingWhenSunElevationLowerThanZero = true;
        ConcurrentExclusiveSchedulerPair scheduler;
        TaskFactory lowPriorityTaskFactory;

        #endregion periodical images processing


        #region CC_Moscow_bot connected properties

        private TimeSpan makeCurrentSnapshotPreviewPicturePeriod = new TimeSpan(0, 1, 0);
        private bool bSendProcessedDataTo_CC_Moscow_bot_server = false;
        private string strRemoteBotServerHost = "krinitsky.ru";
        private int intRemoteBotServerPort = 22;
        private string strRemoteBotServerHostAuthKeyFile = "";
        private string strRemoteBotServerSSHusername = "mk";
        private string strAcceptedSSHhostCertPublicKeyFile = "";

        #endregion CC_Moscow_bot connected properties





        public MeteoObservatoryBotHelperMainForm()
        {
            InitializeComponent();

            scheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, 4);
            lowPriorityTaskFactory = new TaskFactory(scheduler.ConcurrentScheduler);
        }
















        #region periodical snapshot processing for preview snapshots creation



        private void btnSSHsendPreview_Click(object sender, EventArgs e)
        {
            if (theLogWindow == null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow);
            }

            lowPriorityTaskFactory.StartNew(() => MakeAndSendSnapshotsPreviewPictures());
        }



        private void MakeCurrentSnapshotPreviewPicture(object state)
        {
            // stwMakeCurrentSnapshotPreviewPictureTimer.Restart();

            #region проверка на высоту солнца
            bool sunElevationMoreThanZero = true;
            if (latestGPSdata.validGPSdata && restrictSnapshotsProcessingWhenSunElevationLowerThanZero)
            {
                SPA spaCalc = new SPA(latestGPSdata.dateTimeUTC.Year, latestGPSdata.dateTimeUTC.Month, latestGPSdata.dateTimeUTC.Day, latestGPSdata.dateTimeUTC.Hour,
                        latestGPSdata.dateTimeUTC.Minute, latestGPSdata.dateTimeUTC.Second, (float)latestGPSdata.LonDec, (float)latestGPSdata.LatDec,
                        (float)SPAConst.DeltaT(latestGPSdata.dateTimeUTC));
                int res = spaCalc.spa_calculate();
                AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                    spaCalc.spa.zenith);

                if (sunPositionSPAext.ElevationAngle <= 0.0d)
                {
                    sunElevationMoreThanZero = false;
                }
            }

            #endregion проверка на высоту солнца

            if (sunElevationMoreThanZero)
            {
                lowPriorityTaskFactory.StartNew(() => MakeAndSendSnapshotsPreviewPictures());

            }
        }




        private async void MakeAndSendSnapshotsPreviewPictures()
        {
            if (bSendProcessedDataTo_CC_Moscow_bot_server)
            {


                string FilenameToSend = "";
                Image<Bgr, byte> lastImagesCouple = CurrentImagesCouple(out FilenameToSend);
                string tmpFNameToSave = Path.GetTempPath();
                tmpFNameToSave += (tmpFNameToSave.Last() == Path.DirectorySeparatorChar)
                    ? ("")
                    : (Path.DirectorySeparatorChar.ToString());
                tmpFNameToSave += FilenameToSend;
                lastImagesCouple.Save(tmpFNameToSave);
                // var fileStream = File.Open(tmpFNameToSave, FileMode.Open);
                // Message sentMessage = await Bot.SendPhoto(update.Message.Chat.Id, new FileToSend(FilenameToSend, fileStream));
                // послать файл на сервер бота


                string filenameToSend = tmpFNameToSave;
                string concurrentDataXMLfilename =
                    await
                        CommonTools.FindConcurrentDataXMLfileAsync(filenameToSend, strOutputConcurrentDataDirectory,
                            true,
                            ServiceTools.DatetimeExtractionMethod.Filename);

                Zip zip = new Zip();
                string tempZipFilename = Path.GetTempPath();
                tempZipFilename += (tempZipFilename.Last() == Path.DirectorySeparatorChar)
                    ? ("")
                    : (Path.DirectorySeparatorChar.ToString());
                tempZipFilename += Path.GetFileNameWithoutExtension(filenameToSend) + ".zip";
                zip.ArchiveFile = tempZipFilename;
                zip.IncludeFiles(filenameToSend + " | " + concurrentDataXMLfilename);
                zip.Compress();

                //theLogWindow = ServiceTools.LogAText(theLogWindow,
                //    "zip file created: " + Environment.NewLine + tempZipFilename);

                Exception retEx = null;
                bool sendingResult = SendFileToBotServer(tempZipFilename, "~/cc_moscow_bot/" + Path.GetFileName(tempZipFilename), out retEx);

                File.Delete(tmpFNameToSave);
                File.Delete(tempZipFilename);

                #region report error
                if (!sendingResult)
                {
                    if (theLogWindow != null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }
                    else
                    {
                        ServiceTools.logToTextFile(errorLogFilename,
                            "ERROR sending file to bot server" + Environment.NewLine + "filename: " + tempZipFilename +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }

                    return;
                }
                #endregion report error

                List<string> commands = new List<string>();
                commands.Add("cd ~/cc_moscow_bot/");
                commands.Add("unzip " + Path.GetFileName(tempZipFilename));
                commands.Add("rm " + Path.GetFileName(tempZipFilename));
                commands.Add("ll");
                bool execResult = ExecSShellCommandsOnBotServer(commands, out retEx);

                #region report error

                if (!execResult)
                {
                    if (theLogWindow != null)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR executing commands on bot server" + Environment.NewLine +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }
                    else
                    {
                        ServiceTools.logToTextFile(errorLogFilename,
                            "ERROR executing commands on bot server" + Environment.NewLine +
                            Environment.NewLine + "Exception messages: " + Environment.NewLine +
                            ServiceTools.GetExceptionMessages(retEx) + "at the code line: " +
                            ServiceTools.CurrentCodeLineDescription());
                    }
                    return;
                }

                #endregion

            }
        }



        #region exec Sshell commands methods

        private bool ExecSShellCommandsOnBotServer(List<string> commands, out Exception retEx)
        {
            retEx = null;

            Sshell sh = new Sshell()
            {
                SSHHost = strRemoteBotServerHost,
                SSHPort = intRemoteBotServerPort,
                SSHAuthMode = SshellSSHAuthModes.amPublicKey,
                SSHUser = strRemoteBotServerSSHusername
            };

            try
            {
                sh.SSHCert = new Certificate(CertStoreTypes.cstPPKFile, strRemoteBotServerHostAuthKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }

            try
            {
                sh.SSHAcceptServerHostKey = new Certificate(CertStoreTypes.cstSSHPublicKeyFile,
                    strAcceptedSSHhostCertPublicKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }


            sh.OnSSHServerAuthentication += Sh_OnSSHServerAuthentication;
            sh.OnStdout += Sh_OnStdout;

            foreach (string command in commands)
            {
                sh.Execute(command);
            }

            return true;
        }



        private void Sh_OnStdout(object sender, SshellStdoutEventArgs e)
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow, e.Text);
        }



        private void Sh_OnSSHServerAuthentication(object sender, SshellSSHServerAuthenticationEventArgs e)
        {
            Sshell sh = sender as Sshell;
            if (!e.Accept)
            {
                try
                {
                    sh.Interrupt();
                    sh.SSHLogoff();
                    sh.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion exec Sshell commands methods




        #region scp file methods

        private bool SendFileToBotServer(string localFile, string remoteFile, out Exception retEx)
        {
            retEx = null;
            if (!File.Exists(strRemoteBotServerHostAuthKeyFile))
            {
                retEx =
                    new FileNotFoundException("unable to locate bot server auth key file: " +
                                              strRemoteBotServerHostAuthKeyFile);
                return false;
            }

            if (!File.Exists(strAcceptedSSHhostCertPublicKeyFile))
            {
                retEx =
                    new FileNotFoundException("unable to locate bot server accepted host key file: " +
                                              strAcceptedSSHhostCertPublicKeyFile);
                return false;
            }



            Scp scp = new Scp()
            {
                SSHHost = strRemoteBotServerHost,
                SSHPort = intRemoteBotServerPort,
                SSHAuthMode = ScpSSHAuthModes.amPublicKey,
                SSHUser = strRemoteBotServerSSHusername
            };


            try
            {
                scp.SSHCert = new Certificate(CertStoreTypes.cstPPKFile, strRemoteBotServerHostAuthKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }



            try
            {
                scp.SSHAcceptServerHostKey = new Certificate(CertStoreTypes.cstSSHPublicKeyFile,
                    strAcceptedSSHhostCertPublicKeyFile, "", "*");
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }


            scp.OnSSHServerAuthentication += Scp_OnSSHServerAuthentication;
            scp.RemoteFile = remoteFile;
            scp.LocalFile = localFile;
            scp.OnEndTransfer += Scp_OnEndTransfer;

            try
            {
                scp.Upload();
            }
            catch (Exception ex)
            {
                retEx = ex;
                return false;
            }

            return true;

        }


        private void Scp_OnEndTransfer(object sender, ScpEndTransferEventArgs e)
        {
            if (theLogWindow != null)
            {
                theLogWindow = ServiceTools.LogAText(theLogWindow, "file " + e.LocalFile + " transfer finished");
            }
        }



        private void Scp_OnSSHServerAuthentication(object sender, ScpSSHServerAuthenticationEventArgs e)
        {
            Scp scpSender = sender as Scp;
            if (!e.Accept)
            {
                try
                {
                    scpSender.Interrupt();
                    scpSender.SSHLogoff();
                    scpSender.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion scp file methods




        private Image<Bgr, byte> CurrentImagesCouple(out string FilenameToSend)
        {
            DirectoryInfo dir = new DirectoryInfo(outputSnapshotsDirectory);
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




        #endregion periodical snapshot processing for preview snapshots creation




    }
}
