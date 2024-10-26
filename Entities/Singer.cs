namespace Entities
{
    public class Singer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public List<Album> Albums { get; set; } = [];
        public List<Track> Tracks { get; set; } = [];
    }
}
