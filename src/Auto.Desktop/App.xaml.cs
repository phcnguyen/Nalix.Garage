using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Auto.Desktop;

/// <summary>
/// Interaction logic for Desktop.xaml
/// </summary>
public partial class App : Application
{
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    protected override void OnStartup(StartupEventArgs e)
    {
        AllocConsole();

        base.OnStartup(e);

        Console.WriteLine("WPF application started with console!");
    }
}