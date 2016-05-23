using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageStatsCalculatorServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ClaculatorServer srv = new ClaculatorServer();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(srv.CancelHandler);
            srv.Start();
        }
    }
}
