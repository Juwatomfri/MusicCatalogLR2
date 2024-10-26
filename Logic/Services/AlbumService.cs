using Entities;
using Logic.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class AlbumService:ISearchStrategy
    {
        private readonly AppDbContext _appDbContext;
        public AlbumService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public void CreateAlbum(string name, Singer singer)
        {
            var albumBuilder = new AlbumBuilder(name, singer);
            //tracks.ForEach(track => albumBuilder.AddTrack(track));
            _appDbContext.Albums.Add(albumBuilder.Build());
            _appDbContext.SaveChanges();
        }

        public Album GetAlbumById(int id)
        {
            return _appDbContext.Albums.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentException("Элемента с таким Id нет в базе"); 
        }

        public List<Album> GetAlbumsByName(string name)
        {
            return _appDbContext.Albums.Where(a => a.Name.ToLower().Contains(name)).ToList() ?? throw new ArgumentException("Элементов с таким имененм нет в базе"); 
        }

        public List<object> Search(Catalog catalog, string query)
        {
            return catalog.Singers
                .SelectMany(a => a.Albums)
                .Where(al => al.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Cast<object>().ToList();
        }
    }
}
