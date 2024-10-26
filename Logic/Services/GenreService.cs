using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class GenreService
    {
        private readonly AppDbContext _appDbContext;
        public GenreService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public void CreateGenre(string name)
        {
            var genre = new Genre()
            {
                Name = name,
            };
            _appDbContext.Genres.Add(genre);
            _appDbContext.SaveChanges();
        }

        public Genre GetGenreById(int id)
        {
            return _appDbContext.Genres.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentException("Элемента с таким Id нет в базе");
        }

        public List<Genre> GetGenresByName(string name)
        {
            return _appDbContext.Genres.Where(a => a.Name.Contains(name)).ToList() ?? throw new ArgumentException("Элементов с таким имененм нет в базе");
        }
    }
}
