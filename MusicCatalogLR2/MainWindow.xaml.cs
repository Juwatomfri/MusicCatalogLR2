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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            _singerService.CreateSinger("Deep Durple", _genreService.GetGenresByName("rock")[0]);

            List<string> values = ["Исполнитель", "Альбом", "Трек", "Жанр"];

            AddTypeSelector.ItemsSource = values;

            GenreSelector.ItemsSource = _appDbContext.Genres.ToList().Select(genre => genre.Name);
            SingerSelector.ItemsSource = _appDbContext.Singers.ToList().Select(song => song.Name);
        }

        public void SingerChanged(object sender, SelectionChangedEventArgs e)
        {
            AlbumSelector.ItemsSource = _appDbContext.Albums.ToList().Where(a => a.Singer.Name == SingerSelector.SelectedValue).Select(a => a.Name);
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            ResultsList.Items.Clear();
            string query = SearchBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(query)) return;

            switch (SearchTypeSelector.Text)
            {
                case "Исполнители":
                    {
                        var results = _singerService.GetASingerByName(query);
                        foreach (Singer singer in results)
                        {
                            ResultsList.Items.Add($"Исполнитель {singer.Name}");
                        }
                    }
                    break;
                case "Альбомы":
                    {
                        var results = _albumService.GetAlbumsByName(query);
                        foreach (Album album in results)
                        {
                            ResultsList.Items.Add($"Альбом {album.Name} исполнителя {album.Singer.Name}");
                        }
                    }
                    break;
                case "Треки":
                    {
                        var results = _trackService.GetTracksByName(query);
                        foreach (Entities.Track track in results)
                        {
                            ResultsList.Items.Add($"Трек {track.Name} из альбома {track.Album.Name} исполнителя {track.Singer.Name}");
                        }
                    }
                    break;
            }          
        }


        private void ChoseAddtype(object sender, SelectionChangedEventArgs e)
        {
            Button addButton = new Button
            {
                Name = "AddButtonName",
                Content = "Добавить",
                Width = 100,
                Height = 30,
                Background = new SolidColorBrush(Colors.LightBlue),
                Foreground = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(5)
            };
            // Получаем выбранный элемент
            var selectedType = AddTypeSelector.SelectedItem.ToString();


            if (selectedType is not "Жанр")
            {
                GenreSelector.Visibility = Visibility.Visible;
                GenreSelectorLabel.Visibility = Visibility.Visible;
                SingerSelector.Visibility = Visibility.Visible;
                SingerSelectorLabel.Visibility = Visibility.Visible;
                AlbumSelector.Visibility = Visibility.Visible;
                AlbumSelectorLabel.Visibility = Visibility.Visible;
                if (selectedType is "Исполнитель")
                {
                    SingerSelector.Visibility = Visibility.Collapsed;
                    SingerSelectorLabel.Visibility = Visibility.Collapsed;
                    AlbumSelector.Visibility = Visibility.Collapsed;
                    AlbumSelectorLabel.Visibility = Visibility.Collapsed;
                }
                if (selectedType is "Альбом")
                {
                    GenreSelector.Visibility = Visibility.Collapsed;
                    GenreSelectorLabel.Visibility = Visibility.Collapsed;
                    AlbumSelector.Visibility = Visibility.Collapsed;
                    AlbumSelectorLabel.Visibility = Visibility.Collapsed;
                }
            }
            else if (selectedType is "Жанр")
            {
                GenreSelector.Visibility = Visibility.Collapsed;
                GenreSelectorLabel.Visibility = Visibility.Collapsed;

                SingerSelector.Visibility = Visibility.Collapsed;
                SingerSelectorLabel.Visibility = Visibility.Collapsed;
                AlbumSelector.Visibility = Visibility.Collapsed;
                AlbumSelectorLabel.Visibility = Visibility.Collapsed;
            }
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Message.Text = "";
            var selectedType = AddTypeSelector.SelectedItem.ToString();
            if (FutureName.Text == null) return;
            if (selectedType is "Жанр")
            {
                _genreService.CreateGenre(FutureName.Text);
                Message.Text = $"Вы успешно добавили жанр \"{FutureName.Text}\"";
                //Обновляю списки UI
                GenreSelector.ItemsSource = _appDbContext.Genres.ToList().Select(genre => genre.Name);
            }
            else if (selectedType is "Альбом")
            {
                //Обязательно приводить к toLower
                Singer singer = _singerService.GetASingerByName(SingerSelector.Text.ToLower())[0];

                _albumService.CreateAlbum(FutureName.Text, singer);
                Message.Text = $"Вы успешно добавили альбом \"{FutureName.Text}\" \n исполнителю \"{singer.Name}\"";
                //Обновляю списки UI
                AlbumSelector.ItemsSource = _appDbContext.Albums.ToList().Select(album => album.Name);
            }
            else if (selectedType is "Исполнитель")
            {
                string name = FutureName.Text;
                Genre genre = _genreService.GetGenresByName(GenreSelector.Text.ToLower())[0];
                _singerService.CreateSinger(name, genre);
                Message.Text = $"Вы успешно добавили исполнителя \"{FutureName.Text}\" \n в жанре \"{genre.Name}\"";
                //Обновляю списки UI
                SingerSelector.ItemsSource = _appDbContext.Singers.ToList().Select(song => song.Name);
            }
            else if (selectedType is "Трек")
            {
                string name = FutureName.Text;
                Genre genre = _genreService.GetGenresByName(GenreSelector.Text.ToLower())[0];
                Singer singer = _singerService.GetASingerByName(SingerSelector.Text.ToLower())[0];

                Album album = _albumService.GetAlbumsByName(AlbumSelector.Text.ToLower())[0];
                _trackService.CreateTrack(name, genre, singer, album);
                Message.Text = $"Вы успешно добавили трек {FutureName.Text} \n в жанре {genre.Name} \n исполнителю {singer.Name} \n в альбом {album.Name} ";
            }
        }
    }
}