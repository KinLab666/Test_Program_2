using System.Data.SQLite;
using WebApplicationTest3.DataBase;
using WebApplicationTest3.Models;
using WebApplicationTest3.Models.RequestModels;

namespace WebApplicationTest3.Services
{
    public class TagService
    {
        private readonly DatabaseContext _db;

        public TagService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            var tags = new List<Tag>();
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("SELECT * FROM Tags", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                tags.Add(new Tag
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Title"].ToString()
                });
            }

            return tags;
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("SELECT * FROM Tags WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Tag
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Title"].ToString()
                };
            }

            return null;
        }

        public async Task<int> CreateTagAsync(CreateTagRequest request)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("INSERT INTO Tags (Name) VALUES (@name); SELECT last_insert_rowid();", conn);
            cmd.Parameters.AddWithValue("@name", request.Name);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<bool> UpdateTagAsync(int id, UpdateTagRequest request)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("UPDATE Tags SET Name = @name WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@name", request.Name);
            cmd.Parameters.AddWithValue("@id", id);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("DELETE FROM Tags WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        // Вспомогательный метод
        private Tag MapRowToTag(SQLiteDataReader reader) => new()
        {
            Id = Convert.ToInt32(reader["Id"]),
            Name = reader["Name"].ToString()
        };
    }
}
