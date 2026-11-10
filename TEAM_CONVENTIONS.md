# Aturan & Standar Proyek (Team Conventions)

Dokumen ini adalah pedoman kerja resmi untuk seluruh anggota tim (4 orang) dalam mengembangkan proyek **Smart Keychain**. Tujuannya agar struktur kode seragam, tidak ada konflik saat penggabungan (*merge conflict*), dan arsitektur tetap bersih sesuai standar mata kuliah **Pemrograman II**.

---

## 1. Aturan Penamaan C# (Naming Conventions)

Semua anggota tim **WAJIB** mengikuti konvensi penamaan resmi Microsoft C# berikut:

| Elemen Kode | Format | Contoh Benar | Contoh Salah (DILARANG) |
|---|---|---|---|
| **Class & Struct** | `PascalCase` | `KeychainController`, `MediaTrack` | `keychain_controller`, `keychainController` |
| **Interface** | `PascalCase` diawali huruf `I` | `ICommunicationChannel`, `IMediaService` | `CommunicationChannel`, `I_CommunicationChannel` |
| **Abstract Class** | `PascalCase` (opsional diawali `Base`) | `BaseCommunicationChannel`, `DeviceBase` | `abstract_channel`, `baseChannel` |
| **Method** | `PascalCase` (kata kerja) | `ConnectAsync()`, `SendMediaUpdate()` | `connect()`, `send_media_update()` |
| **Property (`get`/`set`)** | `PascalCase` | `BatteryPercentage`, `IsConnected` | `battery_percentage`, `isConnected` |
| **Private Field (Variable Class)** | `_camelCase` (diawali garis bawah) | `_serialPort`, `_currentTrack`, `_isDisposed` | `serialPort`, `m_serialPort`, `_SerialPort` |
| **Parameter Method** | `camelCase` | `endpoint`, `targetEmotion`, `maxRetry` | `Endpoint`, `target_emotion` |
| **Local Variable (di dalam method)** | `camelCase` | `rawMessage`, `trackInfo`, `index` | `RawMessage`, `raw_message` |
| **Constant (`const`)** | `PascalCase` | `DefaultBaudRate = 115200;` | `DEFAULT_BAUD_RATE`, `default_baud_rate` |
| **Enum Type & Value** | `PascalCase` | `RobotEmotion.Happy`, `DeviceMode.Music` | `robot_emotion.happy`, `ROBOT_EMOTION.HAPPY` |
| **Event** | `PascalCase` (bentuk waktu lampau/present) | `DataReceived`, `TrackChanged` | `onDataReceived`, `data_received` |

### Contoh Penerapan Nyata:

```csharp
namespace SmartKeychain.Core.Services;

public class AnimationManager // PascalCase
{
    // Konstanta: PascalCase
    public const int MaxFrameRate = 30;

    // Private field: _camelCase
    private readonly Dictionary<RobotEmotion, string> _emotionMap;
    private int _currentFrameIndex;

    // Property: PascalCase
    public RobotEmotion ActiveEmotion { get; private set; }
    public bool IsAnimating => _currentFrameIndex > 0;

    // Constructor: PascalCase
    public AnimationManager(Dictionary<RobotEmotion, string> emotionMap)
    {
        // Parameter: camelCase
        _emotionMap = emotionMap ?? throw new ArgumentNullException(nameof(emotionMap));
        ActiveEmotion = RobotEmotion.Normal;
    }

    // Method: PascalCase
    public void SetEmotion(RobotEmotion newEmotion) // Parameter: camelCase
    {
        // Local variable: camelCase
        string commandCode = _emotionMap[newEmotion];
        ActiveEmotion = newEmotion;
    }
}
```

---

## 2. Aturan Struktur & Batasan Layer (Architectural Rules)

Proyek ini dibagi menjadi 3 lapisan utama. **Patuhi arah dependensi berikut:**

