using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace DriveMonitor.Data
{
    public class SqlLogger
    {
        private readonly string _connectionString;

        public SqlLogger()
        {
            var config = ConfigHelper.GetConfiguration();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void LogMessage(string message)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                string query = @"
                    INSERT INTO dbo.StorageLogs (LogId, Message, CreatedAt, SystemName)
                    VALUES (@LogId, @Message, @CreatedAt, @SystemName)";

                using var command = new SqlCommand(query, connection);

                Guid logId = Guid.NewGuid();                          // Generate UUID
                string systemName = Environment.MachineName;          // Get local machine name
                DateTime createdAt = DateTime.Now;

                command.Parameters.AddWithValue("@LogId", logId);
                command.Parameters.AddWithValue("@Message", message);
                command.Parameters.AddWithValue("@CreatedAt", createdAt);
                command.Parameters.AddWithValue("@SystemName", systemName);

                connection.Open();
                command.ExecuteNonQuery();

                Console.WriteLine("✅ Log inserted into database with system name.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to log to database: {ex.Message}");
            }
        }
    }
}

