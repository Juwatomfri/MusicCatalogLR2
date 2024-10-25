using Entities;
using Logic;
using System.Reflection;
using System.Security.AccessControl;
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

namespace MusicCatalogLR2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Catalog catalog;

        public MainWindow()
        {
            InitializeComponent();
            catalog = new Catalog();  // Инициализация каталога
        }

        private void btnSearchArtist_Click(object sender, RoutedEventArgs e)
        {
            string singerName = txtArtistSearch.Text;  // Получаем текст из TextBox
            var singers = catalog.Search(singerName, new SingerSearchStrategy());  // Выполняем поиск
            if (singers.Count == 0)
            {
                txtResult.Text = $"Ничего не найдено";
            }
            else
            {
                foreach (Singer singer in singers)
                {
                    txtResult.Text = $"Найден артист: {singer.Name}";  // Выводим результат в TextBlock
                }
            }
        }
    }
}