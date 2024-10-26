using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Builders
{
    public class AlbumBuilder
    {
        private readonly Album _album;

        public AlbumBuilder(string name, Singer singer)
        {
            _album = new Album()
            {
                Name = name,
                Singer = singer,
            };
        }

        public AlbumBuilder AddTrack(Track track)
        {
            _album.Tracks.Add(track);
            return this;
        }

        public Album Build() => _album;
    }
}
