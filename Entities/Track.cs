namespace Entities
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public Singer Singer { get; set; }
        public Album Album { get; set; }
    }
}
