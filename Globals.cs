using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverageLyrics
{
    public class Globals
    {
        public static string DefaultArtistText = "Enter Artist Name";
        public static List<ArtistItem> MatchingArtists = new List<ArtistItem>();
        public static ArtistItem SelectedArtist = null;
        public static List<SongItem> MatchingSongs = new List<SongItem>();
        public static List<SongItem> SelectedSongs = new List<SongItem>();
        public static Dictionary<string, int> LyricCount = new Dictionary<string, int>();

        public static string FormatLyrics(string rawLyrics)
        {
            string _formattedLyrics = rawLyrics.ToLower().Trim();
            _formattedLyrics = _formattedLyrics
                .Replace("{", "")
                    .Replace("}", "")
                        .Replace("\"lyrics\":", "")
                            .Replace("\"", "")
                                .Replace("\'", "")
                                    .Replace("\\n", " ")
                                        .Replace("  ", " ");
            return _formattedLyrics;
        }

        public static double LyricAverage()
        {
            if (SelectedSongs == null || SelectedSongs.Count == 0) { return 0; }
            else { return SelectedSongs.Average(ss => ss.LyricCount); }
        }

    }
}
