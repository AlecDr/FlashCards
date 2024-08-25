using CodingTracker.Dtos.CodingSession;

namespace HabitLogger.Dtos.HabitOccurrence;

internal class CodingSessionUpdateDTO : CodingSessionStoreDTO
{
    internal int Id { get; }

    internal CodingSessionUpdateDTO(
        int id,
        string username,
        string? description,
        string startTime,
        string endTime,
        long durationInSeconds)
        : base(username, description, startTime, endTime, durationInSeconds)
    {
        Id = id;
    }

    internal static CodingSessionUpdateDTO FromPromptDTO(int id, string username, CodingSessionPromptDTO codingSessionPromptDTO)
    {
        return new CodingSessionUpdateDTO(
            id,
            username,
            codingSessionPromptDTO.Description,
            codingSessionPromptDTO.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            codingSessionPromptDTO.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            codingSessionPromptDTO.DurationInSeconds
        );
    }

    internal override object ToAnonymousObject()
    {
        return new
        {
            Id,
            Username = Username,
            Description,
            StartDateTime,
            EndDateTime,
            DurationInSeconds
        };
    }
}
