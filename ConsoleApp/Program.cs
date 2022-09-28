// Followed this article to update the target framework: 5.0 -> 6.0
// https://docs.microsoft.com/en-us/dotnet/core/tutorials/top-level-templates
using ConsoleApp.App;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            // run initialization displays and get initial user input
            Start.Run();

            /// get the main user menu
            /// I think this can be ignored if wanting to try something differnt
            Menuing.CreateMenu(Start.MenuName, Start.MenuOptions);

            // TODO: the following methods should be abstracted and added to the menuing system somehow
            //SqlCommands.GetAllSampleData();
            //SqlCommands.InsertPeopleList(INSERT_LIST_HERE);
            //FileConsumer.WritePeopleToScreen();
            //FileConsumer.InsertPeopleFromJson();

            // menu has finished, close the app
            Menuing.Close();
        }
    }
}
