using Auto.Common.Enums;
using Auto.Common.Models;
using Auto.Desktop.Helpers;
using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Serialization;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Auto.Desktop.ViewModels;

public sealed class LoginViewModel : INotifyPropertyChanged
{
    public event Action? NewWindow;
    public event Action? ShowProgress;
    public event Action? HideProgress;

    private string _username = string.Empty;
    private string _password = string.Empty;

    public ICommand LoginCommand { get; }
    public event Action<string, string>? ShowMessage;
    public event PropertyChangedEventHandler? PropertyChanged;

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

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(ExecuteLoginAsync, CanExecuteLogin);
    }

    private async Task ExecuteLoginAsync()
    {
        ShowProgress?.Invoke();

        AccountModel account = new()
        {
            Username = Username,
            Password = Password
        };

        try
        {
            SocketClient.Instance.Send(new Packet(
                PacketType.Json, PacketFlags.None, PacketPriority.None,
                (ushort)Command.Login, Json.Serialize(account)));

            IPacket packetReceive = await SocketClient.Instance.ReceiveAsync();

            NewWindow?.Invoke();
        }
        catch (Exception ex)
        {
            ShowMessage?.Invoke($"Login failed: {ex.Message}", "Error");
        }
        finally
        {
            HideProgress?.Invoke();
        }
    }

    private void OnPropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private bool CanExecuteLogin()
        => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
}