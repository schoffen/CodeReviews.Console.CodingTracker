using System.ComponentModel;

namespace schoffen.CodingTracker.UI.Options;

public enum MySessionsOptions
{
    [Description("Filter By Period")]
    FilterByPeriod,
    [Description("Filter By Order")]
    FilterByOrder,
    [Description("UpdateSession")]
    UpdateSession,
    [Description("Delete Session")]
    DeleteSession,
    Return
}