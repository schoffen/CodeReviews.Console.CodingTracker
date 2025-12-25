using schoffen.CodingTracker.Enums;
using schoffen.CodingTracker.Models;
using schoffen.CodingTracker.UI.Options;

namespace schoffen.CodingTracker.UI;

public interface IUserInterface
{
    public void ShowMainMenu();
    public void ShowMySessionsMenu();
    public void ShowCodingSession(CodingSession session);
    public void ShowSessionsTable(List<CodingSession> sessions);
    public void ShowMessage(string message);
    
    public string GetDateTimeInput(DateType dateType);
    public string GetDateInput();
    public int GetYearInput();
    public bool GetUserConfirmation(string message);
    
    public MainMenuOptions GetMainMenuOption();
    public MySessionsOptions GetMySessionsOption();
    public SortDirection GetSortDirectionOption();
    public FilterPeriodOptions GetFilterPeriodOption();
    
    public CodingSession SelectCodingSession(List<CodingSession> sessions);
}