using Dapper;
using schoffen.CodingTracker.Database;
using schoffen.CodingTracker.Enums;
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

    public void DeleteCodingSession(CodingSession session)
    {
        const string deleteSql = "DELETE FROM CodingSessions WHERE Id = @Id;";

        using var connection = dbContext.CreateConnection();

        connection.Execute(deleteSql, session);
    }

    public List<CodingSession> GetAllCodingSessions()
    {
        const string getSql = "SELECT * FROM CodingSessions;";

        using var connection = dbContext.CreateConnection();

        return connection.Query<CodingSession>(getSql).ToList();
    }

    public CodingSession? GetCodingSessionById(int id)
    {
        const string getSql = "SELECT * FROM CodingSessions WHERE Id = @Id;";

        using var connection = dbContext.CreateConnection();

        return connection.QuerySingleOrDefault<CodingSession>(getSql, new { Id = id });
    }

    public List<CodingSession> GetAllCodingSessionsOrderedByStartTime(SortDirection sortDirection)
    {
        var direction = sortDirection == SortDirection.Ascending ? "ASC" : "DESC";

        var getSql = $"SELECT * FROM CodingSessions ORDER BY StartTime {direction};";

        using var connection = dbContext.CreateConnection();

        return connection.Query<CodingSession>(getSql).ToList();
    }

    public List<CodingSession> GetCodingSessionsByDate(DateTime date)
    {
        var start = date.Date;
        var end = date.Date.AddDays(1);

        const string getSql =
            "SELECT * FROM CodingSessions WHERE StartTime  >= @Start AND StartTime < @End ORDER BY StartTime;";

        using var connection = dbContext.CreateConnection();

        return connection.Query<CodingSession>(getSql, new { Start = start, End = end }).ToList();
    }

    public List<CodingSession> GetCodingSessionsByWeek(DateTime weekStartDate, DateTime weekEndDate)
    {
        const string getSql =
            "SELECT * FROM CodingSessions WHERE StartTime  >= @Start AND StartTime <= @End ORDER BY StartTime;";

        using var connection = dbContext.CreateConnection();

        return connection.Query<CodingSession>(getSql, new { Start = weekStartDate, End = weekEndDate }).ToList();
    }

    public List<CodingSession> GetCodingSessionsByYear(int year)
    {
        var start = new DateTime(year, 1, 1);
        var end = start.AddYears(1);

        const string getSql =
            "SELECT * FROM CodingSessions WHERE StartTime  >= @Start AND StartTime <= @End ORDER BY StartTime;";

        using var connection = dbContext.CreateConnection();

        return connection.Query<CodingSession>(getSql, new { Start = start, End = end }).ToList();
    }
}