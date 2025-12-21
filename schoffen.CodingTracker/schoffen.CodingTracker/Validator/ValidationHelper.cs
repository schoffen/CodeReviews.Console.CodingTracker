using System.Globalization;

namespace schoffen.CodingTracker.Validator;

public static class ValidationHelper
{
    public static bool IsDateValid(string date)
    {
        return DateTime.TryParseExact(
            date, 
            "dd/MM/yyyy HH:mm:ss", 
            CultureInfo.InvariantCulture, 
            DateTimeStyles.None,
            out _);
    }

    public static bool IsEndDateAfterStartDate(DateTime startDate, DateTime endDate)
    {
        return endDate > startDate;
    }
}