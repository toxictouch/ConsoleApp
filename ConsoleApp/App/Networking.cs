using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            "run a [u]ri test",
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

                case 'u': // try the uri test
                    UriTest();
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
            string pingTarget = GetTarget(false);
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

            string pingTarget = GetTarget(false);
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
        /// prompts the user to provide a network target,
        /// parameter used to specify if the target has to be a URL (ie: a webserver)
        /// </summary>
        /// <returns>the string/user input for the network target</returns>
        internal static string GetTarget(bool urlOnly)
        {
            // clear window and issue user prompts
            Console.Clear();
            Console.WriteLine("please write the target destination");

            if (urlOnly)
                Console.Write("note - only a complete url will work (ie: https://www.test.com): ");
            else
                Console.Write("note - the following should all work (url, ip address, or hostname): ");

            // necessary variable and user input
            string target = Console.ReadLine();
            bool validTarget = false;

            // check user input and ask them to retry IF necessary
            while (!validTarget)
            {
                if (string.IsNullOrWhiteSpace(target)) // empty target, prompt user to try again
                {
                    Console.WriteLine("target destination was invalid, please try again");
                    Console.WriteLine();

                    target = Console.ReadLine();
                }
                else if (urlOnly) // uh-oh, need to make sure its a full url too
                {
                    // validate the string STARTS correctly
                    if (target.StartsWith("https://") || target.StartsWith("http://"))
                        validTarget = true;
                    else
                    {
                        // it didn't, prompt user to try again
                        Console.WriteLine("target destination was invalid, please try again");
                        Console.WriteLine();

                        target = Console.ReadLine();
                    }
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
        internal static bool DoesPingSucceed(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
                Console.WriteLine("a ping exception occured O_o");
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

        #region Proof of concept area

        /// <summary>
        /// Runs a test on a URL. Get's URI and web resource information
        /// </summary>
        private static void UriTest()
        {
            // get a url target from the user
            string target = GetTarget(true);

            // alert user to impending test
            Console.WriteLine();
            Console.WriteLine("preparing to run a uri test on: " + target);
            Console.WriteLine();
            
            // create a new uri from the provided target and write it's attributes to the screen
            Uri path = new(target);
            Console.WriteLine("Port: " + path.Port);
            Console.WriteLine("Host: " + path.Host);
            Console.WriteLine("LocalPath: " + path.LocalPath);
            Console.WriteLine("Authority: " + path.Authority);
            Console.WriteLine("Scheme: " + path.Scheme);
            Console.WriteLine("PathAndQuery: " + path.PathAndQuery);
            Console.WriteLine("Query: " + path.Query);
            Console.WriteLine("AbsolutePath: " + path.AbsolutePath);
            Console.WriteLine("AbsoluteUri: " + path.AbsoluteUri);
            Console.WriteLine("OriginalString: " + path.OriginalString);
            Console.WriteLine("Fragment: " + path.Fragment);
            Console.WriteLine("UserInfo: " + path.UserInfo);
            Console.WriteLine("IsAbsoluteUri: " + path.IsAbsoluteUri);
            Console.WriteLine("IsDefaultPort: " + path.IsDefaultPort);
            Console.WriteLine("IsFile: " + path.IsFile);
            Console.WriteLine("IsLoopback: " + path.IsLoopback);
            Console.WriteLine("IsUnc: " + path.IsUnc);

            // alert user to resource test, attempt to get uri resource info
            Console.WriteLine();
            Console.WriteLine("Attempting to get web resource...");
            GetWebResource(path);

            // add a little spacing and prompt user to close feature
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
        }

        private static void GetWebResource(Uri uri)
        {
            WebRequest request = WebRequest.Create(uri); // obsolete?

            using WebResponse response = request.GetResponse();

            var headers = response.Headers;

            Console.WriteLine(headers);

            // HttpClient request = WebRequest.Create(uri);
        }

        #endregion
    }
}
