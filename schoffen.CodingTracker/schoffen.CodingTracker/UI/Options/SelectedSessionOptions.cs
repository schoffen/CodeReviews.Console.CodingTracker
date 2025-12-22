using System.ComponentModel;

namespace schoffen.CodingTracker.UI.Options;

public enum SelectedSessionOptions
{
    [Description("Update Session")]
    UpdateSession,
    [Description("Delete Session")]
    DeleteSession,
    Return
}