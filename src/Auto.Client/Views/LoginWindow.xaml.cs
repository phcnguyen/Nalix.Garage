using Auto.Client.ViewModels;
using System.Windows;

namespace Auto.Client.Views.Login;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();

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
            {
                viewModel.LoginCommand.Execute(null);
            }
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