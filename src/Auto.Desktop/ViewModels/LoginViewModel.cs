using Auto.Common.Dto;
using Auto.Common.Enums;
using Auto.Desktop.Sockets;
using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Utilities;
using System;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Auto.Desktop.ViewModels;

public sealed class LoginViewModel : ViewModelBase
{
    public event Action? NewWindow;
    public event Action? ShowWindow;
    public event Action? HideWindow;
    public event Action? ShowProgress;
    public event Action? HideProgress;

    public event Action<string, string, MessageBoxImage>? MessageBox;

    public ICommand LoginCommand { get; }

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
        LoginCommand = new ViewModelCommand(ExecuteAuth, CanExecuteAuth);
    }

    private void ExecuteAuth()
    {
        HideWindow?.Invoke();
        ShowProgress?.Invoke();

        AccountDto account = new() { Username = Username, Password = Password };

        try
        {
            NetworkClient.Instance.Send(new Packet(
                PacketType.Json, PacketFlags.None, PacketPriority.None,
                (ushort)Command.Login, JsonBuffer.SerializeToBytes(account, JsonContext.Default.AccountDto)));

            IPacket packetReceive = NetworkClient.Instance.Receive();

            if (packetReceive.Command == 1)
            {
                NewWindow?.Invoke();
            }
            else if (packetReceive.Type == (byte)PacketType.String)
            {
                MessageBox?.Invoke(Encoding.UTF8.GetString(packetReceive.Payload.Span), "Error", MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox?.Invoke($"{ex.Message}", "Error", MessageBoxImage.Error);
        }
        finally
        {
            ShowWindow?.Invoke();
            HideProgress?.Invoke();
        }
    }

    private bool CanExecuteAuth() =>
        !string.IsNullOrWhiteSpace(Username) &&
        !string.IsNullOrWhiteSpace(Password);
}
