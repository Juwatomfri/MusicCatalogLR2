namespace Entities
{
    public class Track
    {
        public string Name { get; set; }
        public Genre Genre { get; set; }

        public List<Singer> Singers { get; set; }

        public Track(string name, Genre genre, List<Singer> singers)
        {
            Name = name;
            Genre = genre;
            Singers = singers;
        }
    }
}
