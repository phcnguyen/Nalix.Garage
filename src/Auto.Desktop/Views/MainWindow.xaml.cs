using Auto.Desktop.ViewModels;
using Auto.Desktop.Views.Login;
using Notio.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Auto.Desktop.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        LoginWindow loginWindow = new();
        ProgressWindow progressWindow = new();

        progressWindow.Show();
        this.Hide();

        try
        {
            (bool Status, string Message) = await MainViewModel.EstablishServerConnectionAsync();
            if (!Status)
            {
                throw new Exception(Message);
            }

            await Task.Delay(2000);

            (bool Status, string Message) secureConnected = await MainViewModel.EstablishSecureConnectionAsync();
            if (!secureConnected.Status)
            {
                throw new Exception(secureConnected.Message);
            }

            await Task.Delay(2000);

            progressWindow.Close();
            loginWindow.Show();
            this.Close();
        }
        catch (Exception ex)
        {
            ex.Error(nameof(MainWindow), "Connection Error");
            progressWindow.Close();
            MessageBox.Show(ex.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }
    }
}