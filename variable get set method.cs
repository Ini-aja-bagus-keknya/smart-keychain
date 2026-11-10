namespace SmartKeychain.Core.Models;

public class KeychainStatus
{
    // 1. Private variable (backing field)
    private int _batteryPercentage;
    private string _deviceName;

    // 2. Auto-Implemented Property
    public bool IsConnected { get; set; }
    public DateTime LastSyncTime { get; private set; }

    // 3. Property dengan validasi logika di dalam get & set
    public int BatteryPercentage
    {
        get => _batteryPercentage;
        set
        {
            // Validasi: baterai hanya boleh berada di rentang 0 - 100%
            if (value < 0) _batteryPercentage = 0;
            else if (value > 100) _batteryPercentage = 100;
            else _batteryPercentage = value;
        }
    }

    public string DeviceName
    {
        get => _deviceName;
        set => _deviceName = string.IsNullOrWhiteSpace(value) ? "Unknown Device" : value.Trim();
    }

    // Constructor
    public KeychainStatus(string deviceName, int initialBattery)
    {
        DeviceName = deviceName;
        BatteryPercentage = initialBattery;
        IsConnected = false;
        LastSyncTime = DateTime.Now;
    }

    // 4. Methods dengan return value dan parameter
    public void UpdateSyncTimestamp()
    {
        LastSyncTime = DateTime.Now;
    }

    public bool IsLowBattery(int threshold = 20)
    {
        return BatteryPercentage <= threshold;
    }
}