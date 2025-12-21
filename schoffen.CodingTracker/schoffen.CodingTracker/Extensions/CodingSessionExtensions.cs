using schoffen.CodingTracker.Models;

namespace schoffen.CodingTracker.Extensions;

public static class CodingSessionExtensions
{
    private static TimeSpan GetDuration(this CodingSession session)
    {
        return TimeSpan.FromSeconds(session.Duration);
    }

    public static string GetFormattedDuration(this CodingSession session)
    {
        return session.GetDuration().ToString(@"hh\:mm\:ss");
    }
}