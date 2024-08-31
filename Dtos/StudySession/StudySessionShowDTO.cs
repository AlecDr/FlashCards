namespace FlashCards.Dtos.StudySession;

internal class StudySessionShowDTO
{
    internal int Id { get; }
    internal string Username { get; }
    internal int StackId { get; }
    internal DateTime StartedAt { get; }
    internal DateTime? FinishedAt { get; }

    internal StudySessionShowDTO() { }

    internal StudySessionShowDTO(int id, string username, int stackId, DateTime startedAt, DateTime? finishedAt)
    {
        Id = id;
        Username = username;
        StackId = stackId;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
    }
}
