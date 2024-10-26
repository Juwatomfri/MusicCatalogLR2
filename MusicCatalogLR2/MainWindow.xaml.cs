using Entities;
using Logic;
using Logic.Builders;
using Logic.Services;
//using SearchInterface;
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
        private static AppDbContext _appDbContext = new AppDbContext();
        private SingerService _singerService = new SingerService(_appDbContext);
        private AlbumService _albumService = new AlbumService(_appDbContext);
        private GenreService _genreService = new GenreService(_appDbContext);
        private TrackService _trackService = new TrackService(_appDbContext);
        private CompilationService _compilationService = new CompilationService(_appDbContext);

        public MainWindow()
        {
            InitializeComponent();
            InitializeCatalog();
        }

        private void InitializeCatalog()
        {
            // Инициализация каталога и добавление тестовых данных
            _catalog = new Catalog();


            List<string> genres = ["Rock", "Pop", "Country"];
            genres.ForEach(_genreService.CreateGenre);
            _singerService.CreateSinger("Deep Durple", _genreService.GetGenresByName("rock")[0], [], []);


        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(query)) return;

            switch (SearchTypeSelector.Text)
            {
                case "Исполнители":
                    {
                        var results = _singerService.GetASingerByName(query);
                        foreach (Singer singer in results)
                        {
                            ResultsList.Items.Add(singer.Name);
                        }
                    }
                    break;
                case "Альбомы":
                    {
                        var results = _albumService.GetAlbumsByName(query);
                        foreach (Album album in results)
                        {
                            ResultsList.Items.Add(album.Name);
                        }
                    }
                    break;
                case "Треки":
                    {
                        var results = _trackService.GetTracksByName(query);
                        foreach (Entities.Track track in results)
                        {
                            ResultsList.Items.Add(track.Name);
                        }
                    }
                    break;
            }

            //ISearchStrategy strategy = GetSearchStrategy();
            //if (strategy == null) return;

            //var results = _catalog.Search(query, strategy);
            
        }

        private ISearchStrategy GetSearchStrategy()
        {
            switch (SearchTypeSelector.SelectedIndex)
            {
                case 0: return _singerService;
                case 1: return _albumService;
                case 2: return _trackService;
                default: return null;
            }
        }
    }
}