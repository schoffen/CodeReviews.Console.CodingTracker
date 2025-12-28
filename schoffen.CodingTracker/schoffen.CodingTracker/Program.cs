using schoffen.CodingTracker.Controller;
using schoffen.CodingTracker.Repository;
using schoffen.CodingTracker.Services;
using schoffen.CodingTracker.UI;

namespace schoffen.CodingTracker;

internal abstract class Program
{
    public static void Main()
    {
        var dbContext = new Database.DatabaseContext();
        var repository = new CodingSessionRepository(dbContext);
        var service = new CodingSessionService(repository);
        var ui = new ConsoleUi();
        var controller = new CodingSessionController(ui, service);
        
        controller.Start();
    }
}