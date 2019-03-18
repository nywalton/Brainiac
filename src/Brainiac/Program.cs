using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Brainiac
{
    /// <summary>
    /// Entry class of the application.
    /// </summary>
    public class Program
    {
        private static readonly ILogger _log = LogWrapper.Instance.GetLogger<Program>();

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// BuildWebHost helper method.
        /// .UseKestrel(options => { options.Listen(IPAddress.Loopback, 9000); })
        /// </summary>
        /// <param name="args">Arguments pass through the command line.</param>
        /// <returns>An instance of an IWebHost.</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<StartUp>().Build();
    }
}
