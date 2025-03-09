using System.Windows.Input;

namespace Auto.Client.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _username = string.Empty;
    private string _password = string.Empty;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(Login, CanLogin);
    }

    public ICommand LoginCommand { get; }

    private bool CanLogin(object parameter)
        => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

    private void Login(object parameter)
    {
        string data = $"{Username}|{Password}";
    }
}
