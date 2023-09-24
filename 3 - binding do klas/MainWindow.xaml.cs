using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MediaLibrary
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<MediaItem> MediaItems { get; } = new ObservableCollection<MediaItem>();

        public MainWindow()
        {
            InitializeComponent();
            MediaListView.ItemsSource = MediaItems;
            MediaListView.SelectionChanged += MediaListView_SelectionChanged;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MediaItem newItem = new MediaItem();
            MediaItems.Add(newItem);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (MediaListView.SelectedItem != null)
            {
                EditMediaItem((MediaItem)MediaListView.SelectedItem);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MediaListView.SelectedItem != null)
            {
                MediaItems.Remove((MediaItem)MediaListView.SelectedItem);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki JSON|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                string json = System.IO.File.ReadAllText(openFileDialog.FileName);
                ObservableCollection<MediaItem> importedItems = JsonConvert.DeserializeObject<ObservableCollection<MediaItem>>(json);

                MediaItems.Clear();
                foreach (var item in importedItems)
                {
                    MediaItems.Add(item);
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki JSON|*.json";

            if (saveFileDialog.ShowDialog() == true)
            {
                string json = JsonConvert.SerializeObject(MediaItems);
                System.IO.File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private void EditMediaItem(MediaItem item)
        {
            // Obsługa edycji wybranego elementu
        }

        private void MediaListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Aktualizuj dostępność przycisków edycji i usuwania w zależności od wyboru
            IsItemSelected = MediaListView.SelectedItem != null;
        }

        public bool IsItemSelected { get; set; }
    }

    public class MediaItem
    {
        public string Title { get; set; }
        public string DirectorOrAuthor { get; set; }
        public string PublisherOrStudio { get; set; }
        public MediaType MediaType { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
    public enum MediaType
    {
        Film,
        Music
    }

}
