namespace SmartKeychain.Core.Channels;

using SmartKeychain.Core.Interfaces;

// 2. ABSTRACT CLASS: Kerangka dasar yang mengimplementasikan interface
public abstract class BaseCommunicationChannel : ICommunicationChannel
{
    // Protected variable: hanya bisa diakses oleh kelas turunan
    protected bool _isDisposed;

    public bool IsConnected { get; protected set; }
    public string ActiveEndpoint { get; protected set; } = string.Empty;

    // Event menggunakan Generic EventHandler<T>
    public event EventHandler<string>? DataReceived;

    // Method concrete (sudah ada isinya dan dipakai semua turunan)
    protected void OnDataReceived(string message)
    {
        // Memanggil event jika ada listener/subscriber
        DataReceived?.Invoke(this, message);
    }

    // Abstract method: WAJIB diimplementasikan secara berbeda oleh kelas turunan
    public abstract Task ConnectAsync(string endpoint);
    public abstract Task DisconnectAsync();
    public abstract Task SendRawDataAsync(string data);

    // Virtual method: Boleh di-override jika kelas turunan butuh logic tambahan
    public virtual void Dispose()
    {
        _isDisposed = true;
    }
}