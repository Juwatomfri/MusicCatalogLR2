using Entities;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MusicCatalogLR2
{
    public partial class MainWindow : Window
    {
        private readonly Catalog _catalog;

        public MainWindow()
        {
            InitializeComponent();
            _catalog = InitializeCatalog();
        }

        private Catalog InitializeCatalog()
        {
            var connectionString = "Data Source=musiccatalog.db";
            var catalog = new Catalog(connectionString);
            AddTestData(catalog);
            LoadGenres(catalog);
            return catalog;
        }

        private void LoadGenres(Catalog catalog)
        {
            var genres = catalog.GetGenres(); // Получаем жанры из базы данных
            ChoseAGenre.Items.Clear(); // Очищаем существующие элементы

            foreach (var genre in genres)
            {
                ChoseAGenre.Items.Add(genre.Name); // Добавляем каждый жанр в ComboBox
            }

            // Устанавливаем первый элемент выбранным, если жанры есть
            if (ChoseAGenre.Items.Count > 0)
            {
                ChoseAGenre.SelectedIndex = 0; // Устанавливаем первый элемент выбранным
            }
        }

        private void AddTestData(Catalog catalog) {

        private void AddTestData(Catalog catalog)
        {
            // Add genres
            var genre = new Genre("Rock");
            var genre2 = new Genre("Pop");
            catalog.AddGenre(genre);
            catalog.AddGenre(genre2);

            // Add artists
            var artist1 = new Singer("Queen", genre);
            var artist2 = new Singer("Freddie Mercury", genre);
            catalog.AddSinger(artist1);
            catalog.AddSinger(artist2);

            // Add album with artist
            var album = new Album("A Night at the Opera", new List<Singer> { artist1 });
            catalog.AddAlbum(album);

            // Add track with multiple artists
            var track = new Track("Bohemian Rhapsody", genre, album, new List<Singer> { artist1, artist2 });
            catalog.AddTrack(track, album);

            // Log tracks to the console
            LogTracks(catalog.GetTracks());
        }

        private void LogTracks(IEnumerable<Track> tracks)
        {
            foreach (var track in tracks)
            {
                Console.WriteLine($"Track: {track.Name}, Artists: {string.Join(", ", track.Singers.Select(s => s.Name))}");
            }
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            ResultsList.Items.Clear(); // Очищаем предыдущие результаты
            string query = SearchBox.Text;

            if (string.IsNullOrWhiteSpace(query)) return;

            var results = new List<string>();

            string Type = SearchTypeSelector.Text;

            if (Type == "Артисты")
            {
                // Поиск исполнителей
                var singers = _catalog.SearchSingers(query)
                    .Where(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Distinct() // Убираем дубликаты
                    .ToList();

                results.AddRange(singers.Select(s => $"Исполнитель: {s.Name}"));
            }
            else if (Type == "Альбомы")
            {
                // Поиск альбомов
                var albums = _catalog.GetTracks()
                    .Select(t => t.Album) // Получаем альбомы из треков
                    .Where(a => a.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Distinct() // Убираем дубликаты
                    .ToList();

                results.AddRange(albums.Select(a => $"Альбом: {a.Name}"));
            }
            else if (Type == "Песни")
            {
                // Поиск треков
                var tracks = _catalog.GetTracks()
                    .Where(t => t.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Distinct() // Убираем дубликаты
                    .ToList();

                foreach (var track in tracks)
                {
                    var singers = string.Join(", ", track.Singers.Select(s => s.Name));
                    if (string.IsNullOrWhiteSpace(singers))
                    {
                        results.Add($"Песня: {track.Name}, Исполнитель: Неизвестен");
                    }
                    else
                    {
                        results.Add($"Песня: {track.Name}, Исполнитель: {singers}");
                    }
                }
            }

            // Проверяем наличие результатов
            if (!results.Any())
            {
                ResultsList.Items.Add("По данному запросу ничего не найдено.");
            }
            else
            {
                foreach (var result in results.Distinct()) // Убираем дубликаты при выводе
                {
                    ResultsList.Items.Add(result);
                }
            }
        }



    }
}
