using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            int _initialSleep = (Globals.SleepTime / 10);
            DelaySlider.Value = _initialSleep;
            ArtistName.Focus();
        }

        private void setInitialView()
        {
            toggleSongSearchControls(false);
            toggleSongChoiceControls(false);
            WaitInstructions.Visibility = Visibility.Hidden;
        }
        
        private void toggleSongSearchControls(bool show)
        {
            Visibility _newVisibility = show ? Visibility.Visible : Visibility.Hidden;
            SecondInstructions.Visibility = ArtistDataGrid.Visibility = SongSearchButton.Visibility = _newVisibility;
            FourthInstructions.Visibility = DelayLabel.Visibility = DelaySlider.Visibility = _newVisibility;
        }

        private void toggleSongChoiceControls(bool show)
        {
            Visibility _newVisibility = show ? Visibility.Visible : Visibility.Hidden;
            SongDataGrid.Visibility = ThirdInstructions.Visibility = RecalculateButton.Visibility = _newVisibility;
            SelectGroup.Visibility = AverageLabel.Visibility = AverageWords.Visibility = _newVisibility;            
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
                MessageBox.Show("Please enter an artist's name in the search box.", "No name entered", MessageBoxButton.OK, MessageBoxImage.Stop);
                ArtistName.Focus();
                return;
            }
            else
            {
                setInitialView();
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
                    toggleSongSearchControls(true);
                }
            }
            catch (Exception exp) { MessageBox.Show("Error searching for artists: " + exp.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
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
                    toggleSongChoiceControls(true);
                    SelectAll.IsChecked = true;
                    showAverage();
                    toggleWait(false);
                }
                else
                {
                    MessageBox.Show("Please select an artist record in the upper table.", "No artist selected", MessageBoxButton.OK, MessageBoxImage.Stop);
                    ArtistDataGrid.Focus();
                }
            }
            catch (Exception exp) { MessageBox.Show("Error searching for songs: " + exp.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }      

        private void toggleWait(bool start)
        {
            SongSearchButton.IsEnabled = ArtistSearchButton.IsEnabled = DelaySlider.IsEnabled = !start;
            WaitInstructions.Visibility = start ? Visibility.Visible : Visibility.Hidden;
            toggleSongChoiceControls(!start);
        }

        private void showAverage()
        {
            if (SongDataGrid.SelectedItems == null)
            {
                MessageBox.Show("Please select at least one song in the lower table.", "No songs selected", MessageBoxButton.OK, MessageBoxImage.Stop);
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
                string _aveText = String.Format("{0:0.##}", _average);
                AverageResultBlock.Text = _aveText;
                AverageWords.Content = _aveText;
                SongDataGrid.Focus();
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
            SongDataGrid.SelectedItem = null;
            foreach (var dataLine in SongDataGrid.Items)
            {
                SongItem _thisSong = dataLine as SongItem;
                if (_thisSong != null && _thisSong.LyricCount >= 0)
                {
                    SongDataGrid.SelectedItems.Add(dataLine);
                }
            }
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

        private void DelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Globals.SleepTime = (int)DelaySlider.Value * 10;
            DelaySlider.ToolTip = Globals.SleepTime.ToString();
        }

    }
}
