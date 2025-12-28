using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Models;

namespace schoffen.CodingTracker.Repository;

public interface ICodingSessionRepository
{
    public void InsertCodingSession(CodingSession session);
    public void UpdateCodingSession(CodingSession session);
    public void DeleteCodingSession(CodingSession session);
    public List<CodingSession> GetAllCodingSessions();
    public CodingSession? GetCodingSessionById(int id);
    public List<CodingSession> GetAllCodingSessionsOrderedByStartTime(SortDirection sortDirection);
    public List<CodingSession> GetCodingSessionsByDate(DateTime date);
    public List<CodingSession> GetCodingSessionsByWeek(DateTime weekStartDate, DateTime weekEndDate);
    public List<CodingSession> GetCodingSessionsByYear(int year);
}