using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkyImagesAnalyzer;
using SkyImagesAnalyzerLibraries;

namespace FilesListCreationUsingFilters
{
    public partial class FilesListCreationUsingFiltersMainForm : Form
    {
        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>();
        private string defaultPropertiesXMLfileName = "";
        private string ImagesBaseSourcePath = "";
        private string ConcurrentAndStatsXMLfilesDir = "";
        private string ObservedDataCSVfile = "";
        private string DestinationPath = "";
        private TimeSpan DateTimeFilterTolerance = new TimeSpan(0, 5, 0);
        private LogWindow theLogWindow = null;



        public FilesListCreationUsingFiltersMainForm()
        {
            InitializeComponent();
        }



        private void FilesListCreationUsingFiltersMainForm_Load(object sender, EventArgs e)
        {
            readDefaultProperties();
        }





        #region default properties mechs

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
            string curDir = Directory.GetCurrentDirectory();


            #region ImagesBaseSourcePath

            if (defaultProperties.ContainsKey("ImagesBaseSourcePath"))
            {
                ImagesBaseSourcePath = (string)defaultProperties["ImagesBaseSourcePath"];
            }
            else
            {
                ImagesBaseSourcePath = curDir;
                defaultProperties.Add("ImagesBaseSourcePath", ImagesBaseSourcePath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbImagesBaseSourcePath, ImagesBaseSourcePath, false);

            #endregion ImagesBaseSourcePath




            #region ConcurrentAndStatsXMLfilesDir

            if (defaultProperties.ContainsKey("ConcurrentAndStatsXMLfilesDir"))
            {
                ConcurrentAndStatsXMLfilesDir = (string)defaultProperties["ConcurrentAndStatsXMLfilesDir"];
            }
            else
            {
                ConcurrentAndStatsXMLfilesDir = curDir;
                defaultProperties.Add("ConcurrentAndStatsXMLfilesDir", ConcurrentAndStatsXMLfilesDir);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbConcurrentAndStatsXMLfilesDir, ConcurrentAndStatsXMLfilesDir, false);

            #endregion ConcurrentAndStatsXMLfilesDir




            #region ObservedDataCSVfile

            if (defaultProperties.ContainsKey("ObservedDataCSVfile"))
            {
                ObservedDataCSVfile = (string)defaultProperties["ObservedDataCSVfile"];
            }
            else
            {
                ObservedDataCSVfile = curDir;
                defaultProperties.Add("ObservedDataCSVfile", ObservedDataCSVfile);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbObservedDataCSVfile, ObservedDataCSVfile, false);

            #endregion ObservedDataCSVfile




            #region DestinationPath

            if (defaultProperties.ContainsKey("DestinationPath"))
            {
                DestinationPath = (string)defaultProperties["DestinationPath"];
            }
            else
            {
                DestinationPath = curDir;
                defaultProperties.Add("DestinationPath", DestinationPath);
                bDefaultPropertiesHasBeenUpdated = true;
            }
            ThreadSafeOperations.SetTextTB(rtbDestinationPath, DestinationPath, false);

            #endregion DestinationPath




            #region DateTimeFilterTolerance

            if (defaultProperties.ContainsKey("DateTimeFilterTolerance"))
            {
                DateTimeFilterTolerance = TimeSpan.Parse((string)defaultProperties["DateTimeFilterTolerance"]);
            }
            else
            {
                defaultProperties.Add("DateTimeFilterTolerance", DateTimeFilterTolerance.ToString());
                bDefaultPropertiesHasBeenUpdated = true;
            }

            #endregion DateTimeFilterTolerance





            if (bDefaultPropertiesHasBeenUpdated)
            {
                saveDefaultProperties();
            }
        }


        private void saveDefaultProperties()
        {
            ServiceTools.WriteDictionaryToXml(defaultProperties, defaultPropertiesXMLfileName, false);
        }


        private void PropertiesFormClosed(object sender, FormClosedEventArgs e)
        {
            readDefaultProperties();
        }



        private void btnProperties_Click(object sender, EventArgs e)
        {
            PropertiesEditor propForm = new PropertiesEditor(defaultProperties, defaultPropertiesXMLfileName);
            propForm.FormClosed += new FormClosedEventHandler(PropertiesFormClosed);
            propForm.ShowDialog();
        }


        #endregion default properties mechs




