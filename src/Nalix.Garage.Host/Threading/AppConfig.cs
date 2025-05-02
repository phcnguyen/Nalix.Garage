using Nalix.Assemblies;
using Nalix.Common.Logging;
using Nalix.Cryptography.Asymmetric;
using Nalix.Cryptography.Hashing;
using Nalix.Environment;
using Nalix.Garage.Database;
using Nalix.Garage.Host.Network;
using Nalix.Garage.Host.Services;
using Nalix.Logging;
using Nalix.Logging.Options;
using Nalix.Logging.Targets;
using Nalix.Network.Dispatch;
using Nalix.Network.Dispatch.BuiltIn;
using Nalix.Network.Package;
using Nalix.Shared.Memory.Buffers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nalix.Garage.Host.Threading;

public static class AppConfig
{
    private static ServerListener? Server;
    private static AutoDbContext? DbContext;
    private static ILogger Logger = NLogix.Host.Instance;

    public static readonly bool IsDebug = Debugger.IsAttached;
    public static readonly CancellationTokenSource CTokenSrc = new();
    public static readonly ManualResetEventSlim ExitEvent = new(false);

    public static string VersionInfo =>
        $"Version {AssemblyInspector.GetAssemblyInformationalVersion()} | {(IsDebug ? "Debug" : "Release")}";

    private static readonly ConcurrentDictionary<ConsoleKey, Action> Shortcuts = new()
    {
        [ConsoleKey.H] = () => ShowHelp(),
        [ConsoleKey.Q] = () =>
        {
            Logger.Info("Ctrl+Q pressed: Exiting application...");
            System.Environment.Exit(0);
        },
        [ConsoleKey.R] = () =>
        {
            if (Server != null) ThreadPool.QueueUserWorkItem(_ => Server.BeginListening(CTokenSrc.Token));
        },
        [ConsoleKey.P] = () =>
        {
            if (Server != null) Task.Run(() => Server.EndListening());
        }
    };

    public static void InitializeConsole()
    {
        Console.CursorVisible = false;
        Console.TreatControlCAsInput = false;
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = $"Auto ({VersionInfo})";
        Console.ResetColor();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            Logger.Warn("Ctrl+C is disabled. Use Ctrl+Q to exit.");
        };

        Console.Clear();
        ListenForKeyPresses();
    }

    public static AutoDbContext InitializeDatabase()
    {
        var context = new AutoDbContextFactory().CreateDbContext([]);
        context.Database.EnsureCreated();
        return context;
    }

    public static ILogger InitializeLogging()
    {
        FileLogOptions options = new()
        {
            LogFileName = "Auto",
            LogDirectory = Directories.LogsPath,
        };

        return new NLogix(cfg =>
        {
            cfg.SetMinLevel(LogLevel.Information)
               .AddTarget(new FileLogTarget(options))
               .AddTarget(new ConsoleLogTarget());
        });
    }

    public static ServerListener InitializeServer()
    {
        if (DbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        return new ServerListener(
               new ServerProtocol(new PacketDispatch<Packet>(cfg => cfg
                   .WithLogging(Logger)
                   .WithErrorHandling((exception, command) =>
                        Logger.Error($"Error handling command: {command}", exception))
                   .WithHandler(() => new HandshakeController<Packet>(new SHA256(), new X25519(), Logger))
                   .WithHandler(() => new AccountService(DbContext))
                   .WithHandler(() => new VehicleService(DbContext))
                   .WithHandler(() => new CustomerService(DbContext))
        )), new BufferAllocator(), Logger);
    }

    public static void SetServer(ServerListener server) => Server = server;

    public static void SetDbContext(AutoDbContext dbContext) => DbContext = dbContext;

    public static void SetLogger(ILogger logger) => Logger = logger;

    private static void ListenForKeyPresses()
    {
        Logger.Warn("Ctrl+H detected → Showing help menu...");

        Task.Run(async () =>
        {
            while (!ExitEvent.IsSet)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control) &&
                        Shortcuts.TryGetValue(keyInfo.Key, out var action))
                    {
                        action?.Invoke();
                    }
                }
                await Task.Delay(10);
            }
        });
    }

    private static void ShowHelp()
    {
        Logger.Info(@"Available shortcuts:
                        Ctrl+H → Show help
                        Ctrl+Q → Exit application
                        Ctrl+R → Restart server
                        Ctrl+P → Stop server
        ");
    }

    public static void SetShortcut(ConsoleKey key, Action action) => Shortcuts[key] = action;
}