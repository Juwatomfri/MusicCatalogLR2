﻿using Entities;
using Logic.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class SingerService:ISearchStrategy
    {
        private readonly AppDbContext _appDbContext;
        public SingerService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public void CreateSinger(string name, Genre genre)
        {
            var singerBuilder = new SingerBuilder(name, genre);
            //tracks.ForEach(track => singerBuilder.AddTrack(track));
            //albums.ForEach(album => singerBuilder.AddAlbum(album));

            _appDbContext.Singers.Add(singerBuilder.Build());
            _appDbContext.SaveChanges();
        }

        public Singer GetSingerById(int id)
        {
            return _appDbContext.Singers.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentException("Элемента с таким Id нет в базе");
        }

        public List<Singer> GetASingerByName(string name)
        {
            return _appDbContext.Singers.Where(a => a.Name.ToLower().Contains(name)).ToList() ?? throw new ArgumentException("Элементов с таким именем нет в базе");
        }

        public List<object> Search(Catalog catalog, string query)
        {
            return catalog.Singers
                .Where(a => a.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Cast<object>().ToList();
        }
    }
}
