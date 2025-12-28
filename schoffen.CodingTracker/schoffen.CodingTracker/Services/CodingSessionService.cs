using schoffen.CodingTracker.Exceptions;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.Repository;

namespace schoffen.CodingTracker.Services;

public class CodingSessionService(ICodingSessionRepository repository)
{
    public CodingSession CreateAndSave(DateTime start, DateTime end)
    {
        if (end < start)
            throw new EndDateMustBeAfterStartDateException();

        if (end == start)
            throw new SessionDurationException();
        
        var codingSession = new CodingSession
        {
            StartTime = start,
            EndTime = end
        };
        
        repository.InsertCodingSession(codingSession);

        return codingSession;
    }
    
    // TODO create new Service Methods
}