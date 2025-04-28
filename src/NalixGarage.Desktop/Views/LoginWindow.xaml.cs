using NalixGarage.Desktop.Sockets;
using NalixGarage.Desktop.ViewModels;
using Notio.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Auto.Desktop.Views.Login;

public partial class LoginWindow : Window
{
    public static readonly ProgressWindow ProgressWindow = new();
    public LoginWindow()
    {
        InitializeComponent();

        NetworkClient.Instance.Set("192.168.1.3", 5000);
        LoginViewModel viewModel = new();

        viewModel.MessageBox += (message, title, messageBoxImage) =>
        {
            ProgressWindow.Hide();
            MessageBox.Show(message, title, MessageBoxButton.OK, messageBoxImage);
        };

        viewModel.ShowWindow += () => this.Show();
        viewModel.HideWindow += () => this.Hide();

        viewModel.ShowProgress += () => ProgressWindow.Show();
        viewModel.HideProgress += () => ProgressWindow.Hide();

        DataContext = viewModel;

        txtUsername.Focus();
        txtPass.PasswordChanged += TxtPass_PasswordChanged;

        Loaded += Login_Loaded;
    }

    private async void Login_Loaded(object sender, RoutedEventArgs e)
        => await PerformLogin();

    private async Task PerformLogin()
    {
        this.Hide();
        ProgressWindow.Show();

        try
        {
            (bool Status, string Message) = await NetworkClient.Instance.SecureHandshakeAsync();
            if (!Status)
            {
                throw new Exception(Message);
            }

            await Task.Delay(2000);

            ProgressWindow.Hide();
            this.Show();
        }
        catch (Exception ex)
        {
            ex.Error(nameof(LoginWindow), "Connection");
            ProgressWindow.Hide();
            MessageBoxResult result = MessageBox.Show(
                $"{ex.Message}\nDo you want reload ?", "Connection", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (result == MessageBoxResult.No)
            {
                Application.Current.Shutdown();
            }
            else if (result == MessageBoxResult.Yes)
            {
                await PerformLogin();
            }
        }
    }

    private void TxtUsername_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Enter)
        {
            e.Handled = true;
            txtPass.Focus();
        }
    }
    private void TxtPass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Enter)
        {
            e.Handled = true;

            if (DataContext is LoginViewModel viewModel &&
                viewModel.LoginCommand.CanExecute(null))
                viewModel.LoginCommand.Execute(null);
        }
    }

    private void TxtPass_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = txtPass.Password;
        }
    }
}