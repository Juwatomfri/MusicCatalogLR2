namespace Entities
{
    public class Compilation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Track> Tracks { get; set; } = new List<Track>();

    }
}
