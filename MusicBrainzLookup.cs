using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hqub.MusicBrainz.API.Entities;
using System.Threading.Tasks;
using System.Windows;

namespace AverageLyrics
{
    class MusicBrainzLookup : Globals
    {
        public static async Task LookupArtist(string enteredName)
        {
            try
            {
                Globals.MatchingArtists.Clear();
                
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
                    MessageBox.Show("Could not find artist " + enteredName);
                }
            }
            catch (Exception exp) { MessageBox.Show("Error finding artist " + enteredName + ":" + exp.Message); }
        }

    }
}
