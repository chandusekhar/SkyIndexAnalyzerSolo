using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MLsetPerMissionCreationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> argsList = new List<string>(args);

            if (argsList.Where(str => str.Contains("--help")).Count() > 0)
            {
                ReportHelp();
                return;
            }



            Calculator calcObj = new Calculator();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(calcObj.CancelHandler);
            calcObj.Start(args);


            if (!argsList.Any())
            {
                ReportHelp();
                return;
            }
        }




        static void ReportHelp()
        {
            string strToReport = "usage: " + Path.GetFileName(Assembly.GetEntryAssembly().Location) +
                                 " <options> " + Environment.NewLine;
            strToReport += "options: " + Environment.NewLine +
                "--recursive \t\t\t means recursive images listing" + Environment.NewLine +
                "-y means processing starting without confirmation" + Environment.NewLine +
                "--camera-id=<(int) camera ID>\t\t to specify filtering by camera ID" + Environment.NewLine +
                "--sdc=<(int) sdc index> in the range of: none, 0, 1, 2 - to specify filtering by Sun disk condition" + Environment.NewLine +
                "--filter-by-observed-cloud-cover-records - to specify filtering by observed cloud cover data." + Environment.NewLine +
                           "\t\t\tPlease note that the app will have to read the file specified by ObservedCloudCoverDataCSVfile property value" + Environment.NewLine +
                "--help\t\t\t\t\tshows this help";
            Console.Write(strToReport);
        }
    }
}
