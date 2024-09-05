namespace FlashCards.Data.Dtos.Reports;

internal class ResumedStudySessionsReportDTO
{
    internal int SessionId { get; }
    internal string StackName { get; }
    internal DateTime StartedAt { get; }
    internal DateTime? FinishedAt { get; }

    internal int TotalPoints { get; }

    public ResumedStudySessionsReportDTO(int sessionId, string stackName, DateTime startedAt, DateTime? finishedAt, int totalPoints)
    {
        SessionId = sessionId;
        StackName = stackName;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
        TotalPoints = totalPoints;
    }
}
