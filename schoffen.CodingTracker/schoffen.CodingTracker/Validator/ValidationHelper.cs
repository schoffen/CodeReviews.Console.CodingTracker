using System.Globalization;

namespace schoffen.CodingTracker.Validator;

public static class ValidationHelper
{
    private const string DateFormat = "dd/MM/yyyy";
    private const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
    
    public static bool IsYearValid(int year)
    {
        return year <= DateTime.Now.Year;
    }
    
    public static bool IsDateValid(string date)
    {
        return DateTime.TryParseExact(
            date, 
            DateFormat,
            CultureInfo.InvariantCulture, 
            DateTimeStyles.None,
            out var dateTime) && IsDateTimeNotInFuture(dateTime);
    }
    
    public static bool IsDateTimeValid(string date)
    {
        return DateTime.TryParseExact(
            date, 
            DateTimeFormat,
            CultureInfo.InvariantCulture, 
            DateTimeStyles.None,
            out var dateTime) && IsDateTimeNotInFuture(dateTime);
    }

    public static bool IsEndDateAfterStartDate(DateTime startDate, DateTime endDate)
    {
        return endDate > startDate;
    }

    private static bool IsDateTimeNotInFuture(DateTime dateTime)
    {
        return dateTime <= DateTime.Now;
    }
}