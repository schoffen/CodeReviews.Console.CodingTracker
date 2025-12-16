using Dapper;
using Microsoft.Data.Sqlite;

namespace schoffen.CodingTracker.Database;

public class Database
{
    public Database()
    {
        Initialize();   
    }
    
    private static void Initialize()
    {
        using var connection = new SqliteConnection(DatabaseConfiguration.GetConnectionString());

        const string codingSessionsTableQueue = """
                                                 CREATE TABLE IF NOT EXISTS CodingSessions (
                                                     Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                     StartTime TEXT,
                                                     EndTime TEXT,
                                                     Duration INTEGER
                                                 );
                                                """;

        connection.Execute(codingSessionsTableQueue);
    }
}