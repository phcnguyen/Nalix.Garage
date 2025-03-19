using Auto.Common.Dto;
using Auto.Common.Enums;
using Auto.Desktop.Helpers;
using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Utilities;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Auto.Desktop.ViewModels;

public sealed class LoginViewModel : INotifyPropertyChanged
{
    public event Action? NewWindow;
    public event Action? ShowProgress;
    public event Action? HideProgress;

    public event Action? ShowLogin;
    public event Action? HideLogin;

    public event Action<string, string>? ShowError;

    private string _username = string.Empty;
    private string _password = string.Empty;

    public ICommand LoginCommand { get; }
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
        HideLogin?.Invoke();
        ShowProgress?.Invoke();

        AccountDto account = new()
        {
            Username = Username,
            Password = Password
        };

        try
        {
            await SocketClient.Instance.SendAsync(new Packet(
                PacketType.Json, PacketFlags.None, PacketPriority.None,
                (ushort)Command.Login, JsonBinary.SerializeToBytes(account, JsonContext.Default.AccountDto)));

            int timeoutMilliseconds = 10000;
            var receiveTask = SocketClient.Instance.ReceiveAsync();
            var timeoutTask = Task.Delay(timeoutMilliseconds);

            var completedTask = await Task.WhenAny(receiveTask, timeoutTask);

            if (completedTask == timeoutTask)
                throw new TimeoutException("Server did not respond in time.");

            IPacket packetReceive = await receiveTask;

            if (packetReceive.Command == 1)
            {
                NewWindow?.Invoke();
            }
            else
            {
                if (packetReceive.Type == (byte)PacketType.String)
                    ShowError?.Invoke(Encoding.UTF8.GetString(packetReceive.Payload.Span), "Error");
            }
        }
        catch (Exception ex)
        {
            ShowError?.Invoke($"Login failed: {ex.Message}", "Error");
        }
        finally
        {
            ShowLogin?.Invoke();
            HideProgress?.Invoke();
        }
    }

    private void OnPropertyChanged(string propertyName)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private bool CanExecuteLogin()
        => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
}