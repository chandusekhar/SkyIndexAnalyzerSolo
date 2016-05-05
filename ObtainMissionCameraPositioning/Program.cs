using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CameraPositioningPerMission
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
                "--camera-id=<(int) camera ID>" + Environment.NewLine +
                "--sdc=<(int) sdc index> in the range of: none, 0, 1, 2" + Environment.NewLine +
                "--help\t\t\t\t\tshows this help";
            Console.Write(strToReport);
        }
    }
}
