using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkyImagesAnalyzerLibraries;
using System.Reflection;

namespace SortFilesByDates
{
    public class Sorter
    {
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private string FilesBasePath = "";
        private string filesMask = "*.jpg";
        private string filesDateMask = "????xxxxxxxxxxxxxxxxxxx*";
        private bool useFileCreationDateTime = false;

        private string OutputSubdirectoriesPrefix = "";
        private string outputBaseDirectory = "";


        private bool bSearchFilesTopDirectoryOnly = true;
        private bool bStartWithoutConfirmation = false;
        private int totalFilesCountToProcess = 0;
        private int totalFilesProcessed = 0;
        private bool NeedToStopFlag = false;


        private List<FilesOrganizingData> lFilesOrganizing = new List<FilesOrganizingData>();




        public Sorter()
        { }



        public void Start(string[] args)
        {
            readDefaultProperties();

            List<string> argsList = new List<string>(args);



            if (argsList.Count(str => str=="--help") > 0)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "SortFilesByDates.Resources.helpfile.txt";

                Stream stream = assembly.GetManifestResourceStream(resourceName);
                StreamReader reader = new StreamReader(stream);
                string helptext = reader.ReadToEnd();
                
                Console.WriteLine(helptext);
                Console.WriteLine("Press any key...");
                Console.ReadKey();
                return;
            }




            #region bSearchFilesTopDirectoryOnly

            if (argsList.Find(str => str == "--recursive") != null)
            {
                bSearchFilesTopDirectoryOnly = false;
                if (!defaultProperties.ContainsKey("bSearchFilesTopDirectoryOnly"))
                {
                    defaultProperties.Add("bSearchFilesTopDirectoryOnly", bSearchFilesTopDirectoryOnly);
                }
            }

            #endregion bSearchFilesTopDirectoryOnly



            #region StartWithoutConfirmation

            if (argsList.Find(str => str == "-y") != null)
            {
                bStartWithoutConfirmation = true;
                defaultProperties.Add("StartWithoutConfirmation", bStartWithoutConfirmation);
            }

            #endregion StartWithoutConfirmation



            #region filesMask

            if (argsList.Count(str => str.Contains("--filenames-mask=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--filenames-mask="));


                filesMask = arg.Replace("--filenames-mask=", "");

                if (defaultProperties.ContainsKey("filesMask"))
                {
                    defaultProperties["filesMask"] = filesMask;
                }
                else
                {
                    defaultProperties.Add("filesMask", filesMask);
                }
            }

            #endregion filesMask

            

            #region filesDateMask

            if (argsList.Count(str => str.Contains("--file-date-mask=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--file-date-mask="));


                filesDateMask = arg.Replace("--file-date-mask=", "");

                if (filesDateMask == "creation")
                {
                    useFileCreationDateTime = true;
                }

                if (defaultProperties.ContainsKey("filesDateMask"))
                {
                    defaultProperties["filesDateMask"] = filesDateMask;
                }
                else
                {
                    defaultProperties.Add("filesDateMask", filesDateMask);
                }
            }




            #endregion filesDateMask




            #region --FilesBasePath=

            if (argsList.Count(str => str.Contains("--FilesBasePath=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--FilesBasePath="));


                FilesBasePath = arg.Replace("--FilesBasePath=", "");

                if (defaultProperties.ContainsKey("FilesBasePath"))
                {
                    defaultProperties["FilesBasePath"] = FilesBasePath;
                }
                else
                {
                    defaultProperties.Add("FilesBasePath", FilesBasePath);
                }
            }

            #endregion

            

            #region --OutputBaseDirectory=

            if (argsList.Count(str => str.Contains("--OutputBaseDirectory=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--OutputBaseDirectory="));


                outputBaseDirectory = arg.Replace("--OutputBaseDirectory=", "");

                if (outputBaseDirectory == "same")
                {
                    outputBaseDirectory = FilesBasePath;
                }

                if (defaultProperties.ContainsKey("outputBaseDirectory"))
                {
                    defaultProperties["outputBaseDirectory"] = outputBaseDirectory;
                }
                else
                {
                    defaultProperties.Add("outputBaseDirectory", outputBaseDirectory);
                }
            }

            #endregion

            

            #region --OutputSubdirectoriesPrefix=

