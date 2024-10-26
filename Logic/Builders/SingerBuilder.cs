using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Builders
{
    public class SingerBuilder
    {
        private readonly Singer _singer;

        public SingerBuilder(string name, Genre genre)
        {
            _singer = new Singer()
            {
                Name = name,
                Genre = genre,
            };
        }

        public SingerBuilder AddTrack(Track track)
        {
            _singer.Tracks.Add(track);
            return this;
        }
        
        public SingerBuilder AddAlbum(Album album)
        {
            _singer.Albums.Add(album);
            return this;
        }

        public Singer Build() => _singer;
    }
}
