using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace ConsoleApp.App
{
    internal class Start
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region App Start functions. Called once

        /// <summary>
        /// Method called from Program {Main}. Runs this classes functions
        /// </summary>
        public static void Run()
        {
            Config.Logging.DotNetConfig();
            Initialize();
            DisplayAppInfo();
            ConsoleDisplays.DisplaySkm3(3);


            logger.Info("App.Start.Run => successfully configured!");
        }

        /// <summary>
        /// Runs console initialization
        /// </summary>
        private static void Initialize()
        {
            Console.WriteLine("loading application.....");
            Thread.Sleep(1500);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("...");
                Thread.Sleep(100);
            }

            Console.WriteLine("application loaded!!");

            Thread.Sleep(2000);

            Console.Clear();
        }

        /// <summary>
        /// Displays application title and author information
        /// </summary>
        private static void DisplayAppInfo()
        {
            string Welcome = "welcome to skm3 - console app demo";
            string Author = "author: stanley munson";
            string Date = DateTime.Now.ToLongDateString();
            string Time = DateTime.Now.ToShortTimeString();
            string TimeDateMessage = "current date: " + Date + " | current time: " + Time;
            string ContinuePrompt = "press any key to continue";
            
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (Welcome.Length / 2)) + "}", Welcome));
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (Author.Length / 2)) + "}", Author));
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (TimeDateMessage.Length / 2)) + "}", TimeDateMessage));

            Console.WriteLine();
            Console.WriteLine();

            Console.Write(string.Format("{0," + ((Console.WindowWidth / 2) + (ContinuePrompt.Length / 2)) + "}", ContinuePrompt));
            Console.ReadKey();
            Thread.Sleep(500);
        }

        #endregion

        #region Main application menu. Could be referenced multiple times

        public static readonly string MenuName = "main menu";

        public static readonly List<string> MenuOptions = new()
        {
            "go to console [d]isplays menu",
            "go to [n]etworking menu",
            "go to [a]ttacking menu",
            "[c]lose this application"
        };

        public static bool MenuSelector(char selection)
        {
            bool closeMenu = false;
            switch (selection)
            {
                case 'd':
                    Menuing.CreateMenu(ConsoleDisplays.MenuName, ConsoleDisplays.MenuOptions);
                    break;
                case 'n':
                    Menuing.CreateMenu(Networking.MenuName, Networking.MenuOptions);
                    break;
                case 'a':
                    ConsoleDisplays.Warning(Attacking.MenuName); // issue warning before opening menu
                    Menuing.CreateMenu(Attacking.MenuName, Attacking.MenuOptions);
                    break;
                case 'c':
                    closeMenu = true;
                    break;
                default:
                    Console.WriteLine("invalid selection. please try again");
                    break;
            }

            Console.WriteLine();

            return closeMenu;
        }

        #endregion
    }
}
