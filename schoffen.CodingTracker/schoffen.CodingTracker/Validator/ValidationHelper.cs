using System.Globalization;

namespace schoffen.CodingTracker.Validator;

public class ValidationHelper
{
    public static bool IsDateValid(string date)
    {
        return DateTime.TryParseExact(
            date, 
            "dd/MM/yyyy HH:mm", 
            CultureInfo.InvariantCulture, 
            DateTimeStyles.None,
            out _);
    }
}