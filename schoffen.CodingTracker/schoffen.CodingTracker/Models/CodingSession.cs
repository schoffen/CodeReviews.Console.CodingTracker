namespace schoffen.CodingTracker.Models;

public class CodingSession
{
    public int Id { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public int Duration { get; private set; }

    public CodingSession(DateTime startTime, DateTime endTime, )
    
    private static int CalculateDuration()
    {
        return (int)(EndTime - StartTime).TotalMinutes;
    }
}