using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Exceptions;
using schoffen.CodingTracker.Extensions;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.Services;
using schoffen.CodingTracker.UI;
using schoffen.CodingTracker.UI.Options;

namespace schoffen.CodingTracker.Controller;

public class CodingSessionController(
    IUserInterface ui,
    CodingSessionService service)
{
    public void Start()
    {
        MainMenuLoop();
    }

    private void MainMenuLoop()
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
                    MySessionsMenuLoop();
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
        ui.NotifyUser(NotificationType.StartingNewSession);

        try
        {
            var session = TrackAndSaveSession();
            ui.ShowCodingSession(session);
        }
        catch (CodingTrackerException e)
        {
            ui.NotifyUserException(e);
        }
    }

    private CodingSession TrackAndSaveSession()
    {
        var tracker = new SessionTracker();
        tracker.Start();

        ui.NotifyUser(NotificationType.TrackingSession);
        ui.NotifyUser(NotificationType.PressEnterToStop);

        while (!Console.KeyAvailable)
        {
            ui.ShowElapsedTime(tracker.GetElapsed());
            Thread.Sleep(500);
        }

        Console.ReadKey(true);

        tracker.Stop();

        return service.SaveSession(tracker.StartDateTime, tracker.EndDateTime);
    }

    private void ExecuteInsertSession()
    {
        Console.Clear();

        var startDate = ui.GetDateTimeInput(DateType.Start);
        var endDate = ui.GetDateTimeInput(DateType.End);

        ExecuteSafely(() =>
        {
            var session = service.SaveSession(startDate, endDate);
            ui.ShowCodingSession(session);
        });
    }

    private void MySessionsMenuLoop()
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
                    codingSessions = service.GetAllSessions();

                    if (ui.TryShowSessionsTable(codingSessions))
                        SelectCodingSessionFromList(codingSessions);

                    break;
                case MySessionsOptions.FilterByPeriod:
                    FilterByPeriod(ui.GetFilterPeriodOption());

                    break;
                case MySessionsOptions.SortOrder:
                    var sortDirection = ui.GetSortDirectionOption();

                    codingSessions = service.GetAllSessionsSortedByStartTime(sortDirection);

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
                codingSessions = service.GetSessionsByDate(referenceDate);
                break;
            case FilterPeriodOptions.Week:
                var referenceWeekDate = ui.GetDateInput();
                var week = referenceWeekDate.GetWeekRange();
                codingSessions = service.GetSessionsByWeekRange(week.start, week.end);
                break;
            case FilterPeriodOptions.Year:
                var referenceYear = ui.GetYearInput();
                codingSessions = service.GetSessionsByYear(referenceYear);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(period), period, null);
        }

        if (ui.TryShowSessionsTable(codingSessions))
            SelectCodingSessionFromList(codingSessions);
    }

    private void SelectCodingSessionFromList(List<CodingSession> codingSessions)
    {
        if (!ui.GetUserConfirmationInput(ConfirmationType.ConfirmSelectSession))
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

    private void ExecuteUpdateSession(CodingSession sessionToUpdate)
    {
        var newStartDate = ui.GetDateTimeInput(DateType.Start);
        var newEndDate = ui.GetDateTimeInput(DateType.End);

        if (!ui.GetUserConfirmationInput(ConfirmationType.ConfirmUpdate))
        {
            ui.NotifyUser(NotificationType.OperationCanceled);
            return;
        }

        ExecuteSafely(() => service.UpdateSession(sessionToUpdate, newStartDate, newEndDate));

        ui.NotifyUser(NotificationType.SessionUpdated);
    }

    private void ExecuteDeleteSession(CodingSession codingSession)
    {
        if (!ui.GetUserConfirmationInput(ConfirmationType.ConfirmDelete))
        {
            ui.NotifyUser(NotificationType.OperationCanceled);
            return;
        }

        ExecuteSafely(() => service.DeleteSession(codingSession));

        ui.NotifyUser(NotificationType.SessionDeleted);
    }

    private void ExecuteSafely(Action action)
    {
        try
        {
            action();
        }
        catch (CodingTrackerException e)
        {
            ui.NotifyUserException(e);
        }
    }
}