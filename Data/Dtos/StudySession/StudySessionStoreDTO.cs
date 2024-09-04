namespace FlashCards.Data.Dtos.StudySession;

internal class StudySessionStoreDTO
{
    internal int StackId { get; }
    internal DateTime StartedAt { get; }
    internal DateTime? FinishedAt { get; }

    internal StudySessionStoreDTO(int stackId, DateTime startedAt, DateTime? finishedAt = null)
    {
        StackId = stackId;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            StackId,
            StartedAt,
            FinishedAt,
        };
    }
}
