namespace Nalix.Garage.Host.Threading;

internal class Program
{
    internal static void Main()
    {
        AppConfig.SetLogger(AppConfig.InitializeLogging());

        AppConfig.InitializeConsole();
        AppConfig.SetDbContext(AppConfig.InitializeDatabase());
        AppConfig.SetServer(AppConfig.InitializeServer());

        AppConfig.ExitEvent.Wait();
    }
}