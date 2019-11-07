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
        public static async Task GetLyrics(string artistName, string songName)
        {
            var _baseAddress = new Uri("https://api.lyrics.ovh/v1/");
            using (var httpClient = new HttpClient { BaseAddress = _baseAddress })
            {
                using (var _response = await httpClient.GetAsync(artistName + "/" + songName))
                {
                    int _countWords = 0;
                    string _responseData = await _response.Content.ReadAsStringAsync();
                    if (_responseData.ToLower().Trim().StartsWith("{\"error\":")) { _countWords = -1; }
                    else
                    {
                        string _lyrics = FormatLyrics(_responseData);
                        _countWords = (_lyrics == "")? 0 : _lyrics.Count(l => l == ' ') + 1;
                        //if (_countWords <10) { Clipboard.SetText(_responseData); }
                    }
                    LyricCount.Add(songName, _countWords);
                }
            }
        }
    }
}
