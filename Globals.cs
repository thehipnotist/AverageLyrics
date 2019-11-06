﻿using System;
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
        public static List<RecordingItem> MatchingSongs = new List<RecordingItem>();

        public static Dictionary<string, int> LyricCount = new Dictionary<string, int>();
    }
}
