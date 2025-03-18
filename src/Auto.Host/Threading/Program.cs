using System;
using System.Threading;

namespace Auto.Host.Threading;

internal class Program
{
    private static readonly CancellationTokenSource cancellationTokenSource = new();
    internal static void Main()
    {
        var server = AppConfig.InitializeServer(AppConfig.InitializeDatabase());
        server.BeginListening(cancellationTokenSource.Token);

        Console.WriteLine("Press any key to exit...");
        Console.ReadLine();

        server.EndListening();
    }
}