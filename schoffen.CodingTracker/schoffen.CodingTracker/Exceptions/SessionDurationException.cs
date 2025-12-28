namespace schoffen.CodingTracker.Exceptions;

public class SessionDurationException() : 
    CodingTrackerException("Session duration must be greater than zero");