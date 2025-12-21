using Dapper;
using schoffen.CodingTracker.Database;
using schoffen.CodingTracker.Models;

namespace schoffen.CodingTracker.Repository;

public class CodingSessionRepository(DatabaseContext dbContext) : ICodingSessionRepository
{
    public void InsertCodingSession(CodingSession session)
    {
        const string insertSql = """
                                 INSERT INTO CodingSessions (StartTime, EndTime)
                                 VALUES (@StartTime, @EndTime);
                                 """;
        
        using var connection = dbContext.CreateConnection();
        
        connection.Execute(insertSql, session);
    }

    public void UpdateCodingSession(CodingSession session)
    {
        const string updateSql = """
                                 UPDATE CodingSessions 
                                 SET StartTime = @StartTime, EndTime = @EndTime
                                 WHERE Id = @Id;
                                 """;
        
        using var connection = dbContext.CreateConnection();
        
        connection.Execute(updateSql, session);
    }

    public void DeleteCodingSession(int codingSessionId)
    {
        const string deleteSql = "DELETE FROM CodingSessions WHERE Id = @Id;";

        using var connection = dbContext.CreateConnection();

        connection.Execute(deleteSql, codingSessionId);
    }

    public List<CodingSession> GetAllCodingSessions()
    {
        const string getSql = "SELECT * FROM CodingSessions;";

        using var connection = dbContext.CreateConnection();

        return connection.Query<CodingSession>(getSql).ToList();
    }
}