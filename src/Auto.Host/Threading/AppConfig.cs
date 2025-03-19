using Auto.Database;
using Auto.Host.Services;
using Notio.Common.Logging;
using Notio.Logging;
using Notio.Logging.Internal.File;
using Notio.Logging.Targets;
using Notio.Network.Handlers;
using Notio.Network.Package;
using Notio.Network.Package.Helpers;
using Notio.Reflection;
using Notio.Shared;
using Notio.Shared.Memory.Buffers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Host.Threading;

public static class AppConfig
{
    public static readonly bool IsDebug = Debugger.IsAttached;
    public static readonly CancellationTokenSource CTokenSrc = new();
    public static readonly ManualResetEventSlim ExitEvent = new(false);

    public static readonly string VersionInfo =
        $"Version {AssemblyMetadata.GetAssemblyInformationalVersion()} | {(IsDebug ? "Debug" : "Release")}";

    // Lưu các phím tắt có thể chỉnh sửa
    private static readonly ConcurrentDictionary<ConsoleKey, Action> Shortcuts = new()
    {
        [ConsoleKey.Q] = () =>
        {
            CLogging.Instance.Info("Ctrl+Q pressed: Exiting application...");
            Process.GetCurrentProcess().Kill();
        },
        [ConsoleKey.R] = () => ThreadPool.QueueUserWorkItem(_ =>
        {
            Server?.BeginListening(CTokenSrc.Token);
        }),
        [ConsoleKey.O] = () => ThreadPool.QueueUserWorkItem(_ =>
        {
            Server?.EndListening();
        })
    };

    private static Network.ServerListener? Server;

    // Khởi tạo Console
    public static void InitializeConsole(Network.ServerListener server)
    {
        Server = server;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Title = $"Auto ({VersionInfo})";
        Console.OutputEncoding = Encoding.UTF8;
        Console.TreatControlCAsInput = false;
        Console.CursorVisible = false;

        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            CLogging.Instance.Warn("Ctrl+C has been disabled. Use an alternative shutdown method.");
        };

        //Console.Clear();

        PrintDynamicAsciiShortcutTable();
        ListenForKeyPresses();
    }

    public static AutoDbContext InitializeDatabase()
    {
        AutoDbContext context = new AutoDbContextFactory().CreateDbContext([]);
        context.Database.EnsureCreated();
        return context;
    }

    public static CLogging InitializeLogging()
    {
        FileLoggerOptions options = new()
        {
            LogFileName = "Auto",
            LogDirectory = DirectoriesDefault.LogsPath
        };

        return new CLogging(cfg =>
        {
            cfg.SetMinLevel(LoggingLevel.Trace)
               .AddTarget(new FileLoggingTarget(options))
               .AddTarget(new ConsoleLoggingTarget());
        });
    }

    public static Network.ServerListener InitializeServer(AutoDbContext dbContext)
    {
        return new(new Network.ServerProtocol(
            new PacketDispatcher(cfg => cfg.WithLogging(CLogging.Instance)
               .WithPacketSerialization(
                    packet => new System.Memory<byte>(PackageSerializeHelper.Serialize((Packet)packet)),
                    data => PackageSerializeHelper.Deserialize(data.Span))
               .WithErrorHandler(
                    (exception, command) => CLogging.Instance
                        .Error($"Error handling command:{command}", exception))
               .WithPacketCrypto(
                    (packet, connect) => PacketEncryptionHelper
                        .EncryptPayload((Packet)packet, connect.EncryptionKey, connect.Mode),
                    (packet, connect) => PacketEncryptionHelper
                        .DecryptPayload((Packet)packet, connect.EncryptionKey, connect.Mode))
               .WithPacketCompression(null, null)
               .WithHandler<SecureConnection>()
               .WithHandler(() => new AccountService(dbContext))
               .WithHandler(() => new VehicleService(dbContext))
               .WithHandler(() => new CustomerService(dbContext))
        )), new BufferAllocator(), CLogging.Instance);
    }

    // Hàm lắng nghe phím tắt
    private static void ListenForKeyPresses()
    {
        Task.Run(() =>
        {
            while (!ExitEvent.IsSet)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control) &&
                        Shortcuts.TryGetValue(keyInfo.Key, out var action))
                    {
                        action?.Invoke();
                    }
                }
                Thread.Sleep(10);
            }
        });
    }

    // Cho phép thay đổi phím tắt từ bên ngoài
    public static void SetShortcut(ConsoleKey key, Action action)
        => Shortcuts[key] = action;

    private static void PrintDynamicAsciiShortcutTable(Dictionary<string, string>? shortcuts = null, char borderChar = '─', ConsoleColor color = ConsoleColor.White)
    {
        shortcuts ??= new Dictionary<string, string>
        {
            ["Ctrl+Q"] = "Quit",
            ["Ctrl+R"] = "Start Server",
            ["Ctrl+O"] = "Stop Server"
        };

        var (maxKeyWidth, maxValueWidth) = GetColumnWidths(shortcuts);
        int col1Width = maxKeyWidth + 2;
        int col2Width = maxValueWidth + 2;

        var sb = new StringBuilder();
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        sb.Append('┌').Append(new string(borderChar, col1Width)).Append('┬').Append(new string(borderChar, col2Width)).Append('┐').AppendLine();
        sb.Append('│').Append(" SHORTCUTS".PadRight(col1Width)).Append('│').Append("".PadRight(col2Width)).Append('│').AppendLine();
        sb.Append('├').Append(new string(borderChar, col1Width)).Append('┬').Append(new string(borderChar, col2Width)).Append('┤').AppendLine();

        foreach (var (key, value) in shortcuts)
        {
            sb.Append('│').Append($" {key}".PadRight(col1Width)).Append('│').Append($" {value}".PadRight(col2Width)).Append('│').AppendLine();
        }

        sb.Append('└').Append(new string(borderChar, col1Width)).Append('┴').Append(new string(borderChar, col2Width)).Append('┘');

        Console.WriteLine(sb.ToString());
        Console.ForegroundColor = originalColor; // Khôi phục màu gốc
    }

    private static (int maxKeyWidth, int maxValueWidth) GetColumnWidths(Dictionary<string, string> shortcuts)
    {
        int maxKeyWidth = "SHORTCUTS".Length; // So sánh với tiêu đề
        int maxValueWidth = 0;

        foreach (var (key, value) in shortcuts)
        {
            if (key.Length > maxKeyWidth) maxKeyWidth = key.Length;
            if (value.Length > maxValueWidth) maxValueWidth = value.Length;
        }

        return (maxKeyWidth, maxValueWidth);
    }
}
