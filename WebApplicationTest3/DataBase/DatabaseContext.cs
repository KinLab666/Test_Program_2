using System.Data.SQLite;

namespace WebApplicationTest3.DataBase
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sqlScript = File.ReadAllText("Data/Migrations/init.sql");

            using var command = new SQLiteCommand(sqlScript, connection);
            command.ExecuteNonQuery();
        }

        public SQLiteConnection GetConnection() => new SQLiteConnection(_connectionString);
    }
}
