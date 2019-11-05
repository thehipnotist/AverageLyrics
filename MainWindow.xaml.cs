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
        
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchArtistName = ArtistName.Text;
                await MusicBrainzLookup.LookupArtist(searchArtistName);
                ArtistDataGrid.ItemsSource = null;
                ArtistDataGrid.ItemsSource = Globals.MatchingArtists;
            }
            catch (Exception exp) { MessageBox.Show("Error searching for artists: " + exp.Message); }
        }

        private void ArtistName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ArtistName.Text == Globals.DefaultArtistText)
            {
                ArtistName.SelectAll();
            }
        }
    }
}
