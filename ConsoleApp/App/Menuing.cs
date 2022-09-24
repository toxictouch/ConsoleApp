using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApp.App
{
    internal class Menuing
    {
        /// <summary>
        /// Future proofs for the inevitable logging steps required
        /// </summary>
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates menu, displays the menu names and the options for the menu. Continues to show options until the
        /// menu is triggered to close
        /// </summary>
        /// <param name="menuName">Menu name to display</param>
        /// <param name="menuOptions">Menu options</param>
        public static void CreateMenu(string menuName, List<string> menuOptions)
        {
            Console.Clear();
            Console.WriteLine(menuName);
            Console.WriteLine();

            bool closeMenu;
            do
            {
                DisplayMenuOptions(menuOptions);

                char response = Console.ReadKey().KeyChar;
                Console.WriteLine();

                closeMenu = MenuSelectionOrClose(menuName, response);

            } while (!closeMenu);
        }

        /// <summary>
        /// Function takes in a list of strings for the menu options and dispays them to the user
        /// </summary>
        /// <param name="menuOptions">The list of options for the current menu</param>
        private static void DisplayMenuOptions(List<string> menuOptions)
        {
            Console.WriteLine("press the highlighted key to make your selection");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            foreach (string option in menuOptions)
            {
                Console.WriteLine(option);
            }

            Console.WriteLine(" ");
        }

        /// <summary>
        /// Method takes the menu name and compares if it matches any known menus; if it does, we call that objects menu selector
        /// NOTE: This method is the only one that will require updates as the app expands
        /// TODO: add new app functions here
        /// </summary>
        /// <param name="menuName">The menu name we're comparing</param>
        /// <param name="selection">The char value the user input</param>
        /// <returns>False, unless the menu name is a match. Requires the called menu selector to return True</returns>
        private static bool MenuSelectionOrClose(string menuName, char selection)
        {
            bool closeMenu = false;

            // See if menu name matches networking menu name
            if (menuName == Networking.MenuName)
                closeMenu = Networking.MenuSelector(selection);
            else if (menuName == ConsoleDisplays.MenuName)
                closeMenu = ConsoleDisplays.MenuSelector(selection);
            else if (menuName == Start.MenuName)
                closeMenu = Start.MenuSelector(selection);
            else if (menuName == Attacking.MenuName)
                closeMenu |= Attacking.MenuSelector(selection);

            // TODO: add new menus here

            return closeMenu;
        }

        /// <summary>
        /// prompts user to close press any "key" to close the app. should be last method called by the app
        /// </summary>
        public static void Close()
        {
            Console.WriteLine("thanks for trying my app");
            Console.WriteLine("press any key to close this window");
            Console.ReadKey();
        }
    }
}
