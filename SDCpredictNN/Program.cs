using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SDCpredictNN
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> argsList = new List<string>(args);

            if (argsList.Find(str => str.Substring(0, 6) == "--help") != null)
            {
                ReportHelp();
                return;
            }

            if (!argsList.Any())
            {
                ReportHelp();
                return;
            }

            NNPredictor predictorObj = new NNPredictor();
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(calcObj.CancelHandler);
            predictorObj.Start(args);
        }





        static void ReportHelp()
        {
            string strToReport = "usage: " + Path.GetFileName(Assembly.GetEntryAssembly().Location) + " [options] <filename>";
            strToReport += "options: " + Environment.NewLine +
                /*"--calc-stats\t\t\t\tmain calculation start" + Environment.NewLine +
                    "\t secondary options in that case:" + Environment.NewLine +
                    "\t --mask=<filenames-pattern>\treplaces default filenames mask which is defined" + Environment.NewLine +
                            "\t\t\t\t\tin CollectMaskDataApp-Settings.xml by FilesToProcessMask variable" + Environment.NewLine +
                    "\t --recursive\t\t\tattempt to enumerate files (using mask mentioned above) recursively" + Environment.NewLine +
                           "\t\t\t\t\tstarting from base directory" + Environment.NewLine +
                "--convert-existing-stats=<filename.csv || filename-pattern.csv>\tconverts previously calculated *.csv file(s)" + Environment.NewLine +
                           "\t\t\t\t\t(see --calc-stats option) to one *.xml file containing one mean ImageRD data" + Environment.NewLine +
                           "\t\t\t\t\tusing this option the output data file will get the same name with \"xml\" extension." + Environment.NewLine +
                    "\t secondary options in that case:" + Environment.NewLine +
                    "\t --recursive\t\t\tattempt to enumerate files (using filename pattern) recursively" + Environment.NewLine +
                           "\t\t\t\t\tstarting from base directory" + Environment.NewLine +
                    "\t --filter-default-CenterX-double-value=<value>\t\t\texclude records with default value of CenterX" + Environment.NewLine +
                    "\t --filter-default-CenterY-double-value=<value>\t\t\texclude records with default value of CenterY" + Environment.NewLine +
                    "\t --filter-default-Radius-double-value=<value>\t\t\texclude records with default value of Radius" + Environment.NewLine + */
                "--help\t\t\t\t\tshows this help";
            Console.Write(strToReport);
        }
    }
}
