using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyImagesAnalyzerLibraries;

namespace FindFilesFromFilenamesList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> argsList = args.ToList();

            List<string> lDirectoriesToSearchIn = new List<string>();
            List<string> lFilenamesToSearch = new List<string>();

            if (!argsList.Any(str => str.Contains("--directories-listfile=")))
            {
                Console.WriteLine("--directories-listfile is not specified. Current working directory will be used.");
                lDirectoriesToSearchIn.Add(Directory.GetCurrentDirectory());
            }
            else
            {
                string arg = argsList.First(str => str.Contains("--directories-listfile="));
                string strDirectoriesListFile = arg.Replace("--directories-listfile=", "");
                if (!File.Exists(strDirectoriesListFile))
                {
                    Console.WriteLine("--directories-listfile does not exist. Current working directory will be used.");
                    lDirectoriesToSearchIn.Add(Directory.GetCurrentDirectory());
                }
                else
                {
                    string strFileContents = ServiceTools.ReadTextFromFile(strDirectoriesListFile);
                    List<string> lDirectories = strFileContents.Split(Environment.NewLine.ToCharArray()).ToList();
                    lDirectories.RemoveAll(str => str.Length == 0);
                    lDirectoriesToSearchIn.AddRange(lDirectories);
                }
            }

            Console.WriteLine("Files will be searched in directories:");
            foreach (string s in lDirectoriesToSearchIn)
            {
                Console.WriteLine(s);
            }



            if (!argsList.Any(str => str.Contains("--fileslist=")))
            {
                Console.WriteLine("--fileslist is not specified. Can`t proceed.");
                return;
            }
            else
            {
                string arg = argsList.First(str => str.Contains("--fileslist="));
                string strFilesList_file = arg.Replace("--fileslist=", "");
                if (!File.Exists(strFilesList_file))
                {
                    Console.WriteLine("--fileslist does not exist. Can`t proceed.");
                    return;
                }
                else
                {
                    string strFileContents = ServiceTools.ReadTextFromFile(strFilesList_file);
                    List<string> lFilenames = strFileContents.Split(Environment.NewLine.ToCharArray()).ToList();
                    lFilenames.RemoveAll(str => str.Length == 0);
                    lFilenamesToSearch.AddRange(lFilenames);
                }
            }

            Console.WriteLine("Files to search count: " + lFilenamesToSearch.Count);

            List<string> lFilesFound = new List<string>();
            foreach (string filename in lFilenamesToSearch)
            {
                Console.WriteLine("searching for file " + filename);
                foreach (string dirname in lDirectoriesToSearchIn)
                {
                    DirectoryInfo currDirInfo = new DirectoryInfo(dirname);
                    List<FileInfo> filenamesFound = currDirInfo.GetFiles(filename, SearchOption.AllDirectories).ToList();
                    if (filenamesFound.Any())
                    {
                        lFilesFound.AddRange(filenamesFound.ConvertAll(finfo => finfo.FullName));
                        break;
                    }
                }
            }

            Console.WriteLine("Found files:");
            foreach (string fileFound in lFilesFound)
            {
                Console.WriteLine(fileFound);
            }

            if (argsList.Any(str => str.Contains("--copy-to=")))
            {
                string arg = argsList.First(str => str.Contains("--copy-to="));
                string directoryToCopyTo = arg.Replace("--copy-to=", "");
                directoryToCopyTo = directoryToCopyTo +
                                    ((directoryToCopyTo.Last() == Path.DirectorySeparatorChar)
                                        ? ("")
                                        : (Path.DirectorySeparatorChar.ToString()));

                foreach (string fileFound in lFilesFound)
                {
                    File.Copy(fileFound, directoryToCopyTo + Path.GetFileName(fileFound));
                    Console.WriteLine(fileFound + " >> " + directoryToCopyTo + Path.GetFileName(fileFound));
                }
            }
        }
    }
}
