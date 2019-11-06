using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hqub.MusicBrainz.API;
using Hqub.MusicBrainz.API.Entities;
using Hqub.MusicBrainz.API.Entities.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace AverageLyrics
{
    public class MusicBrainzLookup : Globals
    {
        public static void SetArtistTypes()
        {
            try
            {
                // Couldn't find a way to get this list from the API
                ArtistTypes.Add(AllRecords);
                ArtistTypes.Add("Person");
                ArtistTypes.Add("Group");
                ArtistTypes.Add("Orchestra");
                ArtistTypes.Add("Choir");
                ArtistTypes.Add("Character");
                ArtistTypes.Add("Other");
            }
            catch (Exception exp) { MessageBox.Show("Error listing artist types: " + exp.Message); }
        }        
        
        public static async Task LookupArtist(string enteredName, string artistType)
        {
            try
            {
                MatchingArtists.Clear();

                var _artistQuery = new QueryParameters<Artist>()
                {
                    { "artist", enteredName }                    
                };

                if (artistType != "" && artistType != AllRecords) { _artistQuery.Add("type", artistType); }
                
                //var _foundArtists = await Artist.SearchAsync("\"" + enteredName + "\"", 10);
                var _foundArtists = await Artist.SearchAsync(_artistQuery, 10);
                if (_foundArtists != null && _foundArtists.Items.Count > 0)
                {
                    foreach (var a in _foundArtists.Items)
                    {
                        var _artist = new ArtistItem
                        {
                            Score = a.Score,
                            Id = a.Id,
                            Name = a.Name,
                            Country = (a.Country != null && a.Country.Trim() != "")? a.Country : "Various/Unknown",
                            Type = a.Type
                        };
                        MatchingArtists.Add(_artist);
                    }
                }
                else
                {
                    MessageBox.Show("Could not find an artist called '" + enteredName + "'.");
                }
            }
            catch (Exception exp) { MessageBox.Show("Error finding artist " + enteredName + ": " + exp.Message); }
        }

        public static async Task LookupSongs(ArtistItem artist)
        {
            try
            {                
                MatchingSongs.Clear();
                LyricCount.Clear();

                var _works = await Artist.GetAsync(artist.Id, "works");
                foreach (var w in _works.Works)
                {
                    if (MatchingSongs != null && MatchingSongs.Exists(m => m.Title == w.Title)) { continue; }
                    else { await addSong(artist.Name, w); }
                }

                var _recordingsQuery = new QueryParameters<Recording>()
                {
                    { "arid", artist.Id }
                };
                var _records = await Recording.SearchAsync(_recordingsQuery, 500);
                var _recordings = _records.Items.ToList();

                foreach (var rec in _recordings)
                {
                    Thread.Sleep(500); 
                    var _recording = await Recording.GetAsync(rec.Id, "work-rels");
                    var _recordingList = _recording.Relations.Where(r => r.Work != null).ToList();
                    foreach (var v in _recordingList)
                    {
                        Thread.Sleep(500); 
                        var w = v.Work;
                        if (w == null || (MatchingSongs != null && MatchingSongs.Exists(m => m.Title == w.Title))) { continue; }
                        else { await addSong(artist.Name, w); }
                    }
                }

                Globals.MatchingSongs = Globals.MatchingSongs.OrderBy(ms => ms.Title).ToList();
            }
            catch (Exception exp) { MessageBox.Show("Error finding songs for artist " + artist.Name + ": " + exp.Message); }
        }

        private static async Task addSong(string artistName, Work w)
        {
            await LyricsLookup.GetLyrics(artistName, w.Title);
            var _lyricCount = 0;
            LyricCount.TryGetValue(w.Title, out _lyricCount);
            
            var _song = new SongItem
            {
                Id = w.Id,
                Title = w.Title,
                Language = w.Language,
                LyricCount = _lyricCount
            };
            Globals.MatchingSongs.Add(_song);
        }

    }
}
