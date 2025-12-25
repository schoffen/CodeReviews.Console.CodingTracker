namespace schoffen.CodingTracker.Extensions;

public static class DateTimeExtensions
{
    public static (DateTime start, DateTime end) GetWeekRange(this DateTime date)
    {
        var daysToSubtract = date.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)date.DayOfWeek - 1;

        var start = date.Date.AddDays(-daysToSubtract);
        var end = start.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

        return (start, end);
    }
}