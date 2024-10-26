using Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Logic
{
    public class Catalog
    {
        private readonly string _connectionString;

        public Catalog(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createGenresTable = @"CREATE TABLE IF NOT EXISTS Genres (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL)";

            var createSingersTable = @"CREATE TABLE IF NOT EXISTS Singers (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL,
                                    GenreId INTEGER,
                                    FOREIGN KEY(GenreId) REFERENCES Genres(Id))";

            var createAlbumsTable = @"CREATE TABLE IF NOT EXISTS Albums (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL)";

            var createTracksTable = @"CREATE TABLE IF NOT EXISTS Tracks (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL,
                                    AlbumId INTEGER,
                                    GenreId INTEGER,
                                    FOREIGN KEY(GenreId) REFERENCES Genres(Id),
                                    FOREIGN KEY(AlbumId) REFERENCES Albums(Id))";

            var createTrackSingersTable = @"CREATE TABLE IF NOT EXISTS TrackSingers (
                                       TrackId INTEGER,
                                       SingerId INTEGER,
                                       FOREIGN KEY(TrackId) REFERENCES Tracks(Id),
                                       FOREIGN KEY(SingerId) REFERENCES Singers(Id),
                                       PRIMARY KEY (TrackId, SingerId))";

            var createAlbumSingersTable = @"CREATE TABLE IF NOT EXISTS AlbumSingers (
                                       AlbumId INTEGER,
                                       SingerId INTEGER,
                                       FOREIGN KEY(AlbumId) REFERENCES Albums(Id),
                                       FOREIGN KEY(SingerId) REFERENCES Singers(Id),
                                       PRIMARY KEY (AlbumId, SingerId))";

            ExecuteNonQuery(createGenresTable, connection);
            ExecuteNonQuery(createSingersTable, connection);
            ExecuteNonQuery(createAlbumsTable, connection);
            ExecuteNonQuery(createTracksTable, connection);
            ExecuteNonQuery(createAlbumSingersTable, connection);
            ExecuteNonQuery(createTrackSingersTable, connection);
        }

        private void ExecuteNonQuery(string commandText, SqliteConnection connection)
        {
            using var command = new SqliteCommand(commandText, connection);
            command.ExecuteNonQuery();
        }

        public void AddGenre(Genre genre)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = new SqliteCommand("INSERT INTO Genres (Name) VALUES (@name)", connection);
            command.Parameters.AddWithValue("@name", genre.Name);
            command.ExecuteNonQuery();
        }

        public void AddSinger(Singer singer)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = new SqliteCommand("INSERT INTO Singers (Name, GenreId) VALUES (@name, (SELECT Id FROM Genres WHERE Name = @genreName))", connection);
            command.Parameters.AddWithValue("@name", singer.Name);
            command.Parameters.AddWithValue("@genreName", singer.Genre.Name);
            command.ExecuteNonQuery();
        }

        public void AddAlbum(Album album)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = new SqliteCommand("INSERT INTO Albums (Name) VALUES (@name)", connection);
            command.Parameters.AddWithValue("@name", album.Name);
            command.ExecuteNonQuery();

            var albumIdCommand = new SqliteCommand("SELECT Id FROM Albums WHERE Name = @albumName", connection);
            albumIdCommand.Parameters.AddWithValue("@albumName", album.Name);
            var albumId = (long)albumIdCommand.ExecuteScalar();

            foreach (var singer in album.Singers)
            {
                var singerIdCommand = new SqliteCommand("SELECT Id FROM Singers WHERE Name = @singerName", connection);
                singerIdCommand.Parameters.AddWithValue("@singerName", singer.Name);
                var singerId = (long)singerIdCommand.ExecuteScalar();

                var checkCommand = new SqliteCommand("SELECT COUNT(1) FROM AlbumSingers WHERE AlbumId = @albumId AND SingerId = @singerId", connection);
                checkCommand.Parameters.AddWithValue("@albumId", albumId);
                checkCommand.Parameters.AddWithValue("@singerId", singerId);
                var exists = (long)checkCommand.ExecuteScalar() > 0;

                if (!exists)
                {
                    var insertCommand = new SqliteCommand("INSERT INTO AlbumSingers (AlbumId, SingerId) VALUES (@albumId, @singerId)", connection);
                    insertCommand.Parameters.AddWithValue("@albumId", albumId);
                    insertCommand.Parameters.AddWithValue("@singerId", singerId);
                    insertCommand.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine($"Запись для альбома '{album.Name}' и певца '{singer.Name}' уже существует в AlbumSingers.");
                }
            }
        }

        public void AddTrack(Track track, Album album)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = new SqliteCommand("INSERT INTO Tracks (Name, AlbumId, GenreId) VALUES (@name, (SELECT Id FROM Albums WHERE Name = @albumName), (SELECT Id FROM Genres WHERE Name = @genreName))", connection);
            command.Parameters.AddWithValue("@name", track.Name);
            command.Parameters.AddWithValue("@albumName", album.Name);
            command.Parameters.AddWithValue("@genreName", track.Genre.Name);
            command.ExecuteNonQuery();

            command = new SqliteCommand("SELECT Id FROM Tracks WHERE Name = @name AND AlbumId = (SELECT Id FROM Albums WHERE Name = @albumName) ORDER BY Id DESC LIMIT 1", connection);
            command.Parameters.AddWithValue("@name", track.Name);
            command.Parameters.AddWithValue("@albumName", album.Name);
            var trackId = (long)command.ExecuteScalar();

            foreach (var singer in track.Singers)
            {
                var singerIdCommand = new SqliteCommand("SELECT Id FROM Singers WHERE Name = @singerName", connection);
                singerIdCommand.Parameters.AddWithValue("@singerName", singer.Name);
                var singerId = (long)singerIdCommand.ExecuteScalar();

                var insertCommand = new SqliteCommand("INSERT INTO TrackSingers (TrackId, SingerId) VALUES (@trackId, @singerId)", connection);
                insertCommand.Parameters.AddWithValue("@trackId", trackId);
                insertCommand.Parameters.AddWithValue("@singerId", singerId);
                insertCommand.ExecuteNonQuery();
            }
        }

        public List<Genre> GetGenres()
        {
            var genres = new List<Genre>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT DISTINCT Name FROM Genres", connection)) // Используем DISTINCT
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) // Проверяем на NULL
                            {
                                var genre = new Genre(reader.GetString(0)); // Создаем объект Genre с полученным именем
                                genres.Add(genre); // Добавляем жанр в список
                            }
                        }
                    }
                }
            }

            return genres; // Возвращаем список жанров
        }

        public List<Singer> SearchSingers(string name)
        {
            var singers = new List<Singer>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = new SqliteCommand("SELECT * FROM Singers WHERE Name LIKE @name", connection);
            command.Parameters.AddWithValue("@name", $"%{name}%");

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                singers.Add(new Singer(reader.GetString(1), new Genre(reader.GetString(2))));
            }

            return singers;
        }

        public List<Track> GetTracks()
        {
            var tracks = new List<Track>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = new SqliteCommand("SELECT * FROM Tracks", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var trackId = reader.GetInt64(0);
                var trackName = reader.GetString(1);
                long? albumId = reader.IsDBNull(2) ? (long?)null : reader.GetInt64(2); // Handle potential NULL
                long? genreId = reader.IsDBNull(3) ? (long?)null : reader.GetInt64(3); // Handle potential NULL

                // Get album name if albumId is not NULL
                string albumName = null;
                if (albumId.HasValue)
                {
                    var albumCommand = new SqliteCommand("SELECT Name FROM Albums WHERE Id = @albumId", connection);
                    albumCommand.Parameters.AddWithValue("@albumId", albumId.Value);
                    albumName = albumCommand.ExecuteScalar()?.ToString();
                }

                // Get genre name if genreId is not NULL
                string genreName = null;
                if (genreId.HasValue)
                {
                    var genreCommand = new SqliteCommand("SELECT Name FROM Genres WHERE Id = @genreId", connection);
                    genreCommand.Parameters.AddWithValue("@genreId", genreId.Value);
                    genreName = genreCommand.ExecuteScalar()?.ToString();
                }

                var singers = new List<Singer>();
                var singerCommand = new SqliteCommand("SELECT s.Name FROM TrackSingers AS ts INNER JOIN Singers AS s ON ts.SingerId = s.Id WHERE ts.TrackId = @trackId", connection);
                singerCommand.Parameters.AddWithValue("@trackId", trackId);
                using (var singerReader = singerCommand.ExecuteReader())
                {
                    while (singerReader.Read())
                    {
                        var singerName = singerReader.GetString(0);
                        singers.Add(new Singer(singerName, new Genre(genreName)));
                    }
                }

                // Create a Track instance, using null checks for album and genre names
                var track = new Track(trackName, new Genre(genreName ?? "Unknown"), new Album(albumName ?? "Unknown", singers), singers);
                tracks.Add(track);
            }

            return tracks;
        }

    }
}
