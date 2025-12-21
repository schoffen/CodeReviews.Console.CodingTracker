using schoffen.CodingTracker.Database;
using schoffen.CodingTracker.Models;

namespace schoffen.CodingTracker.Repository;

public interface ICodingSessionRepository
{
    public void InsertCodingSession(CodingSession session);
    public void UpdateCodingSession(CodingSession session);
    public void DeleteCodingSession(int codingSessionId);
    public List<CodingSession> GetAllCodingSessions();
}