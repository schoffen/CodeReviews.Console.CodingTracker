using System.Diagnostics;
using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Extensions;
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
                    MySessionsMenu();
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
        
        ui.ShowMessage("\nPress any key to continue");
        Console.ReadKey();
    }

    private void InsertSession()
    {
        Console.Clear();

        var startDate = DateTime.Parse(ui.GetDateTimeInput(DateType.Start));
        var endDate = DateTime.Parse(ui.GetDateTimeInput(DateType.End));

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
        var isRunning = true;
        
        while (isRunning)
        {
            ui.ShowMySessionsMenu();
            var option = ui.GetMySessionsOption();

            switch (option)
            {
                case MySessionsOptions.ShowAllSessions:
                    ui.ShowSessionsTable(repository.GetAllCodingSessions());
                    break;
                case MySessionsOptions.FilterByPeriod:
                    FilterByPeriod(ui.GetFilterPeriodOption());
                    break;
                case MySessionsOptions.SortOrder:
                    var sortOrder = ui.GetSortDirectionOption();
                    var codingSessions = repository.GetAllCodingSessionsOrderedByStartTime(sortOrder);
                    ui.ShowSessionsTable(codingSessions);
                    break;
                case MySessionsOptions.SelectSession:
                    break;
                case MySessionsOptions.Return:
                    isRunning = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void FilterByPeriod(FilterPeriodOptions period)
    {
        switch (period)
        {
            case FilterPeriodOptions.Day:
                var referenceDate = DateTime.Parse(ui.GetDateInput());
                ui.ShowSessionsTable(repository.GetCodingSessionsByDate(referenceDate));
                break;
            case FilterPeriodOptions.Week:
                var referenceWeekDate = DateTime.Parse(ui.GetDateInput());
                var week = referenceWeekDate.GetWeekRange();
                ui.ShowSessionsTable(repository.GetCodingSessionsByWeek(week.start, week.end));
                break;
            case FilterPeriodOptions.Year:
                var referenceYear = ui.GetYearInput();
                ui.ShowSessionsTable(repository.GetCodingSessionsByYear(referenceYear));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(period), period, null);
        }
    }
}