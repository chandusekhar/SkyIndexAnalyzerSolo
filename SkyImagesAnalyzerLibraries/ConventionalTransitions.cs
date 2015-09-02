using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public class ConventionalTransitions
    {

        public static string SunDiskInfoFileName(string imageFullPath)
        {
            FileInfo fInfo1 = new FileInfo(imageFullPath);
            string sunDiskInfoFileName = fInfo1.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(imageFullPath) +
                                         "-SunDiskInfo.xml";
            return sunDiskInfoFileName;
        }



        public static string SunDiskInfoFileNamesPattern()
        {
            return "*-SunDiskInfo.xml";
        }





        public static string SunDiskConditionFileName(string imageFullPath)
        {
            FileInfo currImageFInfo = new FileInfo(imageFullPath);
            string strSunDiskConditionFileName = currImageFInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                         "-SunDiskCondition.xml";
            return strSunDiskConditionFileName;
        }




        public static string SunDiskConditionFileNamesPattern()
        {
            return "*-SunDiskCondition.xml";
        }




        public static string SunDiskConditionFileName(FileInfo imageFileInfo)
        {
            //FileInfo currImageFInfo = new FileInfo(imageFullPath);
            string strSunDiskConditionFileName = imageFileInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(imageFileInfo.FullName) +
                                         "-SunDiskCondition.xml";
            return strSunDiskConditionFileName;
        }



        public static string ImageGrIxMedianP5DataFileName(string imageFullPath)
        {
            FileInfo currImageFInfo = new FileInfo(imageFullPath);
            string strImageGrIxMedianP5DataFileName = currImageFInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                         "-GrIxMedianP5.xml";
            return strImageGrIxMedianP5DataFileName;
        }




        public static string ImageGrIxMedianP5DataFileNamesPattern()
        {
            return "*-GrIxMedianP5.xml";
        }





        public static string ImageGrIxYRGBstatsDataFileName(string imageFullPath)
        {
            FileInfo currImageFInfo = new FileInfo(imageFullPath);
            string strImageGrIxYRGBstatsDataFileName = currImageFInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(currImageFInfo.FullName) +
                                         "-GrIxYRGBstats.xml";
            return strImageGrIxYRGBstatsDataFileName;
        }




        public static string ImageGrIxYRGBstatsFileNamesPattern()
        {
            return "*-GrIxYRGBstats.xml";
        }






        public static string ImageGrIxMedianP5DataFileName(FileInfo imageFileInfo)
        {
            //FileInfo currImageFInfo = new FileInfo(imageFullPath);
            string strImageGrIxMedianP5DataFileName = imageFileInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(imageFileInfo.FullName) +
                                         "-GrIxMedianP5.xml";
            return strImageGrIxMedianP5DataFileName;
        }
    }
}
