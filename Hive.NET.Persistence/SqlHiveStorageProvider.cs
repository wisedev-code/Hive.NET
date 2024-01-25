using System.Data.SqlClient;
using System.Text.Json;
using Hive.NET.Core.Configuration.Storage;

namespace Hive.NET.Persistence;

 public class SqlHiveStorageProvider : IHiveStorageProvider
    {
        private readonly string _connectionString;

        public SqlHiveStorageProvider(string connectionString)
        {
            _connectionString = connectionString;
            EnsureDatabaseCreated();
        }

        public void UpsertHive(Guid id, Core.Components.Hive? hive)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(@"
                MERGE INTO Hives AS target
                USING (VALUES (@Id, @HiveData)) AS source (Id, HiveData)
                ON target.Id = source.Id
                WHEN MATCHED THEN
                    UPDATE SET target.HiveData = source.HiveData
                WHEN NOT MATCHED THEN
                    INSERT (Id, HiveData) VALUES (source.Id, source.HiveData);", connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@HiveData", SerializeHive(hive));
            command.ExecuteNonQuery();
        }

        public Core.Components.Hive GetHive(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT HiveData FROM Hives WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var hiveData = reader["HiveData"].ToString();
                return DeserializeHive(hiveData);
            }

            return null;
        }

        public List<Core.Components.Hive> GetAllHives()
        {
            var hives = new List<Core.Components.Hive>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT HiveData FROM Hives", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var hiveData = reader["HiveData"].ToString();
                hives.Add(DeserializeHive(hiveData!));
            }

            return hives!;
        }

        // Other methods as needed

        private string SerializeHive(Core.Components.Hive hive)
        {
            return JsonSerializer.Serialize(hive);
        }

        private Core.Components.Hive DeserializeHive(string hiveData)
        {
            return JsonSerializer.Deserialize<Core.Components.Hive>(hiveData)!;
        }

        private void EnsureDatabaseCreated()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Hives')
                BEGIN
                    CREATE TABLE Hives
                    (
                        Id UNIQUEIDENTIFIER PRIMARY KEY,
                        HiveData NVARCHAR(MAX)
                    )
                END", connection);

            command.ExecuteNonQuery();
        }
    }