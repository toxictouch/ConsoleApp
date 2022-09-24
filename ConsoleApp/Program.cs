// Followed this article to update the target framework: 5.0 -> 6.0
// https://docs.microsoft.com/en-us/dotnet/core/tutorials/top-level-templates
namespace ConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            // run initialization displays and get initial user input
            App.Start.Run();

            /// get the main user menu
            /// I think this can be ignored if wanting to try something differnt
            //App.Menuing.CreateMenu(App.Start.MenuName, App.Start.MenuOptions);
            App.Logging.DotNetConfig();


            // menu has finished, close the app
            App.Menuing.Close();
        }
    }
}
