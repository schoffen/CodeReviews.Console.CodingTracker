namespace schoffen.CodingTracker.Models;

public class CodingSession
{
    public int Id { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public int Duration { get; set; }
    
    private double CalculateDuration()
    {
        return (EndTime - StartTime).TotalMinutes;
    }
}