            if (argsList.Count(str => str.Contains("--OutputSubdirectoriesPrefix=")) > 0)
            {
                string arg = argsList.First(str => str.Contains("--OutputSubdirectoriesPrefix="));


                OutputSubdirectoriesPrefix = arg.Replace("--OutputSubdirectoriesPrefix=", "");

                if (defaultProperties.ContainsKey("OutputSubdirectoriesPrefix"))
                {
                    defaultProperties["OutputSubdirectoriesPrefix"] = OutputSubdirectoriesPrefix;
                }
                else
                {
                    defaultProperties.Add("OutputSubdirectoriesPrefix", OutputSubdirectoriesPrefix);
                }
            }

            #endregion





            CommonTools.PrintDictionaryToConsole(defaultProperties, "effective properties list");
            Console.WriteLine("");



            if (!bStartWithoutConfirmation)
            {
                Console.Write("Start with the mentioned properties? [y/n] ");
                string strReply = Console.ReadLine();
                if (strReply.ToLower().First() == 'y')
                {
                    OrganizeFiles();
                }
                else
                {
                    Console.WriteLine("\nWill not proceed due to user interruprion.");
                    Console.WriteLine("===FINISHED===");
                    Console.WriteLine("Press any key...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                OrganizeFiles();
            }


            Console.WriteLine("===FINISHED===");
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }






        private void OrganizeFiles()
        {
            EnumerateFilesToProcess();


            int filesProcesseed = 0;
            int totalFilesCount = lFilesOrganizing.Count;
            foreach (FilesOrganizingData organizingData in lFilesOrganizing)
            {
                // extract date of file using filesDateMask
                filesProcesseed++;

                string strDateOfFile =
                    string.Concat(
                        Path.GetFileName(organizingData.filename)
                            .Zip(filesDateMask, (c, s) => (s == 'x') ? (c.ToString()) : (""))
                            .ToArray());

                string movetoDir = "";

                if (useFileCreationDateTime)
                {
                    DateTime dt = new FileInfo(organizingData.filename).CreationTimeUtc;
                    DateTime dateDay = dt.Date;

                    movetoDir = OutputSubdirectoriesPrefix + dateDay.ToString("yyyy-MM-dd");
                }
                else
                {
                    /// TODO: сделать что-то, если по маске неверно получили строку даты
                    /// хотя бы чтобы все не падало

                    DateTime dt = CommonTools.DateTimeOfString(strDateOfFile);
                    DateTime dateDay = dt.Date;

                    movetoDir = OutputSubdirectoriesPrefix + dateDay.ToString("yyyy-MM-dd");
                }

                
                movetoDir = outputBaseDirectory +
                            ((outputBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                ? ("")
                                : (Path.DirectorySeparatorChar.ToString())) + movetoDir + Path.DirectorySeparatorChar;
                string movetoFullFilename = movetoDir + Path.GetFileName(organizingData.filename);

                if (!ServiceTools.CheckIfDirectoryExists(movetoDir))
                {
                    continue;
                }


                double perc = Math.Round(((double) filesProcesseed/(double) totalFilesCount)*100.0d, 2);

                if (!File.Exists(movetoFullFilename))
                {
                    FileInfo fileinfo = new FileInfo(organizingData.filename);
                    fileinfo.MoveTo(movetoFullFilename);

                    Console.WriteLine("{0}% : moving {1}", perc, Path.GetFileName(organizingData.filename));
                }
                else
                {
                    Console.WriteLine("{0}% : skipping {1}", perc, Path.GetFileName(organizingData.filename));
                    organizingData.State = FilesOrganizingState.Skipped;
                }
                
                organizingData.MovedToFilename = movetoDir + Path.GetFileName(organizingData.filename);
            }

            Console.WriteLine("Moved {0} files", lFilesOrganizing.Count(fod => fod.State == FilesOrganizingState.Moved));
            Console.WriteLine("Skipped {0} files", lFilesOrganizing.Count(fod => fod.State == FilesOrganizingState.Skipped));
        }






        private void EnumerateFilesToProcess()
        {
            //FilesToProcessMask = currPath2Process +
            //                     ((currPath2Process.Last() == Path.DirectorySeparatorChar)
            //                         ? ("")
            //                         : (Path.DirectorySeparatorChar.ToString()))
            //                    + "*.jpg";

            string directory = Path.GetDirectoryName(FilesBasePath);
            string filemask = filesMask;
            List<string> filesList =
                new List<string>(Directory.EnumerateFiles(directory, filemask,
                    bSearchFilesTopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));

            Console.WriteLine("found " + filesList.Count + " files.");

            if (!filesList.Any())
            {
                Console.WriteLine("There is no " + filemask + " files that sutisfy settings specified. Processing will not be started.");
                return;
            }


            int idx = 0;
            int currPerc = 0;
            int prevPerc = 0;
            int consoleWidth = Console.WindowWidth;
            foreach (string fName in filesList)
            {
                string strToShow = String.Format("{0," + consoleWidth + "}", "adding: " + fName);
                Console.WriteLine(strToShow);
                //Console.SetCursorPosition(0, Console.CursorTop - 1);

                lFilesOrganizing.Add(new FilesOrganizingData()
                {
                    filename = fName,
                    State = FilesOrganizingState.Queued
                });

                idx++;

//#if DEBUG
//                if (idx >= 10)
//                {
//                    Console.WriteLine("\n");
//                    break;
//                }
//#endif
            }


            Console.WriteLine("finished enumerating files. Files to process: " + lFilesOrganizing.Count);
        }






        #region default properties mechanics

        /// <summary>
        /// Reads the default properties.
        /// </summary>
        private void readDefaultProperties()
        {
            defaultProperties = new Dictionary<string, object>();
            defaultPropertiesXMLfileName = Directory.GetCurrentDirectory() +
                                           Path.DirectorySeparatorChar + "settings" + Path.DirectorySeparatorChar +
                                           Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) +
                                           "-Settings.xml";

            if (File.Exists(defaultPropertiesXMLfileName))
            {
                defaultProperties = ServiceTools.ReadDictionaryFromXML(defaultPropertiesXMLfileName);
            }


            bool bDefaultPropertiesHasBeenUpdated = false;

            string CurDir = Directory.GetCurrentDirectory();
            FilesBasePath = "";



            #region FilesBasePath

            if (defaultProperties.ContainsKey("FilesBasePath"))
            {
                FilesBasePath = (string)defaultProperties["FilesBasePath"];
            }
            else
            {
                FilesBasePath = CurDir;
                defaultProperties.Add("FilesBasePath", FilesBasePath);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion ImagesBasePath




            #region bSearchFilesTopDirectoryOnly

            if (defaultProperties.ContainsKey("bSearchFilesTopDirectoryOnly"))
            {
                bSearchFilesTopDirectoryOnly = (((string)defaultProperties["bSearchFilesTopDirectoryOnly"]).ToLower() == "true");
            }
            else
            {
                bSearchFilesTopDirectoryOnly = false;
                defaultProperties.Add("bSearchFilesTopDirectoryOnly", bSearchFilesTopDirectoryOnly);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion




            #region outputBaseDirectory

            // outputBaseDirectory
            if (defaultProperties.ContainsKey("outputBaseDirectory"))
            {
                outputBaseDirectory = (string)defaultProperties["outputBaseDirectory"];
            }
            else
            {
                outputBaseDirectory = CurDir;
                defaultProperties.Add("outputBaseDirectory", outputBaseDirectory);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion outputNetCDFfilesDirectory



            #region OutputSubdirectoriesPrefix
            //OutputSubdirectoriesPrefix

            if (defaultProperties.ContainsKey("OutputSubdirectoriesPrefix"))
            {
                OutputSubdirectoriesPrefix = (string)defaultProperties["OutputSubdirectoriesPrefix"];
            }
            else
            {
                OutputSubdirectoriesPrefix = "";
                defaultProperties.Add("OutputSubdirectoriesPrefix", OutputSubdirectoriesPrefix);
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion



            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }



        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }

        #endregion default properties mechanics






        #region helper classes

        private enum FilesOrganizingState
        {
            Queued,
            Moved,
            Skipped,
            Error
        }



        private class FilesOrganizingData
        {
            public string filename { get; set; }
            public FilesOrganizingState State { get; set; }
            public string MovedToFilename { get; set; }
        }

        #endregion helper classes
    }
}
