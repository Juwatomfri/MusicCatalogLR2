using Entities;
using Logic;
using Logic.Builders;
using SearchInterface;
using System;
using System.Collections.ObjectModel;
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

            SingerBuilder singerBuilder = new SingerBuilder("Artist1", rock);
            AlbumBuilder albumBuilder = new AlbumBuilder("Album1", new List<Singer>() { singerBuilder.Build() });
            Singer singer = singerBuilder.AddAlbum(albumBuilder.Build()).Build();
            
            _catalog.Singers.Add(singer);

            SingerBuilder singerBuilder2 = new SingerBuilder("Artist2", rock);
            AlbumBuilder albumBuilder2 = new AlbumBuilder("Album2", new List<Singer>() { singerBuilder2.Build() });
            Singer singer2 = singerBuilder2.AddAlbum(albumBuilder.Build()).Build();

            _catalog.Singers.Add(singer2);
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            ResultsList.Items.Clear();
            string query = SearchBox.Text;
            if (string.IsNullOrWhiteSpace(query)) return;

            ISearchStrategy strategy = GetSearchStrategy();
            if (strategy == null) return;

            var results = _catalog.Search(query, strategy);
            if (results.Count == 0) 
            {
                ResultsList.Items.Add("По данному запросу ничего не найдено");
            } else
            {
                foreach (dynamic result in results)
                {
                    try
                    {
                        ResultsList.Items.Add(result.Name);
                    }
                    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                    {
                        // Такого не может быть, но иначе C# выдаёт ошибку
                    }
                }
            }
            
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