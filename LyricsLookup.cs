using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows;

namespace AverageLyrics
{    
    public class LyricsLookup : Globals
    {
        public static string TheseLyrics = "";
        
        public static async Task GetLyrics(string artistName, string songName)
        {
            var _baseAddress = new Uri("https://api.lyrics.ovh/v1/");
            using (var httpClient = new HttpClient { BaseAddress = _baseAddress })
            {
                using (var _response = await httpClient.GetAsync(artistName + "/" + songName))
                {
                    string _responseData = await _response.Content.ReadAsStringAsync();
                    string _lyrics = _responseData.ToLower();
                    _lyrics = _lyrics
                        .Replace("{", "")
                            .Replace("}", "")
                                .Replace("\"lyrics\":", "")
                                    .Replace("\"", "")
                                        .Replace("\'", "")
                                            .Replace("\\n", " ")
                                                .Replace("  "," ");
                    
                    TheseLyrics = _lyrics;
                    int _countWords = _lyrics.Count();
                    MessageBox.Show(_countWords.ToString());
                }
            }

        }
    }
}
