namespace SmartKeychain.Hardware.Channels;

using SmartKeychain.Core.Channels;

// INHERITANCE: SerialPortChannel mewarisi BaseCommunicationChannel
public class SerialPortChannel : BaseCommunicationChannel
{
    // Override method abstrak dari kelas induk
    public override async Task ConnectAsync(string endpoint)
    {
        // Logika koneksi ke port fisik COM3 / /dev/ttyUSB0
        ActiveEndpoint = endpoint;
        IsConnected = true;
        Console.WriteLine($"[Serial] Terhubung ke port fisik: {endpoint}");
        await Task.CompletedTask;
    }

    public override async Task DisconnectAsync()
    {
        IsConnected = false;
        Console.WriteLine("[Serial] Terputus dari port fisik.");
        await Task.CompletedTask;
    }

    public override async Task SendRawDataAsync(string data)
    {
        if (!IsConnected) throw new InvalidOperationException("Port belum dibuka!");
        // Simulasi pengiriman data fisik ke mikrokontroler
        Console.WriteLine($"[Serial Tx] Mengirim ke ESP32: {data}");
        await Task.CompletedTask;
    }
}

// INHERITANCE KEDUA: MockChannel untuk simulator di laptop
public class MockCommunicationChannel : BaseCommunicationChannel
{
    public override async Task ConnectAsync(string endpoint)
    {
        ActiveEndpoint = "VIRTUAL_EMULATOR";
        IsConnected = true;
        Console.WriteLine("[Mock] Simulator hardware aktif.");
        await Task.CompletedTask;
    }

    public override async Task DisconnectAsync()
    {
        IsConnected = false;
        await Task.CompletedTask;
    }

    public override async Task SendRawDataAsync(string data)
    {
        Console.WriteLine($"[Mock Echo] Menerima data di layar simulator: {data}");
        await Task.CompletedTask;
    }

    // Method khusus simulator untuk memicu penekanan tombol fisik secara buatan
    public void TriggerVirtualButtonPress()
    {
        // Memanggil protected method milik kelas induk
        OnDataReceived("EVT:BTN:CLICK");
    }
}