```
[SmartKeychain.UI]  ──>  bergantung pada  ──>  [SmartKeychain.Core]
        │                                              ▲
        └───>  bergantung pada  ───>  [SmartKeychain.Hardware]
```

### Aturan Ketat Antar-Layer:
1. **`SmartKeychain.Core` adalah Jantung Sistem:**
   - **DILARANG KERAS** mengimpor `System.IO.Ports` atau pustaka UI (WPF / Avalonia) di dalam `Core`.
   - `Core` hanya berisi: Model murni, Enum, Interface, Parser teks, dan Business Logic.
2. **`SmartKeychain.Hardware` adalah Pelaksana Fisik:**
   - Di sinilah tempat `System.IO.Ports` (Serial port fisik) dan driver khusus hardware berada.
   - Kelas di layer ini **WAJIB** mengimplementasikan interface dari `Core` (misal: `SerialPortChannel : ICommunicationChannel`).
3. **`SmartKeychain.UI` Tidak Boleh Koding Langsung di Code-Behind:**
   - **DILARANG** menulis kode pembuka port serial atau logika bisnis langsung di dalam file `MainWindow.xaml.cs` atau `MainWindow.axaml.cs`.
   - Seluruh interaksi UI harus melalui **ViewModel (`MainViewModel.cs`)** menggunakan *Command* dan *Data Binding*.

---

## 3. Standar Protokol Transmisi Data (Software $\leftrightarrow$ Hardware)

Agar anggota tim yang memprogram C# dan anggota tim yang memprogram ESP32 tidak saling bertabrakan format, format string telah dibakukan:

### Aturan Format:
- Setiap perintah diakhiri karakter **Newline (`\n`)**.
- Semua keyword perintah wajib menggunakan **HURUF KAPITAL**.
- Pemisah data menggunakan tanda **Titik Dua (`:`)** dan tanda **Pipa (`|`)**.

```
MEDIA:<Artist>|<Title>|<DurationSec>|<PositionSec>|<State>\n
Contoh: MEDIA:Queen|Bohemian Rhapsody|354|120|PLAY\n

FACE:<EmotionName>\n
Contoh: FACE:HAPPY\n  atau  FACE:SLEEPY\n  atau  FACE:WINK\n

EVT:BTN:CLICK\n   (Dikirim ESP32 saat tombol ditekan sebentar)
EVT:BTN:HOLD\n    (Dikirim ESP32 saat tombol ditekan lama)
```

> ⚠️ **Peringatan Tim:** Dilarang mengubah struktur format string di atas tanpa musyawarah dan persetujuan seluruh anggota tim.

---

## 4. Standar Alur Kerja Git (Git Workflow untuk 4 Orang)

### 4.1 Struktur Branch
- **`main`**: Branch stabil. Hanya berisi kode yang **bisa dicompile dan siap didemokan**. Dilarang melakukan `git push` langsung ke `main`!
- **`dev`**: Branch integrasi. Tempat menggabungkan fitur-fitur yang sudah selesai.
- **Branch Fitur Mandiri:** Setiap anggota membuat branch sendiri dari `dev`:
  - Format: `feat/<nama-fitur>-<nama-anggota>`
  - Contoh: `feat/media-listener-budi`, `feat/oled-renderer-siti`, `feat/ui-preview-agus`

### 4.2 Format Pesan Commit (Conventional Commits)
Gunakan format pesan yang jelas dan informatif:
- `feat: tambah parser untuk protokol MEDIA`
- `fix: perbaiki bug auto-reconnect saat kabel dicabut`
- `refactor: pisahkan method kirim serial ke class terpisah`
- `docs: perbarui skema wiring pinout di README`

### 4.3 Aturan Sebelum Melakukan Merge / Pull Request
Sebelum meminta merge ke branch `dev`:
1. Jalankan `dotnet build` di terminal dan pastikan **0 Error**.
2. Pastikan file temporer/sampah tidak ikut ter-commit (folder `bin/`, `obj/`, `.vs/` harus ada di `.gitignore`).

---
