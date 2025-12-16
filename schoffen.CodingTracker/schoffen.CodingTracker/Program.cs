namespace schoffen.CodingTracker;

internal abstract class Program
{
    public static void Main()
    {
        var db = new Database.DatabaseContext();
    }
}