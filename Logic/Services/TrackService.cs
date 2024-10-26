using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class Trackervice
    {
        private readonly AppDbContext _appDbContext;
        public Trackervice(AppDbContext context)
        {
            _appDbContext = context;
        }

        public void CreateTrack(string name, Genre genre, Singer singer)
        {
            var track = new Track()
            {
                Name = name,
                Genre = genre,
                Singer = singer,
            };
            _appDbContext.Singers.Add(singer);
            _appDbContext.SaveChanges();
        }

        public Track GetTrackById(int id)
        {
            return _appDbContext.Tracks.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentException("Элемента с таким Id нет в базе");
        }

        public List<Track> GetTracksByName(string name)
        {
            return _appDbContext.Tracks.Where(a => a.Name.Contains(name)).ToList() ?? throw new ArgumentException("Элементов с таким имененм нет в базе");
        }
    }
}
