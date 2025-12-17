using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace schoffen.CodingTracker.Database;

public class DatabaseContext
{
    private readonly string? _connectionString = DatabaseConfiguration.GetConnectionString();
    
    public DatabaseContext()
    {
        Initialize();
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
    
    private void Initialize()
    {
        using var connection = CreateConnection();
        connection.Open();

        const string createTableSql = """
                                                 CREATE TABLE IF NOT EXISTS CodingSessions (
                                                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                     StartTime TEXT,
                                                     EndTime TEXT,
                                                     Duration INTEGER
                                                 );
                                                """;

        connection.Execute(createTableSql);
    }
}