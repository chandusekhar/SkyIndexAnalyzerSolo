using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public class ConventionalTransitions
    {

        public static string SunDiskInfoFileName(string imageFullPath, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (xmlFilesBasePath == "")
            {
                FileInfo fInfo1 = new FileInfo(imageFullPath);
                string sunDiskInfoFileName = Path.GetFileNameWithoutExtension(imageFullPath) +
                                             "-SunDiskInfo.xml";
                if (fullPath)
                {
                    sunDiskInfoFileName = fInfo1.DirectoryName + Path.DirectorySeparatorChar + sunDiskInfoFileName;
                }


                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    sunDiskInfoFileName = strNewBaseDirectory +
                                                      Path.GetFileName(sunDiskInfoFileName);
                }



                return sunDiskInfoFileName;
            }
            else
            {
                FileInfo fInfo1 = new FileInfo(imageFullPath);
                string sunDiskInfoFileName = Path.GetFileNameWithoutExtension(imageFullPath) +
                                             "-SunDiskInfo.xml";
                if (fullPath)
                {
                    sunDiskInfoFileName = xmlFilesBasePath +
                                          ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                              ? ("")
                                              : (Path.DirectorySeparatorChar.ToString())) + sunDiskInfoFileName;
                }


                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    sunDiskInfoFileName = strNewBaseDirectory +
                                                      Path.GetFileName(sunDiskInfoFileName);
                }



                return sunDiskInfoFileName;
            }
        }



        public static string SunDiskInfoFileNamesPattern()
        {
            return "*-SunDiskInfo.xml";
        }





        public static string SunDiskConditionFileName(string imageFullPath, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (xmlFilesBasePath == "")
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strSunDiskConditionFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                     "-SunDiskCondition.xml";
                if (fullPath)
                {
                    strSunDiskConditionFileName = currImageFInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                  strSunDiskConditionFileName;
                }


                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strSunDiskConditionFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strSunDiskConditionFileName);
                }



                return strSunDiskConditionFileName;
            }
            else
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strSunDiskConditionFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                     "-SunDiskCondition.xml";
                if (fullPath)
                {
                    strSunDiskConditionFileName = xmlFilesBasePath +
                                                  ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                      ? ("")
                                                      : (Path.DirectorySeparatorChar.ToString())) +
                                                  strSunDiskConditionFileName;
                }


                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strSunDiskConditionFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strSunDiskConditionFileName);
                }



                return strSunDiskConditionFileName;
            }
        }




        public static string SunDiskConditionFileNamesPattern()
        {
            return "*-SunDiskCondition.xml";
        }




        public static string SunDiskConditionFileName(FileInfo imageFileInfo, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            //FileInfo currImageFInfo = new FileInfo(imageFullPath);
            if (xmlFilesBasePath == "")
            {
                string strSunDiskConditionFileName = Path.GetFileNameWithoutExtension(imageFileInfo.FullName) +
                                                     "-SunDiskCondition.xml";
                if (fullPath)
                {
                    strSunDiskConditionFileName = imageFileInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                  strSunDiskConditionFileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFileInfo.FullName, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFileInfo.FullName);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strSunDiskConditionFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strSunDiskConditionFileName);
                }




                return strSunDiskConditionFileName;
            }
            else
            {
                string strSunDiskConditionFileName = Path.GetFileNameWithoutExtension(imageFileInfo.FullName) +
                                                     "-SunDiskCondition.xml";
                if (fullPath)
                {
                    strSunDiskConditionFileName = xmlFilesBasePath +
                                                  ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                      ? ("")
                                                      : (Path.DirectorySeparatorChar.ToString())) +
                                                  strSunDiskConditionFileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFileInfo.FullName, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFileInfo.FullName);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strSunDiskConditionFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strSunDiskConditionFileName);
                }



                return strSunDiskConditionFileName;
            }

        }



        public static string ImageGrIxMedianP5DataFileName(string imageFullPath, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (xmlFilesBasePath == "")
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strImageGrIxMedianP5DataFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                          "-GrIxMedianP5.xml";
                if (fullPath)
                {
                    strImageGrIxMedianP5DataFileName = currImageFInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                       strImageGrIxMedianP5DataFileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strImageGrIxMedianP5DataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strImageGrIxMedianP5DataFileName);
                }



                return strImageGrIxMedianP5DataFileName;
            }
            else
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strImageGrIxMedianP5DataFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                          "-GrIxMedianP5.xml";
                if (fullPath)
                {
                    strImageGrIxMedianP5DataFileName = xmlFilesBasePath +
                                                       ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                           ? ("")
                                                           : (Path.DirectorySeparatorChar.ToString())) +
                                                       strImageGrIxMedianP5DataFileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strImageGrIxMedianP5DataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strImageGrIxMedianP5DataFileName);
                }



                return strImageGrIxMedianP5DataFileName;
            }
        }




        public static string ImageGrIxMedianP5DataFileNamesPattern()
        {
            return "*-GrIxMedianP5.xml";
        }





        public static string ImageGrIxYRGBstatsDataFileName(string imageFullPath, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (xmlFilesBasePath == "")
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strImageGrIxYRGBstatsDataFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                           "-GrIxYRGBstats.xml";
                if (fullPath)
                {
                    strImageGrIxYRGBstatsDataFileName = currImageFInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                        strImageGrIxYRGBstatsDataFileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strImageGrIxYRGBstatsDataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strImageGrIxYRGBstatsDataFileName);
                }



                return strImageGrIxYRGBstatsDataFileName;
            }
            else
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strImageGrIxYRGBstatsDataFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                           "-GrIxYRGBstats.xml";
                if (fullPath)
                {
                    strImageGrIxYRGBstatsDataFileName = xmlFilesBasePath +
                                                           ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                               ? ("")
                                                               : (Path.DirectorySeparatorChar.ToString())) +
                                                        strImageGrIxYRGBstatsDataFileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strImageGrIxYRGBstatsDataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strImageGrIxYRGBstatsDataFileName);
                }



                return strImageGrIxYRGBstatsDataFileName;
            }

        }




        public static string ImageGrIxYRGBstatsFileNamesPattern()
        {
            return "*-GrIxYRGBstats.xml";
        }







        public static string SkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName(string imageFullPath, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (xmlFilesBasePath == "")
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                           "-SkyImagesDataWithConcurrentStatsCloudCoverAndSDC.xml";
                if (fullPath)
                {
                    strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName = currImageFInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                        strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName = strNewBaseDirectory +
                                                      Path.GetFileName(strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName);
                }



                return strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName;
            }
            else
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                           "-SkyImagesDataWithConcurrentStatsCloudCoverAndSDC.xml";
                if (fullPath)
                {
                    strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName = xmlFilesBasePath +
                                                           ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                               ? ("")
                                                               : (Path.DirectorySeparatorChar.ToString())) +
                                                        strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName = strNewBaseDirectory +
                                                      Path.GetFileName(strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName);
                }



                return strSkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileName;
            }

        }




        public static string SkyImagesDataWithConcurrentStatsCloudCoverAndSDC_FileNamesPattern()
        {
            return "*-SkyImagesDataWithConcurrentStatsCloudCoverAndSDC.xml";
        }








        public static string ImageGrIxMedianP5DataFileName(FileInfo imageFileInfo, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (xmlFilesBasePath == "")
            {
                //FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strImageGrIxMedianP5DataFileName = Path.GetFileNameWithoutExtension(imageFileInfo.FullName) +
                                                          "-GrIxMedianP5.xml";
                if (fullPath)
                {
                    strImageGrIxMedianP5DataFileName = imageFileInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                       strImageGrIxMedianP5DataFileName;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFileInfo.FullName, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFileInfo.FullName);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strImageGrIxMedianP5DataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strImageGrIxMedianP5DataFileName);
                }



                return strImageGrIxMedianP5DataFileName;
            }
            else
            {
                string strImageGrIxMedianP5DataFileName = Path.GetFileNameWithoutExtension(imageFileInfo.FullName) +
                                                          "-GrIxMedianP5.xml";
                if (fullPath)
                {
                    strImageGrIxMedianP5DataFileName = xmlFilesBasePath +
                                                       ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                           ? ("")
                                                           : (Path.DirectorySeparatorChar.ToString())) +
                                                       strImageGrIxMedianP5DataFileName;
                }


                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFileInfo.FullName, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFileInfo.FullName);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strImageGrIxMedianP5DataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strImageGrIxMedianP5DataFileName);
                }


                return strImageGrIxMedianP5DataFileName;
            }
        }






        public static string ConcurrentGPSdataFileName(string imageFullPath, string xmlFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (xmlFilesBasePath == "")
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strConcurrentGPSdataFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                     "-concurrentGPSdata.xml";
                if (fullPath)
                {
                    strConcurrentGPSdataFileName = currImageFInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                  strConcurrentGPSdataFileName;
                }


                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strConcurrentGPSdataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strConcurrentGPSdataFileName);
                }


                return strConcurrentGPSdataFileName;
            }
            else
            {
                FileInfo currImageFInfo = new FileInfo(imageFullPath);
                string strConcurrentGPSdataFileName = Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                                     "-concurrentGPSdata.xml";
                if (fullPath)
                {
                    strConcurrentGPSdataFileName = xmlFilesBasePath +
                                                  ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                      ? ("")
                                                      : (Path.DirectorySeparatorChar.ToString())) +
                                                  strConcurrentGPSdataFileName;
                }


                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFullPath, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFullPath);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = xmlFilesBasePath +
                                                 ((xmlFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    strConcurrentGPSdataFileName = strNewBaseDirectory +
                                                      Path.GetFileName(strConcurrentGPSdataFileName);
                }


                return strConcurrentGPSdataFileName;
            }
        }






        public static string ImageConcurrentDataFilesNamesPattern()
        {
            return "data-????-??-??T??-??-??.???????Z.xml";
        }




        public static DateTime DateTimeOfSkyImageFilename(string imgFileName)
        {
            string currImgFilename = imgFileName;
            currImgFilename = Path.GetFileNameWithoutExtension(currImgFilename);

            string ptrn = @"(devID\d)";
            Regex rgxp = new Regex(ptrn, RegexOptions.IgnoreCase);

            string strCurrImgDT = rgxp.Replace(currImgFilename.Substring(4), "");
            //2015-12-16T06-01-38
            strCurrImgDT = strCurrImgDT.Substring(0, 11) + strCurrImgDT.Substring(11).Replace("-", ":");
            DateTime currImgDT = DateTime.Parse(strCurrImgDT, null,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);
            return currImgDT;
        }




        public static string NetCDFimageBareChannelsDataFilename(string imageFileName, string ncFilesBasePath = "", bool fullPath = true, string PreserveRelativeDirectoriesFromBasePath = "")
        {
            if (ncFilesBasePath == "")
            {
                FileInfo currImageFInfo = new FileInfo(imageFileName);
                string imageBareChannelsDataNCfilename = Path.GetFileNameWithoutExtension(imageFileName) +
                                                     "-ColorMatrices.nc";
                if (fullPath)
                {
                    imageBareChannelsDataNCfilename = currImageFInfo.DirectoryName + Path.DirectorySeparatorChar +
                                                  imageBareChannelsDataNCfilename;
                }



                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(imageFileName, PreserveRelativeDirectoriesFromBasePath);
                    string imageFilename = Path.GetFileName(imageFileName);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = ncFilesBasePath +
                                                 ((ncFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    imageBareChannelsDataNCfilename = strNewBaseDirectory +
                                                      Path.GetFileName(imageBareChannelsDataNCfilename);
                }

                return imageBareChannelsDataNCfilename;
            }
            else
            {
                string imageBareChannelsDataNCfilename = Path.GetFileNameWithoutExtension(imageFileName) +
                                                     "-concurrentGPSdata.nc";
                if (fullPath)
                {
                    imageBareChannelsDataNCfilename = ncFilesBasePath +
                                                  ((ncFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                      ? ("")
                                                      : (Path.DirectorySeparatorChar.ToString())) +
                                                  imageBareChannelsDataNCfilename;
                }

                if (fullPath && (PreserveRelativeDirectoriesFromBasePath != ""))
                {
                    // get imageFileName path relative to PreserveRelativeDirectoriesFromBasePath
                    string relImgFilenamePath = MakeRelativePath(PreserveRelativeDirectoriesFromBasePath, imageFileName);
                    string imageFilename = Path.GetFileName(imageFileName);
                    relImgFilenamePath = relImgFilenamePath.Replace(imageFilename, "");
                    string strNewBaseDirectory = ncFilesBasePath +
                                                 ((ncFilesBasePath.Last() == Path.DirectorySeparatorChar)
                                                     ? ("")
                                                     : (Path.DirectorySeparatorChar.ToString())) + relImgFilenamePath;
                    strNewBaseDirectory += (strNewBaseDirectory.Last() == Path.DirectorySeparatorChar)
                                                ? ("")
                                                : (Path.DirectorySeparatorChar.ToString());
                    imageBareChannelsDataNCfilename = strNewBaseDirectory +
                                                      Path.GetFileName(imageBareChannelsDataNCfilename);
                }


                return imageBareChannelsDataNCfilename;
            }
        }




        public static string MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}
