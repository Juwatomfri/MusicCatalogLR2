using Logic;

namespace SearchInterface
{
    public class SearchOption
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public SearchOption(string whatToSearch)
        {
            switch (whatToSearch)
            {
                case "SingerSearch":
                    Name = "Поиск по артистам";
                    Value = new SingerSearchStrategy();
                    break;
                case "AlbumSearch":
                    Name = "Поиск по альбомам";
                    Value = new AlbumSearchStrategy();
                    break;
                case "TrackSearch":
                    Name = "Поиск по трекам";
                    Value = new TrackSearchStrategy();
                    break;
            }
        }
    }
}
