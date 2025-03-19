using Auto.Database;
using Auto.Host.Network;

namespace Auto.Host.Threading;

internal class Program
{
    internal static void Main()
    {
        AutoDbContext database = AppConfig.InitializeDatabase();
        ServerListener server = AppConfig.InitializeServer(database);

        AppConfig.InitializeConsole(server);

        AppConfig.ExitEvent.Wait();
    }
}