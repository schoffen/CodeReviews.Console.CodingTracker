using System.ComponentModel;

namespace schoffen.CodingTracker.UI.Options;

public enum MainMenuOptions
{
    [Description("Start New Session")]
    StartNewSession,
    [Description("Insert Session")]
    InsertSession,
    [Description("My Sessions")]
    MySessions,
    [Description("Quit")]
    Quit
}