namespace Entities
{
    public class Singer
    {
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public List<Album> Albums { get; set; } = [];
        public List<Track> Tracks { get; set; } = [];

        public Singer(string name, Genre genre)
        {
            Name = name;
            Genre = genre;
        }

        //public void AddAlbum(Album album)
        //{
        //    Albums.Add(album);
        //}
        //public void AddTrack(Track track)
        //{
        //    Tracks.Add(track);
        //}
    }
}