        private void FINISHED()
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow, "====  FINISHED  ====");
        }




        private void btnCreateList_Click(object sender, EventArgs e)
        {
            theLogWindow = ServiceTools.LogAText(theLogWindow, "Started processing...");

            BackgroundWorker bgwCreateList = new BackgroundWorker();

            bgwCreateList.DoWork += (bgwSender, bgwArgs) =>
            {
                List<ImageFileDescription> lImagesFilesList = new List<ImageFileDescription>();
                List<ImagesProcessingData> lImagesAllConcurrentData = new List<ImagesProcessingData>();
                List<ObservedClCoverData> lObservedData = new List<ObservedClCoverData>();

                #region check available data

                //ImagesBaseSourcePath = "";
                //ConcurrentAndStatsXMLfilesDir = "";
                //ObservedDataCSVfile = "";
                //DestinationPath = "";
                //DateTimeFilterTolerance = new TimeSpan(0, 5, 0);

                #region ImagesBaseSourcePath

                theLogWindow = ServiceTools.LogAText(theLogWindow, "Started enumerating images...");

                if (!Directory.Exists(ImagesBaseSourcePath))
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to find sky-images base source path. Please check if it does exist and contain at least one sky-image *.jpg file.");
                    FINISHED();
                    return;
                }

                lImagesFilesList =
                    Directory.GetFiles(ImagesBaseSourcePath, "*.jpg", SearchOption.AllDirectories)
                        .ToList()
                        .ConvertAll(str => new ImageFileDescription(str));

                if (!lImagesFilesList.Any())
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to find any sky-images. Please check if at least one sky-image exists in base source path.");
                    FINISHED();
                    return;
                }

                #endregion ImagesBaseSourcePath


                #region ConcurrentAndStatsXMLfilesDir

                theLogWindow = ServiceTools.LogAText(theLogWindow, "Started reading concurrent data and stats data using existing XML files...");

                if (!Directory.Exists(ConcurrentAndStatsXMLfilesDir))
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to find sky-images pre-calculated concurrent data XML files directory: " +
                        Environment.NewLine + ConcurrentAndStatsXMLfilesDir + Environment.NewLine +
                        ". Please check if it does exist.");
                    FINISHED();
                    return;
                }



                #region read all concurrent data using all-included XML files

                List<string> lXMLfiles =
                    Directory.GetFiles(ConcurrentAndStatsXMLfilesDir, "ImagesCameraPositioning-stats-*-camID?.xml", SearchOption.AllDirectories).ToList();
                if (!lXMLfiles.Any())
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to find any XML file satisfied the following mask: \"ImagesCameraPositioning-stats-*-camID?.xml\". Please check if at least one XML of that kind does exist in directory " +
                        ConcurrentAndStatsXMLfilesDir);
                    FINISHED();
                    return;
                }

                foreach (string xmlFile in lXMLfiles)
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow, "trying to read and parse file " + xmlFile);

                    try
                    {
                        List<ImagesProcessingData> currFileContents =
                            ServiceTools.ReadObjectFromXML(xmlFile, typeof(List<ImagesProcessingData>)) as
                                List<ImagesProcessingData>;
                        if (currFileContents != null)
                        {
                            lImagesAllConcurrentData.AddRange(currFileContents);
                        }
                    }
                    catch (Exception ex)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR! Unable to read data from file: " + xmlFile + Environment.NewLine + ex.Message);
                        continue;
                    }
                }

                if (!lImagesAllConcurrentData.Any())
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to read any concurrent data. Please check the directory for files containing valid concurrent data XML files: " + Environment.NewLine +
                        "directory: " + ConcurrentAndStatsXMLfilesDir + Environment.NewLine +
                        "XML files mask: \"ImagesCameraPositioning-stats-*-camID?.xml\"");
                    FINISHED();
                    return;
                }

                #endregion read all concurrent data using all-included XML files


                #endregion ConcurrentAndStatsXMLfilesDir



                #region ObservedDataCSVfile

                theLogWindow = ServiceTools.LogAText(theLogWindow, "Started reading observed data CSV file...");

                if (!File.Exists(ObservedDataCSVfile))
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to read observed data CSV file: " + ObservedDataCSVfile);
                    FINISHED();
                    return;
                }

                List<List<string>> lCSVfileContents = ServiceTools.ReadDataFromCSV(ObservedDataCSVfile, 1, true);

                if (!lCSVfileContents.Any())
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to read observed data CSV file: " + ObservedDataCSVfile);
                    FINISHED();
                    return;
                }

                lObservedData = lCSVfileContents.ConvertAll(lStr => new ObservedClCoverData(lStr));

                #endregion ObservedDataCSVfile



                #region DestinationPath

                if (!Directory.Exists(DestinationPath))
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "Unable to find the output directory: " + DestinationPath);
                    FINISHED();
                    return;
                }

                #endregion DestinationPath



                #endregion check available data



                List<Tuple<ObservedClCoverData, ImageFileDescription>> lImagesFilteredByAvailableObservedData =
                    new List<Tuple<ObservedClCoverData, ImageFileDescription>>();


                #region filter images by available observed data using DateTimeFilterTolerance

                theLogWindow = ServiceTools.LogAText(theLogWindow, "Filtering by observed data available...");

                foreach (ObservedClCoverData observedData in lObservedData)
                {
                    DateTime currObservedDatumDateTime = observedData.dt;
                    List<Tuple<ObservedClCoverData, ImageFileDescription>> lImagesCloseToCurrObservedDatum = lImagesFilesList
                        .Where(ifd =>
                        {
                            TimeSpan tspan =
                                new TimeSpan(Math.Abs((ifd.currImageDateTime - currObservedDatumDateTime).Ticks));
                            return (tspan <= DateTimeFilterTolerance);
                        })
                        .ToList()
                        .ConvertAll(ifd => new Tuple<ObservedClCoverData, ImageFileDescription>(observedData, ifd));

                    lImagesFilteredByAvailableObservedData.AddRange(lImagesCloseToCurrObservedDatum);
                }

                #endregion filter images by available observed data using DateTimeFilterTolerance

                List<SkyImagesDataWith_Concurrent_Stats_CloudCover> lImagesFilteredByAnyAvailableData =
                    new List<SkyImagesDataWith_Concurrent_Stats_CloudCover>();

                #region map available stats data using image filename

                theLogWindow = ServiceTools.LogAText(theLogWindow, "Mapping concurrent and stats data...");

                lImagesFilteredByAnyAvailableData = lImagesFilteredByAvailableObservedData.ConvertAll(tpl =>
                {
                    SkyImagesDataWith_Concurrent_Stats_CloudCover retVal = null;
                    try
                    {
                        ImagesProcessingData foundConcurrentData =
                            lImagesAllConcurrentData.Where(ipd => Path.GetFileName(ipd.filename) == tpl.Item2.fileName)
                                .ElementAt(0);
                        retVal = new SkyImagesDataWith_Concurrent_Stats_CloudCover()
                        {
                            skyImageFullFileName = tpl.Item2.fullFileName,
                            skyImageFileName = tpl.Item2.fileName,
                            currImageDateTime = tpl.Item2.currImageDateTime,
                            observedCloudCoverData = tpl.Item1,
                            concurrentDataXMLfile = foundConcurrentData.concurrentDataXMLfile,
                            concurrentData = foundConcurrentData.concurrentData,
                            grixyrgbStatsXMLfile = foundConcurrentData.grixyrgbStatsXMLfile,
                            grixyrgbStats = foundConcurrentData.grixyrgbStats
                        };
                        //(tpl.Item1, tpl.Item2, foundConcurrentData);
                    }
                    catch (Exception ex)
                    {
                        theLogWindow = ServiceTools.LogAText(theLogWindow,
                            "ERROR! Couldn`t find concurrent data for file " + Path.GetFileName(tpl.Item2.fileName) +
                            Environment.NewLine + ex.Message);
                    }
                    return retVal;
                });

                lImagesFilteredByAnyAvailableData.RemoveAll(tpl => tpl == null);

                if (!lImagesFilteredByAnyAvailableData.Any())
                {
                    theLogWindow = ServiceTools.LogAText(theLogWindow,
                        "There is no images remain after filtering using all available data. Output will be empty.");
                }

                #endregion map available stats data using image filename

                theLogWindow = ServiceTools.LogAText(theLogWindow, "Writing output list to file...");

                string strOutputXMLfileName = DestinationPath +
                                              ((DestinationPath.Last() == Path.DirectorySeparatorChar)
                                                  ? ("")
                                                  : (Path.DirectorySeparatorChar.ToString())) + "FilesListToDetectCloudCover.xml";

                ServiceTools.WriteObjectToXML(lImagesFilteredByAnyAvailableData, strOutputXMLfileName);

                theLogWindow = ServiceTools.LogAText(theLogWindow, "images list wrote to file: " + strOutputXMLfileName);
            };


            bgwCreateList.RunWorkerAsync();




        }






        #region Helping classes


        private class ImageFileDescription
        {
            public string fullFileName { get; set; }
            public string fileName { get; set; }
            public DateTime currImageDateTime { get; set; }

            public ImageFileDescription()
            {

            }

            public ImageFileDescription(string fName)
            {
                fullFileName = fName;
                fileName = Path.GetFileName(fName);
                currImageDateTime = ConventionalTransitions.DateTimeOfSkyImageFilename(fName);
            }
        }
        
        #endregion Helping classes
    }
}
