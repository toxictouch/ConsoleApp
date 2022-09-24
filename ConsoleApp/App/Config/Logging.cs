using System;
using System.IO;
using System.Reflection;

namespace ConsoleApp.App.Config
{
    internal class Logging
    {
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

                /// run a test log
                /// (everything except for the .Info() method can be used to create a private class ILog logger object)
                log4net
                    .LogManager
                    .GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
                    .Info("logging configuration was successful!");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("The log4net.config file is missing; file not found exception caught!");
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // alert the user if an exception should occur
                Console.WriteLine("An error occured during the configuration of log4net - Exception details follow:");
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
