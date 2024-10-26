namespace Entities
{
    public class Singer
    {
        public string Name { get; set; }
        public Genre Genre { get; set; }

        public Singer(string name, Genre genre)
        {
            Name = name;
            Genre = genre;
        }
    }
}
