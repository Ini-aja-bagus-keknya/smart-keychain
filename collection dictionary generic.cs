namespace SmartKeychain.Core.Collections;

using SmartKeychain.Core.Models;

public class MediaLibrary
{

    private readonly List<MediaTrackInfo> _tracks = new List<MediaTrackInfo>();

    // Dictionary: indeks judul -> lagu (abaikan beda huruf besar/kecil)
    private readonly Dictionary<string, MediaTrackInfo> _indexByTitle =
        new Dictionary<string, MediaTrackInfo>(StringComparer.OrdinalIgnoreCase);

    public int Count => _tracks.Count;

    // Dibungkus read-only agar isi list tidak diubah sembarangan dari luar
    public IReadOnlyList<MediaTrackInfo> All => _tracks.AsReadOnly();

    // Menambah lagu ke list sekaligus mendaftarkannya ke indeks dictionary
    public void Add(MediaTrackInfo track)
    {
        if (track is null) throw new ArgumentNullException(nameof(track));
        _tracks.Add(track);
        _indexByTitle[track.Title] = track; // judul sama -> entri lama ditimpa
    }

    // Pencarian cepat lewat Dictionary berdasarkan judul unik
    public MediaTrackInfo? GetByTitle(string title)
    {
        return _indexByTitle.TryGetValue(title, out MediaTrackInfo? found) ? found : null;
    }

    // Pencarian lewat List (dapat mengembalikan banyak hasil) berdasarkan artis
    public List<MediaTrackInfo> FindByArtist(string artist)
    {
        return _tracks.FindAll(t =>
            string.Equals(t.Artist, artist, StringComparison.OrdinalIgnoreCase));
    }

    // Menghapus lagu dari list dan indeks sekaligus
    public bool Remove(string title)
    {
        if (!_indexByTitle.TryGetValue(title, out MediaTrackInfo? track)) return false;
        _tracks.Remove(track);
        return _indexByTitle.Remove(title);
    }
}

// -----------------------------------------------------------------------------
// 2. DICTIONARY<K,V> dengan kunci bertipe enum.
// EmotionCommandRegistry memetakan emosi mata robot -> string perintah protokol
// FACE:<EmotionName> yang dikirim ke ESP32.
// -----------------------------------------------------------------------------
public class EmotionCommandRegistry
{
    private readonly Dictionary<RobotEmotion, string> _commandMap;

    public EmotionCommandRegistry()
    {
        // Inisialisasi pemetaan default (collection initializer)
        _commandMap = new Dictionary<RobotEmotion, string>
        {
            { RobotEmotion.Normal,  "FACE:NORMAL" },
            { RobotEmotion.Happy,   "FACE:HAPPY" },
            { RobotEmotion.Sleepy,  "FACE:SLEEPY" },
            { RobotEmotion.Blink,   "FACE:BLINK" },
            { RobotEmotion.Wink,    "FACE:WINK" },
            { RobotEmotion.Dizzy,   "FACE:DIZZY" },
            { RobotEmotion.Jamming, "FACE:JAMMING" }
        };
    }

    // Menambah / menimpa pemetaan via indexer
    public void Register(RobotEmotion emotion, string command) => _commandMap[emotion] = command;

    // Mengambil perintah dengan aman; null jika emosi belum terdaftar
    public string? GetCommand(RobotEmotion emotion)
    {
        return _commandMap.TryGetValue(emotion, out string? command) ? command : null;
    }
}

// -----------------------------------------------------------------------------
// 3. GENERIC CLASS
// RecentHistory<T> menyimpan N item terakhir dan bisa dipakai untuk tipe apa pun
// (riwayat lagu, riwayat emosi, dll.) tanpa kehilangan type-safety.
// -----------------------------------------------------------------------------
public class RecentHistory<T>
{
    private readonly List<T> _items = new List<T>();
    private readonly int _maxItems;

    public RecentHistory(int maxItems = 5)
    {
        // Minimal 1 item agar riwayat selalu bermakna
        _maxItems = maxItems < 1 ? 1 : maxItems;
    }

    public int Count => _items.Count;

    // Menambah item terbaru; item paling lama dibuang bila melebihi kapasitas
    public void Push(T newItem)
    {
        _items.Add(newItem);
        if (_items.Count > _maxItems) _items.RemoveAt(0);
    }

    public IReadOnlyList<T> GetAll() => _items.AsReadOnly();
}

public static class CollectionQuery
{
    public static T? FindFirst<T>(IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (T item in source)
        {
            if (predicate(item)) return item;
        }
        return default; 
    }
}
