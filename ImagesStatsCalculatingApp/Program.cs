using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using SkyImagesAnalyzerLibraries;

namespace ImagesStatsCalculatingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            StatsCalculator calc = new StatsCalculator();
            calc.Start(args);
        }
    }
}
