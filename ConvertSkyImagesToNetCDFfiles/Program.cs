using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertSkyImagesToNetCDFfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Converter cnv = new Converter();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(cnv.CancelHandler);
            cnv.Start(args);
        }
    }
}
