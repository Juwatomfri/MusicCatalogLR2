namespace Entities
{
    public class Album
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; } = [];
        public List<Singer> Singers { get; set; }

        public Album(string name, List<Singer> singers)
        {
            Name = name;
            Singers = singers;
        }
    }
}
