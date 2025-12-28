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
        var ui = new ConsoleUi();
        var service = new CodingSessionService(repository);
        var controller = new CodingSessionController(ui, repository, service);
        
        controller.Run();
    }
}