namespace schoffen.CodingTracker.Exceptions;

public class EndDateMustBeAfterStartDateException() : 
    CodingTrackerException("End date must be after start date.");