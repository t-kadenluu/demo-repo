using System;
using System.Threading;
using System.Threading.Tasks;

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
                await WaitForKeyPressAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation if needed
            }
        }

        private static async Task WaitForKeyPressAsync(CancellationToken cancellationToken)
        {
            // Use TaskCompletionSource with RunContinuationsAsynchronously for safe continuations
            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            using var registration = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));

            _ = Task.Run(() =>
            {
                Console.ReadLine();
                tcs.TrySetResult(null);
            }, cancellationToken);

            await tcs.Task.ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}