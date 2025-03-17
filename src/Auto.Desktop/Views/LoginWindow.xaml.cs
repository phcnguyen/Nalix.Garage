using Auto.Desktop.ViewModels;
using System.Windows;

namespace Auto.Desktop.Views.Login;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        ProgressWindow progressWindow = new();
        LoginViewModel viewModel = new();

        viewModel.ShowError += (message, title) =>
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        };

        viewModel.ShowLogin += () => this.Show();
        viewModel.HideLogin += () => this.Hide();

        viewModel.ShowProgress += () => progressWindow.Show();
        viewModel.HideProgress += () => progressWindow.Hide();

        DataContext = viewModel;

        txtUsername.Focus();
        txtPass.PasswordChanged += TxtPass_PasswordChanged;
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