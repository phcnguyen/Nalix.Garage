using Auto.Database;
using Auto.Host.Network;
using Auto.Host.Services;
using Notio.Common.Logging;
using Notio.Common.Package;
using Notio.Defaults;
using Notio.Logging;
using Notio.Logging.Internal.File;
using Notio.Logging.Targets;
using Notio.Network.Package;
using Notio.Network.Package.Security;
using Notio.Network.Package.Serialization;
using Notio.Network.PacketProcessing;
using Notio.Reflection;
using Notio.Shared.Memory.Buffers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Host.Threading;

public static class AppConfig
{
    private static ServerListener? Server;
    private static AutoDbContext? DbContext;
    private static ILogger Logger = CLogging.Instance;

    public static readonly bool IsDebug = Debugger.IsAttached;
    public static readonly CancellationTokenSource CTokenSrc = new();
    public static readonly ManualResetEventSlim ExitEvent = new(false);

    public static string VersionInfo =>
        $"Version {AssemblyMetadata.GetAssemblyInformationalVersion()} | {(IsDebug ? "Debug" : "Release")}";

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
        FileLoggerOptions options = new()
        {
            LogFileName = "Auto",
            LogDirectory = DefaultDirectories.LogsPath,
        };

        return new CLogging(cfg =>
        {
            cfg.SetMinLevel(LogLevel.Information)
               .AddTarget(new FileLoggingTarget(options))
               .AddTarget(new ConsoleLoggingTarget());
        });
    }

    public static ServerListener InitializeServer()
    {
        if (DbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        return new(new ServerProtocol(
            new PacketDispatcher(cfg => cfg
               .WithLogging(Logger)
               .WithErrorHandler((exception, command) =>
                    Logger.Error($"Error handling command: {command}", exception))
               .WithTypedDecryption<IPacket>(
                   (p, c) => PacketEncryption.DecryptPayload((Packet)p, c.EncryptionKey, c.Mode))
               .WithTypedEncryption<IPacket>(
                    (p, c) => PacketEncryption.EncryptPayload((Packet)p, c.EncryptionKey, c.Mode))
               .WithTypedDeserializer<IPacket>(data => PacketSerializationHelper.Deserialize(data.Span))
               .WithTypedSerializer<IPacket>(
                    packet => new Memory<byte>(PacketSerializationHelper.Serialize((Packet)packet)))
               .WithHandler<SecureConnection>()
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
