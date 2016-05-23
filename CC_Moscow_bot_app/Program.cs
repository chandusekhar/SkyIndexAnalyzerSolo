using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace CC_Moscow_bot_app
{
    class Program
    {
        


        static void Main(string[] args)
        {
            BotClass bot = new BotClass();

            Console.CancelKeyPress += new ConsoleCancelEventHandler(bot.CancelHandler);

            bot.Start();
        }









    }
}
