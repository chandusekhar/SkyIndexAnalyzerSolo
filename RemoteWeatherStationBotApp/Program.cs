using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWeatherStationBotApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RemoteServerBotClass bot = new RemoteServerBotClass();

            // Console.CancelKeyPress += new ConsoleCancelEventHandler(bot.CancelHandler);

            bot.Start();
        }
    }
}
