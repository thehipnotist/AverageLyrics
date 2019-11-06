using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AverageLyrics
{
    public class Globals
    {
        public const string DefaultArtistText = "Enter Artist Name";
        public const string AllRecords = "<All>";
        public static string SelectedType = AllRecords;
        public static string SingleSpace = " ";
        public static string DoubleSpace = "  ";

        public static List<ArtistItem> MatchingArtists = new List<ArtistItem>();
        public static ArtistItem SelectedArtist = null;
        public static List<SongItem> MatchingSongs = new List<SongItem>();
        public static List<SongItem> SelectedSongs = new List<SongItem>();
        public static Dictionary<string, int> LyricCount = new Dictionary<string, int>();
        public static List<string> ArtistTypes = new List<string>();

        public static string FormatLyrics(string rawLyrics)
        {
            string _formattedLyrics = rawLyrics.ToLower();
            _formattedLyrics = _formattedLyrics
                .Replace("{", "")
                    .Replace("}", "")
                        .Replace("\"lyrics\":", "")
                            .Replace("\"instrumental\"", "")
                                .Replace("\"", "")
                                    .Replace("\'", "")
                                        .Replace("\\n", SingleSpace)                                            
                                                .Trim();

            while (_formattedLyrics.Length > 0 && _formattedLyrics.IndexOf(DoubleSpace) != -1)
            {
                _formattedLyrics = _formattedLyrics.Replace(DoubleSpace, SingleSpace);
            }            
            return _formattedLyrics;
        }

        public static double LyricAverage()
        {
            if (SelectedSongs == null || SelectedSongs.Count == 0) { return 0; }
            else { return SelectedSongs.Average(ss => ss.LyricCount); }
        }

    }
}
