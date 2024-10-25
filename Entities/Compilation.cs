namespace Entities
{
    public class Compilation
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; } = new List<Track>();

        public Compilation(string name)
        {
            Name = name;
        }

        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }
    }
}
