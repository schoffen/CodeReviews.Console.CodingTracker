using System.Diagnostics;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.Repository;
using schoffen.CodingTracker.UI;
using schoffen.CodingTracker.UI.Options;
using schoffen.CodingTracker.Validator;

namespace schoffen.CodingTracker.Controller;

public class CodingSessionController(IUserInterface ui, ICodingSessionRepository repository)
{
    public void Run()
    {
        var isRunning = true;
        
        while (isRunning)
        {
            ui.ShowMainMenu();
            var option = ui.GetMainMenuOption();

            switch (option)
            {
                case MainMenuOptions.StartNewSession:
                    StartNewSession();
                    break;
                case MainMenuOptions.InsertSession:
                    InsertSession();
                    break;
                case MainMenuOptions.MySessions:
                    break;
                case MainMenuOptions.Quit:
                    isRunning = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void StartNewSession()
    {
        Console.Clear();
        
        ui.ShowMessage("Starting New Session");
        ui.ShowMessage("Press any key to begin!");
        Console.ReadKey();

        var startDate = DateTime.Now;
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        ui.ShowMessage("Tracking session...");
        ui.ShowMessage("Press any key to stop");
        Console.ReadKey();

        var endDate = DateTime.Now;
        stopwatch.Stop();

        var codingSession = new CodingSession
        {
            StartTime = startDate,
            EndTime = endDate
        };
        
        repository.InsertCodingSession(codingSession);
        ui.ShowCodingSession(codingSession);
        
        ui.ShowMessage("Press any key to continue");
        Console.ReadKey();
    }

    private void InsertSession()
    {
        Console.Clear();

        var startDate = DateTime.Parse(ui.GetDateTimeInput());
        var endDate = DateTime.Parse(ui.GetDateTimeInput());

        if (ValidationHelper.IsEndDateAfterStartDate(startDate, endDate))
        {
            var codingSession = new CodingSession
            {
                StartTime = startDate,
                EndTime = endDate
            };
            
            repository.InsertCodingSession(codingSession);
            ui.ShowCodingSession(codingSession);
        }
        else
        {
            ui.ShowMessage("End date cannot be before start date.");
        }
        
        ui.ShowMessage("Press any key to continue");
        Console.ReadKey();
    }

    private void MySessionsMenu()
    {
        Console.Clear();
        
        ui.ShowMySessionsMenu();
        var option = ui.GetMySessionsOption();

        switch (option)
        {
            case MySessionsOptions.FilterByPeriod:
                break;
            case MySessionsOptions.FilterByOrder:
                break;
            case MySessionsOptions.UpdateSession:
                break;
            case MySessionsOptions.DeleteSession:
                break;
            case MySessionsOptions.Return:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateSession(CodingSession session)
    {
        // TODO
    }
}