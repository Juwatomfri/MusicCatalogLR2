using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class CompilationService
    {
        private readonly AppDbContext _appDbContext;
        public CompilationService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public void CreateCompilation(string name, List<Track> tracks)
        {
            var compilation = new Compilation()
            {
                Name = name,
                Tracks = tracks,
            };
            _appDbContext.Compilations.Add(compilation);
            _appDbContext.SaveChanges();
        }

        public Compilation GetCompilationById(int id)
        {
            return _appDbContext.Compilations.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentException("Элемента с таким Id нет в базе");
        }

        public List<Compilation> GetCompilationsByName(string name)
        {
            return _appDbContext.Compilations.Where(a => a.Name.ToLower().Contains(name)).ToList() ?? throw new ArgumentException("Элементов с таким имененм нет в базе");
        }
    }
}
