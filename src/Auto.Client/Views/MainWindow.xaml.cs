using Auto.Application.ViewModels;
using Auto.Application.Views.Login;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Auto.Application.Views;

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
            bool serverConnected = MainViewModel.EstablishServerConnection(out string serverError);
            if (!serverConnected)
            {
                throw new Exception(serverError);
            }

            await Task.Delay(1000);

            bool secureConnected = MainViewModel.EstablishSecureConnection(out string secureError);
            if (!secureConnected)
            {
                throw new Exception(secureError);
            }

            await Task.Delay(1000);

            progressWindow.Close();
            loginWindow.Show();
            this.Close();
        }
        catch (Exception ex)
        {
            progressWindow.Close();
            MessageBox.Show(ex.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            System.Windows.Application.Current.Shutdown();
        }
    }
}