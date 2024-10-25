using Entities;
using Logic;
using Logic.Builders;
using SearchInterface;
using System;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicCatalogLR2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Catalog _catalog;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCatalog();
        }

        private void InitializeCatalog()
        {
            // Инициализация каталога и добавление тестовых данных
            _catalog = new Catalog();

            Genre rock = new Genre("Rock");
            Genre pop = new Genre("Pop");

            var artistBuilder = new SingerBuilder("Artist1", rock);
            var albumBuilder = new AlbumBuilder("Album1", new List<Singer>() { artistBuilder.Build() })
                .AddSong(new Entities.Track("Song1", rock, new List<Singer>() { artistBuilder.Build() }))
                .AddSong(new Entities.Track("Song2", rock, new List<Singer>() { artistBuilder.Build() }));
            var artist = artistBuilder.AddAlbum(albumBuilder.Build()).Build();

            _catalog.Singers.Add(artist);
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;
            if (string.IsNullOrWhiteSpace(query)) return;

            ISearchStrategy strategy = GetSearchStrategy();
            if (strategy == null) return;

            var results = _catalog.Search(query, strategy);
            ResultsList.ItemsSource = results;
        }

        private ISearchStrategy GetSearchStrategy()
        {
            switch (SearchTypeSelector.SelectedIndex)
            {
                case 0: return new SingerSearchStrategy();
                case 1: return new AlbumSearchStrategy();
                case 2: return new TrackSearchStrategy();
                default: return null;
            }
        }
    }
}