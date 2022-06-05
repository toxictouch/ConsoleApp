using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.App
{
    internal class Networking
    {
        #region networking menu/options

        public static readonly string MenuName = "networking menu";

        public static readonly List<string> MenuOptions = new()
        {
            "[t]est ping a url/host",
            "get ping [d]iagnostics to a url/host",
            "[c]lose this menu"
        };

        public static bool MenuSelector(Char selection)
        {
            bool closeMenu = false;
            switch (selection)
            {
                case 't': // test ping a ip or url
                    TestPing();
                    break;

                case 'd': // get ping diagnostics
                    PingDiagnostics();
                    break;

                case 'c': // close menu
                    closeMenu = true;
                    break;

                default: // invalid choice
                    Console.WriteLine("invalid selection. please try again");
                    break;
            }

            Console.WriteLine();

            return closeMenu;
        }

        #endregion

        #region Callable actions for menu selector

        private static void TestPing()
        {
            // let user know their selection and wait 2 secs
            Console.WriteLine("you have chosen to test a ping");
            Thread.Sleep(2000);

            // get the ping target and test the ping
            string pingTarget = GetPingTarget();
            bool pingSuccess = DoesPingSucceed(pingTarget);

            // spacing
            Console.WriteLine();
            Console.WriteLine();

            // let user know the success/failure of the ping
            if (pingSuccess)
                Console.WriteLine("your ping was successful!");
            else
                Console.WriteLine("uh-oh, seems your ping was unsuccessful :(");

            // prompts user to go back a menu
            Console.WriteLine("press any key to return to the networking menu");
            Console.ReadKey();
            Console.Clear();
        }

        private static void PingDiagnostics()
        {
            Console.WriteLine("you have chosen to view ping diagnostics");
            Thread.Sleep(2000);

            string pingTarget = GetPingTarget();
            GetPingDiagnostics(pingTarget);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("press any key to return to the networking menu");
            Console.ReadKey();
            Console.Clear();
        }

        #endregion

        #region Main networking functions

        /// <summary>
        /// prompts the user to provide a pingable target
        /// </summary>
        /// <returns>the string/user input for the ping target</returns>
        private static string GetPingTarget()
        {
            // clear window and issue user prompts
            Console.Clear();
            Console.WriteLine("please write the ping destination");
            Console.Write("(note - the following should all work: url, ip address, or hostname): ");

            // necessary variable and user input
            string target = Console.ReadLine();
            bool validTarget = false;

            // check user input and ask them to retry IF necessary
            while (!validTarget)
            {
                if (string.IsNullOrWhiteSpace(target)) // invalid target, prompt user to try again
                {
                    Console.WriteLine("ping destination was invalid, please try again");
                    Console.WriteLine();

                    target = Console.ReadLine();
                }
                else // seems like a decent target, return it
                    validTarget = true;
            }

            // return ping target
            return target;
        }

        /// <summary>
        /// Attempts to ping a name or address, returning the success/failure of the ping
        /// </summary>
        /// <param name="nameOrAddress">The ping target</param>
        /// <returns>True if the ping succeeded; false if it failed</returns>
        private static bool DoesPingSucceed(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException ex)
            {
                // Discard PingExceptions and return false;
                Console.WriteLine("a ping exception occured | message follows");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        /// <summary>
        /// Runs a more specific ping to the provided url or ip
        /// </summary>
        /// <param name="nameOrAddress">The target to pings</param>
        private static void GetPingDiagnostics(string nameOrAddress)
        {
            // makes sure a ping will work first
            if (DoesPingSucceed(nameOrAddress))
            {
                Ping pingSender = new();
                PingOptions pingOptions = new();

                // use a TTL value of 128 but w/ different fragmentation
                pingOptions.DontFragment = true;
                pingOptions.Ttl = 128;

                // create a buffer of 32 bytes and timeout of 120 ms
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;

                // Add the options to the ping sender and save to a ping reply
                PingReply reply = pingSender.Send(nameOrAddress, timeout, buffer, pingOptions);

                // Check the ping status, issue the following messages if it succeeds
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("ping successful!!");
                    Console.WriteLine("");
                    Console.WriteLine("Address: {0}", reply.Address.ToString());
                    Console.WriteLine("Round Trip time: {0} ms", reply.RoundtripTime);

                    // make sure the options isn't null
                    if (reply.Options != null)
                    {
                        Console.WriteLine("Time to live: {0} packets left", !string.IsNullOrWhiteSpace(reply.Options.Ttl.ToString()) ? reply.Options.Ttl.ToString() : string.Empty);
                        Console.WriteLine("Don't fragment: {0}", !string.IsNullOrWhiteSpace(reply.Options.DontFragment.ToString()) ? reply.Options.DontFragment.ToString() : string.Empty);
                    }

                    Console.WriteLine("Buffer size: {0} bytes", reply.Buffer.Length);
                }
                else // issue the following messages if the ping fails
                {
                    Console.WriteLine("ping failed :(");
                    Console.WriteLine("");
                    Console.WriteLine("ping result: {0}", reply.Status.ToString());
                }
            }
            else
            {
                Console.WriteLine("seems the target is down. check your input and try again");
            }
            
        }

        #endregion
    }
}
