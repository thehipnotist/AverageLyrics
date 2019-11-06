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
        bool radioSelected = false;
        
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
            SongDataGrid.Visibility = ThirdInstructions.Visibility = RecalculateButton.Visibility = SelectGroup.Visibility = show ? Visibility.Visible : Visibility.Hidden;            
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
                    SelectAll.IsChecked = true;
                    showAverage();
                    toggleWait(false);
                }
                else
                {
                    MessageBox.Show("Please select an artist record in the upper table.");
                    ArtistDataGrid.Focus();
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
                AverageResultBlock.Text = String.Format("{0:0.##}", _average);
            }
        }

        private void RecalculateButton_Click(object sender, RoutedEventArgs e)
        {
            showAverage();
        }

        private void toggleRadioSelection(bool selected)
        {
            radioSelected = selected;
            if (selected) { SongDataGrid.Focus(); }
        }

        private void SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            toggleRadioSelection(false);
            SongDataGrid.SelectAll();
            toggleRadioSelection(true);
        }

        private void SelectPositive_Checked(object sender, RoutedEventArgs e)
        {
            toggleRadioSelection(false);
            SongDataGrid.SelectedItem = null;
            foreach (var dataLine in SongDataGrid.Items)
            {
                SongItem _thisSong = dataLine as SongItem;
                if (_thisSong != null && _thisSong.LyricCount > 0) 
                { 
                    SongDataGrid.SelectedItems.Add(dataLine); 
                } 
            }
            toggleRadioSelection(true);
        }

        private void SelectNone_Checked(object sender, RoutedEventArgs e)
        {
            toggleRadioSelection(false);
            SongDataGrid.SelectedItem = null;
            toggleRadioSelection(true);
        }

        private void SongDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (radioSelected)
            {
                SelectAll.IsChecked = SelectNone.IsChecked = SelectPositive.IsChecked = false;
                radioSelected = false;
            }
        }

    }
}
