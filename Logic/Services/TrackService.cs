using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class TrackService:ISearchStrategy
    {
        private readonly AppDbContext _appDbContext;
        public TrackService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public void CreateTrack(string name, Genre genre, Singer singer, Album album)
        {
            var track = new Track()
            {
                Name = name,
                Genre = genre,
                Singer = singer,
                Album = album,
            };
            _appDbContext.Tracks.Add(track);
            _appDbContext.SaveChanges();
        }

        public Track GetTrackById(int id)
        {
            return _appDbContext.Tracks.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentException("Элемента с таким Id нет в базе");
        }

        public List<Track> GetTracksByName(string name)
        {
            return _appDbContext.Tracks.Where(a => a.Name.ToLower().Contains(name)).ToList() ?? throw new ArgumentException("Элементов с таким имененм нет в базе");
        }

        public List<object> Search(Catalog catalog, string query)
        {
            return catalog.Singers
                .SelectMany(al => al.Tracks)
                .Where(t => t.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Cast<object>().ToList();
        }
    }
}
