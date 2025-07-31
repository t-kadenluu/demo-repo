using System;
using System.Threading.Tasks;
using System.Threading;

namespace Microsoft.TestService.Startup
{
    /// <summary>
    /// Application startup configuration
    /// </summary>
    public class Startup
    {
        public void Configure()
        {
            Console.WriteLine("Application configured successfully");
        }
    }
}

namespace Microsoft.TestService.Program
{
    /// <summary>
    /// Main program entry point
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting TestService...");

            // Initialize startup
            var startup = new Microsoft.TestService.Startup.Startup();
            startup.Configure();

            Console.WriteLine("TestService started successfully");
            Console.WriteLine("Press any key to exit...");

            using var cts = new CancellationTokenSource();
            try
            {
                await Task.Run(() => Console.ReadLine(), cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was canceled.");
            }
        }
    }
}