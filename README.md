# Smart Keychain (Cyber-Pet Robot Eye & Music Companion)

Gantungan kunci pintar mini (*smart interactive pocket/desk charm*) berbasis mikrokontroler **ESP32** dan layar mini (**OLED / Round LCD**) yang terhubung ke aplikasi komputer berbasis **C# (.NET)** untuk proyek mata kuliah **Pemrograman II**.

---

## ✨ Fitur Utama

- 🤖 **Expressive Robot Eyes:** Menampilkan animasi sepasang mata robot digital yang hidup (berkedip, melirik, tersenyum, mengantuk, atau *jamming* mengikuti musik).
- 🎵 **Now Playing Music Display:** Menampilkan judul lagu yang sedang diputar di PC/Laptop (Spotify, YouTube, browser), nama artis, progress bar durasi, serta animasi mini equalizer bar.
- 🔄 **Two-Way Interaction:** Pengguna bisa mengganti ekspresi dan mode dari aplikasi C# atau langsung menekan tombol fisik di gantungan kunci.
- 💻 **Virtual Keychain Preview (Mock Mode):** Dilengkapi emulator layar OLED langsung di dalam aplikasi C#, memungkinkan pengujian dan demo aplikasi 100% lancar di Mac/Windows tanpa harus selalu menyambungkan hardware fisik.
- 🧱 **Arsitektur OOP Solid:** Menerapkan Clean Architecture, MVVM, Abstraction, Polymorphism, dan Event-Driven Communication.

---

## 📖 Dokumentasi Lengkap Proyek

- 👉 **[PROJECT_CONTEXT.md](PROJECT_CONTEXT.md):** Panduan spesifikasi teknis lengkap, daftar alat & komponen hardware (BOM), skema pinout sirkuit, protokol pengiriman data, arsitektur kelas C#, hingga rencana pengujian.
- 👉 **[TEAM_CONVENTIONS.md](TEAM_CONVENTIONS.md):** Aturan standar tim 4 orang: konvensi penamaan variable/method/class, batasan arsitektur antar-layer, protokol data, git workflow, dan pembagian tugas.

---

## 🛠️ Ringkasan Kebutuhan Hardware

1. **ESP32 DevKit V1** atau **ESP32-C3 SuperMini** (Microcontroller IoT)
2. **Layar OLED 0.96" I2C SSD1306 (128x64)** (atau Layar Bulat 1.28" Round LCD GC9A01)
3. **Tactile Push Button 6x6mm** (Tombol ganti mode / ekspresi)
4. **Passive Buzzer Mini 3.3V** *(opsional)* (Suara efek robot 8-bit lucu)
5. **Modul Charger TP4056 + Baterai Li-Po 3.7V 300-500mAh** (Daya baterai portabel)
6. **Mini Breadboard & Kabel Jumper** (Media perakitan sirkuit)

---

## 💻 Tech Stack Perangkat Lunak

- **Bahasa:** C# (.NET 8 / .NET 9 LTS)
- **UI Framework:** Avalonia UI (Cross-platform Mac/Windows/Linux) atau WPF (.NET Windows)
- **Media Session Interop:** Windows Media Session Manager (`Windows.Media.Control`) & Cross-Platform Mock Service
- **Komunikasi:** Serial Port (`System.IO.Ports`) / Bluetooth Serial (SPP)
- **Firmware Mikrokontroler:** C++ (Arduino IDE / PlatformIO dengan library `U8g2` atau `Adafruit_SSD1306`)
