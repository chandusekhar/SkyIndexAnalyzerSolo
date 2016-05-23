using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortFilesByDates
{
    class Program
    {
        static void Main(string[] args)
        {
            Sorter srt = new Sorter();
            srt.Start(args);
        }
    }
}
