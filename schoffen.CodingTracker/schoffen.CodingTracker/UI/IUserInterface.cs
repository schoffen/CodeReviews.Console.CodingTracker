using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Exceptions;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.UI.Options;

namespace schoffen.CodingTracker.UI;

public interface IUserInterface
{
    public void ShowMainMenu();
    public void ShowMySessionsMenu();
    public void ShowCodingSession(CodingSession session);
    public bool TryShowSessionsTable(List<CodingSession> sessions);
    public void ShowSelectedSessionMenu(CodingSession codingSession);
    public void ShowElapsedTime(TimeSpan elapsed);
    
    public void NotifyUser(NotificationType notificationType);
    public void NotifyUserException(CodingTrackerException exception);
    
    public DateTime GetDateTimeInput(DateType dateType);
    public DateTime GetDateInput();
    public int GetYearInput();
    public bool GetUserConfirmationInput(ConfirmationType confirmationType);

    public MainMenuOptions GetMainMenuOption();
    public MySessionsOptions GetMySessionsOption();
    public SortDirection GetSortDirectionOption();
    public FilterPeriodOptions GetFilterPeriodOption();
    public SelectedSessionOptions GetSelectedSessionOption();

    public CodingSession SelectCodingSession(List<CodingSession> sessions);
}