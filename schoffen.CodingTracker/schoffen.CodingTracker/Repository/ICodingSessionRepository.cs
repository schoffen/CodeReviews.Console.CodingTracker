using schoffen.CodingTracker.Database;
using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Models;

namespace schoffen.CodingTracker.Repository;

public interface ICodingSessionRepository
{
    public void InsertCodingSession(CodingSession session);
    public void UpdateCodingSession(CodingSession session);
    public void DeleteCodingSession(int codingSessionId);
    public List<CodingSession> GetAllCodingSessions();
    public List<CodingSession> GetAllCodingSessionsOrderedByStartTime(SortDirection sortDirection);
    public List<CodingSession> GetCodingSessionsByDate(DateTime date);
    public List<CodingSession> GetCodingSessionsByWeek(DateTime weekStartDate, DateTime weekEndDate);
    public List<CodingSession> GetCodingSessionsByYear(int year);
}