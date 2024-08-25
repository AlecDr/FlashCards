namespace CodingTracker.Dtos.CodingSession;

internal class CodingSessionPromptDTO
{
    internal string? Description { get; }
    internal DateTime StartDateTime { get; }
    internal DateTime EndDateTime { get; }

    internal long DurationInSeconds
    {
        get
        {
            return long.Parse((EndDateTime - StartDateTime).TotalSeconds.ToString());
        }
    }

    internal CodingSessionPromptDTO(string? description, DateTime startDateTime, DateTime endDateTime)
    {
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }
}
