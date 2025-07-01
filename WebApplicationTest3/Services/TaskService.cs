using System.Data.SQLite;
using WebApplicationTest3.DataBase;
using WebApplicationTest3.Models.RequestModels;
using WebApplicationTest3.Models;

namespace WebApplicationTest3.Services
{
    public class TaskService
    {
        private readonly DatabaseContext _db;

        public TaskService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Models.Task>> GetAllTasksAsync()
        {
            var tasks = new List<Models.Task>();
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("SELECT * FROM Tasks", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                tasks.Add(new Models.Task
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Deadline = reader["Deadline"].ToString(),
                    Completed = Convert.ToBoolean(reader["Completed"])
                });
            }

            foreach (var task in tasks)
            {
                task.Tags = await GetTagsForTaskAsync(conn, task.Id);
            }

            return tasks;
        }

        public async Task<Models.Task> GetTaskByIdAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("SELECT * FROM Tasks WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Models.Task
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Deadline = reader["Deadline"].ToString(),
                    Completed = Convert.ToBoolean(reader["Completed"])
                };
            }

            return null;
        }

        public async Task<int> CreateTaskAsync(CreateTaskRequest request)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand(
                "INSERT INTO Tasks (Title, Description, Deadline, Completed) VALUES (@title, @desc, @deadline, 0); SELECT last_insert_rowid();",
                conn);

            cmd.Parameters.AddWithValue("@title", request.Title);
            cmd.Parameters.AddWithValue("@desc", request.Description ?? "");
            cmd.Parameters.AddWithValue("@deadline", request.Deadline ?? "");

            var taskId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

            if (request.TagIds != null && request.TagIds.Count > 0)
            {
                foreach (var tagId in request.TagIds)
                {
                    using var tagCmd = new SQLiteCommand(
                        "INSERT INTO TaskTags (TaskId, TagId) VALUES (@taskId, @tagId)", conn);
                    tagCmd.Parameters.AddWithValue("@taskId", taskId);
                    tagCmd.Parameters.AddWithValue("@tagId", tagId);
                    await tagCmd.ExecuteNonQueryAsync();
                }
            }

            return taskId;
        }

        public async Task<bool> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            var setClauses = new List<string>();
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(request.Title))
            {
                setClauses.Add("Title = @title");
                parameters.Add("@title", request.Title);
            }

            if (request.Description != null)
            {
                setClauses.Add("Description = @desc");
                parameters.Add("@desc", request.Description);
            }

            if (request.Deadline != null)
            {
                setClauses.Add("Deadline = @deadline");
                parameters.Add("@deadline", request.Deadline);
            }

            if (request.Completed.HasValue)
            {
                setClauses.Add("Completed = @completed");
                parameters.Add("@completed", request.Completed.Value ? 1 : 0);
            }

            if (setClauses.Count == 0 && (request.TagIds == null || request.TagIds.Count == 0))
            {
                return true;
            }

            if (setClauses.Count > 0)
            {
                var query = $"UPDATE Tasks SET {string.Join(", ", setClauses)} WHERE Id = @id";
                using var cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected == 0) return false;
            }

            if (request.TagIds != null)
            {
                using var deleteCmd = new SQLiteCommand("DELETE FROM TaskTags WHERE TaskId = @taskId", conn);
                deleteCmd.Parameters.AddWithValue("@taskId", id);
                await deleteCmd.ExecuteNonQueryAsync();

                foreach (var tagId in request.TagIds)
                {
                    using var insertCmd = new SQLiteCommand(
                        "INSERT INTO TaskTags (TaskId, TagId) VALUES (@taskId, @tagId)", conn);
                    insertCmd.Parameters.AddWithValue("@taskId", id);
                    insertCmd.Parameters.AddWithValue("@tagId", tagId);
                    await insertCmd.ExecuteNonQueryAsync();
                }
            }

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            using var conn = _db.GetConnection();
            await conn.OpenAsync();

            using var cmd = new SQLiteCommand("DELETE FROM Tasks WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private async Task<List<Tag>> GetTagsForTaskAsync(SQLiteConnection conn, int taskId)
        {
            var tags = new List<Tag>();
            using var cmd = new SQLiteCommand(
                "SELECT t.* FROM Tags t INNER JOIN TaskTags tt ON t.Id = tt.TagId WHERE tt.TaskId = @taskId", conn);
            cmd.Parameters.AddWithValue("@taskId", taskId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tags.Add(new Tag
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                });
            }

            return tags;
        }
    }
}
