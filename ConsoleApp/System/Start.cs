using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.System
{
    internal class Start
    {
        public static void Initialize()
        {
            Console.WriteLine("loading application.....");
            Thread.Sleep(1500);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(".");
                Thread.Sleep(100);
            }

            Console.WriteLine("application loaded!");

            Thread.Sleep(1500);

            Console.Clear();
        }
    }
}
