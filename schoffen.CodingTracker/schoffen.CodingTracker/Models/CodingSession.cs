namespace schoffen.CodingTracker.Models;

public class CodingSession
{
    public int Id { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public int DurationInSeconds =>
        (int)(EndTime - StartTime).TotalSeconds;
}