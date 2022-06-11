using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp.App
{
    internal class ConsoleDisplays
    {
        #region console displays menu options

        public static readonly string MenuName = "console displays menu";

        public static readonly List<string> MenuOptions = new()
        {
            "display a [t]riangle",
            "display skm[3] again",
            "[c]lose this menu"
        };

        public static bool MenuSelector(char selection)
        {
            bool closeMenu = false;
            switch(selection)
            {
                case 't':
                    DisplaySidewaysTriangle(3);
                    break;
                case '3':
                    DisplaySkm3(5);
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

        #region Displays icons and clears the screen

        /// <summary>
        /// Displays a cool icon in the console
        /// </summary>
        private static void DisplaySidewaysTriangle(int waitNumSecs)
        {
            Console.Clear();
            Thread.Sleep(500);

            // how big the triangle will be (num of columns per half)
            int NumberOfIcons = 10;

            // write top half of the triangle
            for (int i = 1; i <= NumberOfIcons; i++)
            {
                //Console.SetCursorPosition((Console.WindowWidth - 15) / 2, Console.CursorTop);

                int IconCounter = i;
                int SpaceCounter = i;

                while (SpaceCounter <= NumberOfIcons)
                {
                    Console.Write(" ");
                    SpaceCounter++;
                }

                while (IconCounter > 0)
                {
                    Console.Write("0");
                    IconCounter--;
                }

                Thread.Sleep(100);
                Console.WriteLine();
            }

            // write the bottom half of the triangle
            for (int i = NumberOfIcons; i >= 1; i--)
            {
                //Console.SetCursorPosition((Console.WindowWidth - 15) / 2, Console.CursorTop);

                int IconCounter = i;
                int SpaceCounter = i;

                while (SpaceCounter <= NumberOfIcons)
                {
                    Console.Write(" ");
                    SpaceCounter++;
                }

                while (IconCounter > 0)
                {
                    Console.Write("0");
                    IconCounter--;
                }

                Thread.Sleep(100);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            ClearScreen(waitNumSecs);
        }

        /// <summary>
        /// Displays skm3 as zeros in the console window
        /// 
        /// NOTE: internal access for app initialization displays. normally should be private
        /// </summary>
        internal static void DisplaySkm3(int waitNumSecs)
        {
            // clear the window then wait half a second before beginning
            Console.Clear();
            Thread.Sleep(500);
            
            // create the icon using variables  (used to get string lengths)
            string iconPart1 = "00000     0    0     000  000     00000";
            string iconPart2 = "0   0     0   0      0 0000 0         0";
            string iconPart3 = "0         0  0       0  00  0         0";
            string iconPart4 = "0         000        0      0        00";
            string iconPart5 = "0000      000        0      0      0000";
            string iconPart6 = "    0     0 0        0      0        00";
            string iconPart7 = "    0     0  0       0      0         0";
            string iconPart8 = "0   0     0   0      0      0         0";
            string iconPart9 = "00000     0    0     0      0     00000";

            // using the icon variables, we write them centered to the window. After writing one part, the code will wait 100ms before writing again
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart1.Length / 2)) + "}", iconPart1));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart2.Length / 2)) + "}", iconPart2));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart3.Length / 2)) + "}", iconPart3));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart4.Length / 2)) + "}", iconPart4));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart5.Length / 2)) + "}", iconPart5));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart6.Length / 2)) + "}", iconPart6));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart7.Length / 2)) + "}", iconPart7));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart8.Length / 2)) + "}", iconPart8));
            Thread.Sleep(100);
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (iconPart9.Length / 2)) + "}", iconPart9));
            Thread.Sleep(100);

            // adds some space between the icon and the next items loaded in the console
            Console.WriteLine();
            Console.WriteLine();

            ClearScreen(waitNumSecs);
        }

        /// <summary>
        /// function lets user know the screen will be cleared, then counts down before clearing it
        /// </summary>
        /// <param name="waitNumSecs">The number of seconds to wait before the screen clear</param>
        internal static void ClearScreen(int waitNumSecs)
        {
            // clear screen message and count down
            Console.WriteLine("preparing to clear screen");
            Console.WriteLine();

            for (int i = waitNumSecs; i >= 0; i--)
            {
                Console.WriteLine(i.ToString());

                if (i == 0)
                    Thread.Sleep(250); // only show 0 for a 1/4 of a second, before clearing
                else
                    Thread.Sleep(1000); // otherwise wait a full second before counting down next
            }

            Console.Clear();
        }

        /// <summary>
        /// Issue a warning to the user regarding their menu choice
        /// </summary>
        /// <param name="menuName">Takes menu name to warn user</param>
        public static void Warning(string menuName)
        {
            string menuFunction = menuName.Replace(" menu", string.Empty);

            string msgWarn = "----------------------WARNING----------------------";
            string msgSelected = "!!! you selected the " + menuName + " !!!";
            string msgIllegal = "!!! the features in this menu or the way they can be used, may be ILLEGAL !!!";
            string msgDiscretion = "!!! use EXTREME discretion when " + menuFunction + " !!!";
            string msgDisclaimer = "--- you have been warned ---";


            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (msgWarn.Length / 2)) + "}", msgWarn));
            Console.WriteLine();
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (msgSelected.Length / 2)) + "}", msgSelected));
            Console.WriteLine();
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (msgIllegal.Length / 2)) + "}", msgIllegal));
            Console.WriteLine();
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (msgDiscretion.Length / 2)) + "}", msgDiscretion));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (msgDisclaimer.Length / 2)) + "}", msgDisclaimer));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("press any key to proceed");
            Console.ReadKey();
        }

        #endregion
    }
}
