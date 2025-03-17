using Auto.Application.Helpers;
using Auto.Common.Enums;
using Auto.Common.Models;
using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Serialization;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Auto.Application.ViewModels;

public sealed class LoginViewModel : INotifyPropertyChanged
{
    private string _username = string.Empty;
    private string _password = string.Empty;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
    }

    private void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private bool CanExecuteLogin()
        => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

    private void ExecuteLogin()
    {
        AccountModel account = new()
        {
            Username = Username,
            Password = Password
        };

        SocketClient.Instance.Send(new Packet(
            PacketType.Json, PacketFlags.None, PacketPriority.None,
            (ushort)Command.Login, Json.Serialize(account)));

        IPacket packetReceive = SocketClient.Instance.Receive();

        MessageBox.Show(Encoding.UTF8.GetString(packetReceive.Payload.Span),
            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
