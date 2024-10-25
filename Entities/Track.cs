namespace Entities
{
    public class Track
    {
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public TimeSpan Duration { get; set; }

        public List<Singer> Singers { get; set; }

        public Track(string name, Genre genre, TimeSpan duration, List<Singer> singers)
        {
            Name = name;
            Genre = genre;
            Duration = duration;
            Singers = singers;
        }
    }
}
