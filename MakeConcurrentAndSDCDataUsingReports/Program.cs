using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeConcurrentAndSDCDataUsingReports
{
    class Program
    {
        static void Main(string[] args)
        {
            DataExtractor extr = new DataExtractor();
            extr.Start(args);

        }
    }
}
