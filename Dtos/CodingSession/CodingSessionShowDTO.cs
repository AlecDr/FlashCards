namespace HabitLogger.Dtos.HabitOccurrence;

internal class CodingSessionShowDTO
{
    internal int Id { get; }
    internal string? Description { get; }
    internal string StartDate { get; }
    internal string EndDate { get; }
    internal long DurationInSeconds { get; }

    internal CodingSessionShowDTO() { }

    internal CodingSessionShowDTO(int id, string? description, string startDate, string endDate, long durationInSeconds)
    {
        Id = id;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        DurationInSeconds = durationInSeconds;
    }
}
