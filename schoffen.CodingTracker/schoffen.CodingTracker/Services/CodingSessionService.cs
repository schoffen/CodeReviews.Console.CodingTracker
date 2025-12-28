using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Exceptions;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.Repository;

namespace schoffen.CodingTracker.Services;

public class CodingSessionService(ICodingSessionRepository repository)
{
    public CodingSession SaveSession(DateTime start, DateTime end)
    {
        ValidateSessionDates(start, end);

        var codingSession = new CodingSession
        {
            StartTime = start,
            EndTime = end
        };

        repository.InsertCodingSession(codingSession);

        return codingSession;
    }

    public void UpdateSession(CodingSession sessionToUpdate, DateTime newStart, DateTime newEnd)
    {
        var session = repository.GetCodingSessionById(sessionToUpdate.Id);

        if (session == null)
            throw new SessionNotFoundException();

        ValidateSessionDates(newStart, newEnd);

        session.StartTime = newStart;
        session.EndTime = newEnd;

        repository.UpdateCodingSession(session);
    }

    public void DeleteSession(CodingSession codingSession)
    {
        var session = repository.GetCodingSessionById(codingSession.Id);

        if (session == null)
            throw new SessionNotFoundException();

        repository.DeleteCodingSession(session);
    }

    public List<CodingSession> GetAllSessions() => repository.GetAllCodingSessions();

    public List<CodingSession> GetSessionsByDate(DateTime dateTime) =>
        repository.GetCodingSessionsByDate(dateTime);

    public List<CodingSession> GetSessionsByWeekRange(DateTime startDate, DateTime endDate) =>
        repository.GetCodingSessionsByWeek(startDate, endDate);

    public List<CodingSession> GetAllSessionsSortedByStartTime(SortDirection direction) =>
        repository.GetAllCodingSessionsOrderedByStartTime(direction);

    public List<CodingSession> GetSessionsByYear(int year) => repository.GetCodingSessionsByYear(year);

    private static void ValidateSessionDates(DateTime start, DateTime end)
    {
        if (end < start)
            throw new EndDateMustBeAfterStartDateException();

        if (end == start)
            throw new SessionDurationException();
    }
}