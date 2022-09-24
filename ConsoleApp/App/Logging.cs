using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// log4net requirements?
using System.IO;
using System.Reflection;


namespace ConsoleApp.App
{
    internal class Logging
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Logging));
        // secondary attempt
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Basic log4net configuration. Does NOT work in .Net Core
        /// </summary>
        private static void Configure()
        {
            // basic configuration
            log4net.Config.BasicConfigurator.Configure();
        }


        /// <summary>
        /// Configuration for .Net Core to use log4net
        /// </summary>
        public static void DotNetConfig()
        {
            try
            {
                // get the default assembly info - prepare to change
                log4net.Repository.ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());

                // find the log4net configuration file (excluding during publish)
                FileInfo fileInfo = new("@log4net.config");

                // provide the 
                log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

                // run a log
                logger.Info("logging configuration was successful!");
            }
            catch (Exception ex)
            {
                // alert the user if an exception should occur
                Console.WriteLine("An error occured during the configuration of log4net - Exception details follow:");
                Console.WriteLine();
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
