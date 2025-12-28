namespace schoffen.CodingTracker.Enums;

public enum NotificationType
{
    StartingNewSession,
    TrackingSession,
    PressEnterToStop,
    
    NoSessionsFound,
    OnlyOneSessionFound,
    
    OperationCanceled,
    SessionUpdated,
    SessionDeleted,
    
    ConfirmUpdate,
    ConfirmDelete,
    ConfirmSelectSession
}