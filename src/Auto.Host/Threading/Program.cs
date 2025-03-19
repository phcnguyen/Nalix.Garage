using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Host.Threading;

internal class Program
{
    private static readonly CancellationTokenSource cancellationTokenSource = new();
    internal static void Main()
    {
        var server = AppConfig.InitializeServer(AppConfig.InitializeDatabase());

        // Chạy server trong một task riêng
        Task.Run(() => server.BeginListening(cancellationTokenSource.Token), cancellationTokenSource.Token);

        Console.WriteLine("Press any key to exit...");
        Console.ReadLine();

        // Hủy task và dừng server
        cancellationTokenSource.Cancel();
        server.EndListening();
    }
}