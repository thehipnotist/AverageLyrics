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
        public string Language { get; set; }
        public int LyricCount { get; set; }

        public int CountValue
        {
            get { return (LyricCount >= 0) ? LyricCount : 0; }
        }
        public string CountResult
        {
            get { return (LyricCount >= 0) ? LyricCount.ToString() : "Not found"; }
        }
    }
}
