using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AverageLyrics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ArtistName.Text = Globals.DefaultArtistText;
            ArtistName.Focus();
        }

        private void ArtistName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ArtistName.Text == Globals.DefaultArtistText)
            {
                ArtistName.SelectAll();
            }
        }

        private async void ArtistSearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ArtistDataGrid.ItemsSource = null;
                await MusicBrainzLookup.LookupArtist(ArtistName.Text);                
                ArtistDataGrid.ItemsSource = Globals.MatchingArtists;
                if (Globals.MatchingArtists.Count > 0) { ArtistDataGrid.SelectedIndex = 0; }
            }
            catch (Exception exp) { MessageBox.Show("Error searching for artists: " + exp.Message); }
        }

        private void ArtistDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Globals.SelectedArtist = ArtistDataGrid.SelectedItem as ArtistItem;
        }

        private async void SongSearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Globals.SelectedArtist != null)
                {
                    SongDataGrid.ItemsSource = null;
                    await MusicBrainzLookup.LookupSongs(Globals.SelectedArtist);
                    SongDataGrid.ItemsSource = Globals.MatchingSongs;
                    SongDataGrid.SelectAll();
                    ShowAverage();
                }
                else
                {
                    MessageBox.Show("Please select an artist record in the upper table.");
                }
            }
            catch (Exception exp) { MessageBox.Show("Error searching for songs: " + exp.Message); }
        }

        private void ShowAverage()
        {
            if (SongDataGrid.SelectedItems == null)
            {
                MessageBox.Show("Please select at least one song in the lower table.");
            }
            else
            {
                Globals.SelectedSongs.Clear();
                foreach (var selectedSong in SongDataGrid.SelectedItems)
                {
                    SongItem _thisSong = selectedSong as SongItem;
                    if (_thisSong != null) { Globals.SelectedSongs.Add(_thisSong); }
                }
                double _average = Globals.LyricAverage();
                MessageBox.Show("The average number of words in the selected songs is " + _average);
            }
        }

    }
}
