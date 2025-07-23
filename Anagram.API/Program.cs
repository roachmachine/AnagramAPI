using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AnagramAPI
{
    /// <summary>
    /// Main Program entry method
    /// </summary>
    /// <author>Michael</author>
    /// <datetime>5/25/2017 7:02 PM</datetime>
    /// <remarks>Main Program entry method</remarks>
    public class Program
    {

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}