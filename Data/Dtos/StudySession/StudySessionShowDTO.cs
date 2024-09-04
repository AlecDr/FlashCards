namespace FlashCards.Data.Dtos.StudySession;

internal class StudySessionShowDTO
{
    internal int Id { get; }
    internal int StackId { get; }
    internal DateTime StartedAt { get; }
    internal DateTime? FinishedAt { get; }

    internal StudySessionShowDTO() { }

    internal StudySessionShowDTO(int id, int stackId, DateTime startedAt, DateTime? finishedAt)
    {
        Id = id;
        StackId = stackId;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
    }
}
