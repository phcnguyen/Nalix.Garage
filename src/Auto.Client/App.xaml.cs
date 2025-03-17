using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Auto.Application;

/// <summary>
/// Interaction logic for Application.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    [LibraryImport("kernel32.dll", EntryPoint = "AllocConsoleA", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    protected override void OnStartup(StartupEventArgs e)
    {
        AllocConsole();

        base.OnStartup(e);

        Console.WriteLine("Ứng dụng WPF đã khởi động với console!");
    }
}