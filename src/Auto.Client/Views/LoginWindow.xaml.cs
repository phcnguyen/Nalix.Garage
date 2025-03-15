using Auto.Client.ViewModels;
using System.Windows;

namespace Auto.Client.Views.Login;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }

    private void TxtPass_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = txtPass.Password; // Cập nhật Password vào ViewModel
            viewModel.OnPropertyChanged(nameof(viewModel.Password));
        }
    }
}