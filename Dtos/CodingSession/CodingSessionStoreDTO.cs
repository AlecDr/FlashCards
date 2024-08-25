using CodingTracker.Dtos.CodingSession;

namespace HabitLogger.Dtos.HabitOccurrence;

internal class CodingSessionStoreDTO
{
    internal string Username { get; }
    internal string? Description { get; }
    internal string StartDateTime { get; }
    internal string EndDateTime { get; }
    internal long DurationInSeconds { get; }

    internal CodingSessionStoreDTO(string username, string? description, string startDateTime, string endDateTime, long durationInSeconds)
    {
        Username = username;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        DurationInSeconds = durationInSeconds;
    }

    internal static CodingSessionStoreDTO FromPromptDTO(string username, CodingSessionPromptDTO codingSessionPromptDTO)
    {
        return new CodingSessionStoreDTO(
            username,
            codingSessionPromptDTO.Description,
            codingSessionPromptDTO.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            codingSessionPromptDTO.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            codingSessionPromptDTO.DurationInSeconds
        );
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            Username,
            Description,
            StartDateTime,
            EndDateTime,
            DurationInSeconds
        };
    }
}
