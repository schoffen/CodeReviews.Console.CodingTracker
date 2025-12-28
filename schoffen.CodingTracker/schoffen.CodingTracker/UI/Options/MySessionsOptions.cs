using System.ComponentModel;

namespace schoffen.CodingTracker.UI.Options;

public enum MySessionsOptions
{
    [Description("Show All Sessions")]
    ShowAllSessions,
    [Description("Filter By Period")]
    FilterByPeriod,
    [Description("Sort Order")]
    SortOrder,
    Return
}