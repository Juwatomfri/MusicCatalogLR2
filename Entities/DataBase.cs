//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//namespace Entities
//{
//    public class DataBase : DbContext
//    {
//        public DbSet<Singer> Singers { get; set; }
//        public DbSet<Album> Albums { get; set; }
//        public DbSet<Track> Tracks { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlite("Data Source=MusicCatalog.db");
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            // Настройка отношений между сущностями
//            modelBuilder.Entity<Artist>()
//                .HasMany(a => a.Albums)
//                .WithOne(a => a.Artist)
//                .HasForeignKey(a => a.ArtistId);

//            modelBuilder.Entity<Album>()
//                .HasMany(a => a.Songs)
//                .WithOne(s => s.Album)
//                .HasForeignKey(s => s.AlbumId);
//        }
//    }
//}
