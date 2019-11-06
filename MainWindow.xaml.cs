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
            populateTypeCombo();
            setInitialView();
            ArtistName.Focus();
        }

        private void setInitialView()
        {
            toggleArtistControls(false);
            toggleSongControls(false);
            WaitInstructions.Visibility = Visibility.Hidden;
        }
        
        private void toggleArtistControls(bool show)
        {
            SecondInstructions.Visibility = ArtistDataGrid.Visibility = SongSearchButton.Visibility = show ? Visibility.Visible : Visibility.Hidden;
        }

        private void toggleSongControls(bool show)
        {
            SongDataGrid.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            ThirdInstructions.Visibility = show ? Visibility.Visible : Visibility.Hidden;
        }

        private void populateTypeCombo()
        {
            MusicBrainzLookup.SetArtistTypes();
            TypeCombo.ItemsSource = Globals.ArtistTypes;
            TypeCombo.SelectedValue = Globals.AllRecords;
        }

        private void ArtistName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ArtistName.Text == Globals.DefaultArtistText) { ArtistName.SelectAll(); }
        }

        private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeCombo.SelectedItem == null) { Globals.SelectedType = Globals.AllRecords; }
            else { Globals.SelectedType = TypeCombo.SelectedItem as string; }
        }

        private void ArtistSearchButton_Click(object sender, RoutedEventArgs e)
        {

            if (ArtistName.Text == "" || ArtistName.Text == Globals.DefaultArtistText)
            {
                MessageBox.Show("Please enter an artist's name in the search box.");
                ArtistName.Focus();
                return;
            }
            else
            {
                findArtist();
            }
        }

        private async void findArtist()
        {
            try
            {
                ArtistDataGrid.ItemsSource = null;
                await MusicBrainzLookup.LookupArtist(ArtistName.Text, Globals.SelectedType);
                if (Globals.MatchingArtists.Count() > 0)
                {
                    ArtistDataGrid.ItemsSource = Globals.MatchingArtists;
                    if (Globals.MatchingArtists.Count > 0) { ArtistDataGrid.SelectedIndex = 0; }
                    toggleArtistControls(true);
                }
            }
            catch (Exception exp) { MessageBox.Show("Error searching for artists: " + exp.Message); }
        }

        private void ArtistDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Globals.SelectedArtist = ArtistDataGrid.SelectedItem as ArtistItem;
        }

        private void SongSearchButton_Click(object sender, RoutedEventArgs e)
        {
            findSongs();
        }

        private async void findSongs()
        {
            try
            {
                if (Globals.SelectedArtist != null)
                {
                    toggleWait(true);
                    SongDataGrid.ItemsSource = null;
                    await MusicBrainzLookup.LookupSongs(Globals.SelectedArtist);
                    SongDataGrid.ItemsSource = Globals.MatchingSongs;
                    toggleSongControls(true);
                    SongDataGrid.SelectAll();
                    showAverage();
                    toggleWait(false);
                }
                else
                {
                    MessageBox.Show("Please select an artist record in the upper table.");
                }
            }
            catch (Exception exp) { MessageBox.Show("Error searching for songs: " + exp.Message); }
        }      

        private void toggleWait(bool start)
        {
            SongSearchButton.IsEnabled = !start;
            WaitInstructions.Visibility = start ? Visibility.Visible : Visibility.Hidden;
            toggleSongControls(!start);
        }

        private void showAverage()
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
                //MessageBox.Show("The average number of words in the selected songs is " + _average);
                AverageResultBlock.Text = "The average number of words in the selected songs is " + _average + ".";
            }
        }

    }
}
