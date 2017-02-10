using System;
using System.Diagnostics;
using Serilog;
using Serilog.Core;

namespace WS.Utilities.Injection.Usage
{
    /// <summary>
    /// Simple Demonstration of how to use the Basic Injection Container
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Simple Demonstration of how to use the Basic Injection Container
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public static void Main(string[] args)
        {
            var logger = ConfigureLogger();
            var container = new BasicInjectionContainer();
            try
            {
                // Add an instance to the container that has already been created
                container.RegisterInstance(logger);

                // Add a type to the container
                container.RegisterType<MathsService>();

                // Add a type to the container that shouuld be resolved to an implementing
                // or inherited type. Useful if the consumers require interfaces in thier
                // constructors
                container.RegisterType<IAddService, AddService>();

                // Get an instance from the container
                var mathsService = container.Resolve<MathsService>();

                var sum = mathsService.Add(1, 2);
                logger.Information("The sum of 1 and 2 is {sum}", sum);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "An error occured in the usage demonstration");
            }

            logger.Information("Done");
            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }

        private static Logger ConfigureLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }
    }
}
