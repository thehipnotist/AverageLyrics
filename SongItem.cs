using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverageLyrics
{
    public class SongItem
    {  
        public string Id { get; set; }
        public string Title { get; set; }
        //public string FirstRelease  { get; set; }
        //public int? Seconds { get; set; }
        //public int Score { get; set; }
        public int LyricCount { get; set; }

    //    public TimeSpan TrackLength
    //    {
    //        get { return TimeSpan.FromSeconds((double)Seconds); }
    //    }
    
    }
}
