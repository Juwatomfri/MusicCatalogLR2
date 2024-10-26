namespace Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Track> Tracks { get; set; } = [];
        public Singer Singer { get; set; }
    }
}
