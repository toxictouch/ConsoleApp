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
                case 'p': // run a ping attack
                    RunPingDurationAttack();
                    break;

                case 'w': // run a web server attack
                    // TODO: write functions
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

        

        /// <summary>
        /// Main ping attack method, called from this classes menu selector
        /// </summary>
        private static void RunPingDurationAttack()
        {
            // alert user of their choice
            Console.WriteLine("you have chosen a durated ping attack");
            Thread.Sleep(2000);

            // get the attack target
            string pingTarget = Networking.GetTarget(false);

            // get how long attack should run
            double durationInMinutes = GetDurationInMinutes();

            bool BeginAttack = AttackAlerts.AlertUserAndConfirmAttack("durated ping", pingTarget, durationInMinutes);

            if (BeginAttack)
                RunPingAttack(pingTarget, durationInMinutes);
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

        private static void StreamedPing(string target)
        {
            Ping pingSender = new();

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            pingSender.Send(target, 120, Encoding.ASCII.GetBytes(data));
            pingSender.Dispose();
        }

        private static bool RunPing_GetStatus(string nameOrAddress)
        {
            bool isPingSuccess = false;

            Ping pingSender = new();
            //PingOptions pingOptions = new();

            //// use a TTL value of 128 but w/ different fragmentation
            //pingOptions.DontFragment = true;
            //pingOptions.Ttl = 128;

            //32 bytes: aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            // create a buffer of 32 bytes and timeout of 120 ms
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //byte[] buffer = Encoding.ASCII.GetBytes(data);
            //int timeout = 120;

            // Add the options to the ping sender and save to a ping reply
            PingReply reply = pingSender.Send(nameOrAddress, 120, Encoding.ASCII.GetBytes(data));
            //pingSender.Dispose();

            if (reply.Status == IPStatus.Success)
                isPingSuccess = true;

            return isPingSuccess;
        }

        private static void RunPingAttack(string target, double duration)
        {
            int iterations = 0;
            int failures = 0;
            DateTime attackStart = DateTime.Now;
            DateTime attackEnd = DateTime.Now.AddMinutes(duration);

            while (attackEnd >= attackStart)
            {
                iterations++;
                attackStart = DateTime.Now;

                //StreamedPing(target);

                bool pingSuccess = RunPing_GetStatus(target);
                if (!pingSuccess)
                {
                    Console.WriteLine("icmp failed");
                    failures++;
                }
            }

            Console.WriteLine("attack complete - number of iterations: " 
                + string.Format("{0:n0}", iterations) 
                + " | number of failures: " 
                + string.Format("{0:n0}", failures));
        }

        
    }

    /// <summary>
    /// Class containing alerts for attacks like: 
    /// alerting user of attack type, confirmation of attack, error, and terms accepted/declined
    /// </summary>
    internal class AttackAlerts
    {        
        /// <summary>
        /// alerts the user of the attack details, then confirms that the attack may proceed
        /// </summary>
        /// <param name="attackType">The attack type for display</param>
        /// <param name="attackTarget">The target of the attack</param>
        /// <param name="attackDuration">Possibly nullable; could be the attacks duration</param>
        /// <returns>True if the user has a valid target and confirms the attack; otherwise false</returns>
        internal static bool AlertUserAndConfirmAttack(string attackType, string attackTarget, double? attackDuration)
        {
            // create variable for attack to proceed and alert user of attack intentions
            bool attackMayProceed = false;
            TellUserAttackTargetAndDuration(attackType, attackTarget, attackDuration);

            // see if we can even reach this target, otherwise cancel the attack
            Console.WriteLine("checking if target is pingable... ");
            bool isTargetReachable = Networking.DoesPingSucceed(attackTarget);
            if (isTargetReachable)
            {
                // target is reachable! let user know | create variable to check if the user wishes to continue
                Console.WriteLine("success: target is reachable!" +
                    " | attack may begin, this is the last chance to cancel until the attack completes (or you close the app)");
                Console.WriteLine();
                bool termsAndCondition = false;

                // loop; see if the user confirms or denies the attack. no other input accepted
                do
                {
                    // alert user how to confirm the attack, at least once
                    Console.WriteLine("press [y] to continue or [n] to cancel");
                    char choice = Console.ReadKey().KeyChar;
                    Console.WriteLine();

                    // read user input
                    switch (choice)
                    {
                        // user has confirmed the attack.
                        // terms reviewed (break loop) and attack may proceed (let user know)
                        case 'y':
                            termsAndCondition = true;
                            attackMayProceed = true;
                            TermsAccepted();
                            break;

                        // user has denied the attack.
                        // terms reviewed (break loop)
                        // and let user know they will be returned to menu
                        case 'n':
                            termsAndCondition = true;
                            TermsDeclined();
                            break;

                        // invalid input
                        default:
                            Console.WriteLine("invalid selection. please try again");
                            break;
                    }

                } while (!termsAndCondition);
            }
            else
                ErrorAndCancellingAttack("target unreachable");


            // let calling function know if the attack may proceed of the user should go to menu
            return attackMayProceed;
        }

        /// <summary>
        /// tells the user of the type of attack, the target, and (if not null) the attacks duration
        /// </summary>
        /// <param name="attackType">The attack type ie: PING</param>
        /// <param name="attackTarget">The attack target ie: 192.168.1.1</param>
        /// <param name="attackDuration">Nullable; could be attack duration</param>
        private static void TellUserAttackTargetAndDuration(string attackType, string attackTarget, double? attackDuration)
        {
            // alert user of the attack selection | may include minutes if duration isn't NULL
            Console.Clear();
            Console.WriteLine("["
                // write attack type and target message
                + attackType.ToUpper()
                + " ATTACK] you have selected the following target: "
                + attackTarget
                // if attack duration isn't NULL
                + (attackDuration != null
                    // write this line
                    ? string.Format(" | attack will run for {0} minutes", attackDuration)
                    // else write an empty string
                    : string.Empty));
        }

        /// <summary>
        /// writes an error message and tells user attack has been cancelled. then clears screen on user input
        /// </summary>
        /// <param name="errMsg">Nullable; could be a detailed error message</param>
        private static void ErrorAndCancellingAttack(string errMsg)
        {
            // couldn't reach the target, let user know attack has been cancelled
            Console.WriteLine("error: ["
                // if the message isn't null
                + (!string.IsNullOrWhiteSpace(errMsg)
                    // write the error message in upper case
                    ? errMsg.ToUpper()
                    // otherwise say this
                    : "UNDEFINED")
                + "] - cancelling attack");
            Console.WriteLine();
            // wait for user input to clear the screen
            Console.WriteLine("press any key to return to menu");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// internal function letting the user know the attack will begin
        /// </summary>
        private static void TermsAccepted()
        {
            Console.WriteLine("most excellent - the attack may begin");
        }

        /// <summary>
        /// internal function letting user know they have cancelled the attack
        /// and they will be returned to the menu
        /// </summary>
        private static void TermsDeclined()
        {
            Console.WriteLine("no shame in canceling, returning you to menu");
        }
    }
}
