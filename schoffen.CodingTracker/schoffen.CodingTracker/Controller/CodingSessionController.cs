using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Exceptions;
using schoffen.CodingTracker.Extensions;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.Repository;
using schoffen.CodingTracker.Services;
using schoffen.CodingTracker.UI;
using schoffen.CodingTracker.UI.Options;

namespace schoffen.CodingTracker.Controller;

public class CodingSessionController(
    IUserInterface ui,
    ICodingSessionRepository repository,
    CodingSessionService service)
{
    public void Run()
    {
        MainMenu();
    }

    private void MainMenu()
    {
        var isRunning = true;

        while (isRunning)
        {
            ui.ShowMainMenu();
            var option = ui.GetMainMenuOption();

            switch (option)
            {
                case MainMenuOptions.StartNewSession:
                    ExecuteStartNewSession();
                    break;
                case MainMenuOptions.InsertSession:
                    ExecuteInsertSession();
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

    private void ExecuteStartNewSession()
    {
        Console.Clear();
        ui.ShowMessage(UiMessage.StartingNewSession);

        try
        {
            var session = TrackAndSaveSession();
            ui.ShowCodingSession(session);
        }
        catch (CodingTrackerException e)
        {
            ui.ShowExceptionMessage(e);
        }
    }

    private CodingSession TrackAndSaveSession()
    {
        var tracker = new SessionTracker();
        tracker.Start();

        ui.ShowMessage(UiMessage.TrackingSession);
        ui.ShowMessage(UiMessage.PressEnterToStop);

        while (!Console.KeyAvailable)
        {
            ui.ShowElapsedTime(tracker.GetElapsed());
            Thread.Sleep(500);
        }

        Console.ReadKey(true);

        tracker.Stop();

        return service.CreateAndSave(tracker.StartDateTime, tracker.EndDateTime);
    }

    private void ExecuteInsertSession()
    {
        Console.Clear();

        var startDate = ui.GetDateTimeInput(DateType.Start);
        var endDate = ui.GetDateTimeInput(DateType.End);

        try
        {
            var session = service.CreateAndSave(startDate, endDate);
            ui.ShowCodingSession(session);
        }
        catch (CodingTrackerException e)
        {
            ui.ShowExceptionMessage(e);
        }
    }

    private void MySessionsMenu()
    {
        var isRunning = true;
        while (isRunning)
        {
            ui.ShowMySessionsMenu();
            var option = ui.GetMySessionsOption();

            List<CodingSession> codingSessions;
            switch (option)
            {
                case MySessionsOptions.ShowAllSessions:
                    codingSessions = repository.GetAllCodingSessions();

                    if (ui.TryShowSessionsTable(codingSessions))
                        SelectCodingSessionFromList(codingSessions);
                    break;
                case MySessionsOptions.FilterByPeriod:
                    FilterByPeriod(ui.GetFilterPeriodOption());
                    break;
                case MySessionsOptions.SortOrder:
                    var sortOrder = ui.GetSortDirectionOption();
                    codingSessions = repository.GetAllCodingSessionsOrderedByStartTime(sortOrder);

                    if (ui.TryShowSessionsTable(codingSessions))
                        SelectCodingSessionFromList(codingSessions);
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
        List<CodingSession> codingSessions;

        switch (period)
        {
            case FilterPeriodOptions.Day:
                var referenceDate = ui.GetDateInput();
                codingSessions = repository.GetCodingSessionsByDate(referenceDate);
                break;
            case FilterPeriodOptions.Week:
                var referenceWeekDate = ui.GetDateInput();
                var week = referenceWeekDate.GetWeekRange();
                codingSessions = repository.GetCodingSessionsByWeek(week.start, week.end);
                break;
            case FilterPeriodOptions.Year:
                var referenceYear = ui.GetYearInput();
                codingSessions = repository.GetCodingSessionsByYear(referenceYear);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(period), period, null);
        }

        if (ui.TryShowSessionsTable(codingSessions))
            SelectCodingSessionFromList(codingSessions);
    }

    private void SelectCodingSessionFromList(List<CodingSession> codingSessions)
    {
        if (!ui.GetUserConfirmation(UiConfirmationMessages.ConfirmSelectSession))
            return;

        var session = ui.SelectCodingSession(codingSessions);
        SelectedSessionMenu(session);
    }

    private void SelectedSessionMenu(CodingSession codingSession)
    {
        ui.ShowSelectedSessionMenu(codingSession);

        var option = ui.GetSelectedSessionOption();

        switch (option)
        {
            case SelectedSessionOptions.UpdateSession:
                ExecuteUpdateSession(codingSession);
                break;
            case SelectedSessionOptions.DeleteSession:
                ExecuteDeleteSession(codingSession);
                break;
            case SelectedSessionOptions.Return:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ExecuteUpdateSession(CodingSession codingSession)
    {
        var newStartDate = ui.GetDateTimeInput(DateType.Start);
        var newEndDate = ui.GetDateTimeInput(DateType.End);

        if (!ui.GetUserConfirmation(UiConfirmationMessages.ConfirmUpdate))
        {
            ui.ShowMessage(UiMessage.OperationCanceled);
            return;
        }

        repository.UpdateCodingSession(new CodingSession
            { Id = codingSession.Id, StartTime = newStartDate, EndTime = newEndDate });

        ui.ShowMessage(UiMessage.SessionUpdated);
    }

    private void ExecuteDeleteSession(CodingSession codingSession)
    {
        if (!ui.GetUserConfirmation(UiConfirmationMessages.ConfirmDelete))
        {
            ui.ShowMessage(UiMessage.OperationCanceled);
            return;
        }

        repository.DeleteCodingSession(codingSession.Id);

        ui.ShowMessage(UiMessage.SessionDeleted);
    }
}