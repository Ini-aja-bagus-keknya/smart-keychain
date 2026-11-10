namespace SmartKeychain.Core.Models;


public class MediaTrackInfo
{
    private int _positionSec;

    public string Artist { get; }
    public string Title { get; }
    public int DurationSec { get; }

    public int PositionSec
    {
        get => _positionSec;
        private set
        {
            if (value < 0) _positionSec = 0;
            else if (value > DurationSec) _positionSec = DurationSec;
            else _positionSec = value;
        }
    }

    public bool IsPlaying { get; private set; }

    // Constructor: PascalCase, parameter camelCase
    public MediaTrackInfo(string artist, string title, int durationSec)
    {
        Artist = string.IsNullOrWhiteSpace(artist) ? "Unknown Artist" : artist.Trim();
        Title = string.IsNullOrWhiteSpace(title) ? "Unknown Title" : title.Trim();
        DurationSec = durationSec < 0 ? 0 : durationSec;
    }

    public void UpdateProgress(int positionSec, bool isPlaying)
    {
        PositionSec = positionSec;
        IsPlaying = isPlaying;
    }

    public override string ToString() => $"{Title} - {Artist}";
}
