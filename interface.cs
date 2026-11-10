namespace SmartKeychain.Core.Interfaces;

// 1. INTERFACE: Kontrak murni
public interface ICommunicationChannel : IDisposable
{
    bool IsConnected { get; }
    Task ConnectAsync(string endpoint);
    Task DisconnectAsync();
    Task SendRawDataAsync(string data);
    event EventHandler<string> DataReceived;
}