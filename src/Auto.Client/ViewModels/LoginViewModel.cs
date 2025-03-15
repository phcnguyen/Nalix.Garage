using Auto.Client.Helpers;
using Auto.Common.Enums;
using Auto.Common.Models;
using Notio.Common.Data;
using Notio.Cryptography.Asymmetric;
using Notio.Cryptography.Hash;
using Notio.Network.Package;
using Notio.Network.Package.Enums;
using Notio.Serialization;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Auto.Client.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
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
        try
        {
            SocketClient.Instance.Connect("192.168.1.4", 5000);
            //InitiateSecureConnection();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error connecting to server: {ex.Message}", "Connection Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            Environment.Exit(1);
        }
    }

    protected void OnPropertyChanged(string propertyName)
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

    private static void InitiateSecureConnection()
    {
        try
        {
            (byte[] privateKey, byte[] publicKey) = X25519.GenerateKeyPair();

            Packet packet = new(
                PacketType.Binary, PacketFlags.None, PacketPriority.None,
                (ushort)Command.InitiateSecureConnection, publicKey);

            SocketClient.Instance.Send(packet);
            IPacket packetReceive = SocketClient.Instance.Receive();

            if (packetReceive == null || packetReceive.Payload.Length != 32)
            {
                throw new Exception(
                    $"Invalid packet received. Expected 32 bytes, got {packetReceive?.Payload.Length ?? 0}.");
            }

            byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packetReceive.Payload.ToArray());
            SocketClient.Instance.EncryptionKey = Sha256.HashData(sharedSecret);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error initiating secure connection: {ex.Message}",
                "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}