using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hqub.MusicBrainz.API;
using Hqub.MusicBrainz.API.Entities;
using System.Threading.Tasks;
using System.Windows;

namespace AverageLyrics
{
    public class MusicBrainzLookup : Globals
    {
        public static async Task LookupArtist(string enteredName)
        {
            try
            {
                MatchingArtists.Clear();
                
                if (enteredName == "" || enteredName == DefaultArtistText)
                {
                    MessageBox.Show("Please enter an artist's name in the search box.");
                    return;
                }

                var _foundArtists = await Artist.SearchAsync("\"" + enteredName + "\"", 10);
                if (_foundArtists != null && _foundArtists.Items.Count > 0)
                {
                    foreach (var a in _foundArtists.Items)
                    {
                        var _artist = new ArtistItem
                        {
                            Score = a.Score,
                            Id = a.Id,
                            Name = a.Name,
                            Country = a.Country,
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

                //var _foundWorks = await Artist.GetAsync(artist.Id, "works");
                //foreach (var _work in _foundWorks)
                //{

                //}

                //var _artist = await Artist.GetAsync(artist.Id, "artist-rels", "url-rels");

                var _works = await Artist.GetAsync(artist.Id, "works");
                

                //var _lyrics = _artist.Relations.Where(r => r.TargetType == "url" && r.Type == "lyrics");
                //MessageBox.Show(_lyrics.Count().ToString());

                //var _work = _artist.Relations.Where(r => r.Type == "work");
                
                //MessageBox.Show(_works.Works.Count().ToString());


                //var _workQuery = new QueryParameters<Work>() { { "arid", artist.Id } };
                //var _foundWorks = await Work.GetAsync(_workQuery);

                //var _works = _artist.Relations.Where(r => r.Type == "recordings");

                int i = 0;

                foreach (var w in _works.Works)
                {
                    //MessageBox.Show(w.Title);

                    //var _work = await Work.GetAsync(w.Id, "url-rels");
                    //var _lyrics = _work.Relations.Where(r => r.Type == "lyrics");

                    i++;

                    if (i == 1) { await LyricsLookup.GetLyrics(artist.Name, w.Title); MessageBox.Show(LyricsLookup.TheseLyrics); }

                    var _song = new RecordingItem
                    {
                        Id = w.Id,
                        Title = w.Title
                        //LyricCount = _lyrics.Count()
                        //,
                        //FirstRelease = _work.Releases.OrderByDescending(r => r.Score).Select(r => r.Title).FirstOrDefault(),
                        //Seconds = _work.Length
                        //Score = s.Score
                    };
                    Globals.MatchingSongs.Add(_song);
                }

                



                //var _songQuery = new QueryParameters<Recording>() { { "arid", artist.Id } };
                //var _foundSongs = await Recording.SearchAsync(_songQuery);
                //if (_foundSongs != null && _foundSongs.Items != null && _foundSongs.Items.Count > 0)
                //{
                //    foreach (var s in _foundSongs.Items)
                //    {
                //        MessageBox.Show(s.Title);
                //        var _recording = await Recording.GetAsync(s.Id, "work-rels");
                //        if (_recording == null || _recording.Relations == null || _recording.Relations.Count == 0) { continue; }

                //        foreach (var rl in _recording.Relations)
                //        {
                //            var _work = rl.Work;
                //            //if (_work == null) { continue; }
                //            //var _lyrics = _work.Relations.Where(r => r.Type == "lyrics");
                //            //if (_lyrics != null && _lyrics.Count() > 0)
                //            //{
                //            //    var _song = new RecordingItem
                //            //    {
                //            //        Id = _work.Id,
                //            //        Title = _work.Title,
                //            //        LyricCount = _lyrics.Count()
                //            //        //,
                //            //        //FirstRelease = _work.Releases.OrderByDescending(r => r.Score).Select(r => r.Title).FirstOrDefault(),
                //            //        //Seconds = _work.Length
                //            //        //Score = s.Score
                //            //    };
                //            //    Globals.MatchingSongs.Add(_song);
                //            //}
                //        }
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("Could not find any songs for artist '" + artist.Name + "'.");
                //}
            }
            catch (Exception exp) { MessageBox.Show("Error finding songs for artist " + artist.Name + ": " + exp.Message); }
        }

    }
}
