using FlashCards.Dtos.Card;

namespace FlashCards.Dtos.StudySession;

internal class StudySessionUpdateDTO : StudySessionStoreDTO
{
    internal int Id { get; }

    internal StudySessionUpdateDTO(int id, int stackId, DateTime startedAt, DateTime? finishedAt = null)
        : base(stackId, startedAt, finishedAt)
    {
        Id = id;
    }

    internal override object ToAnonymousObject()
    {
        return new
        {
            Id,
            StackId,
            StartedAt,
            FinishedAt,
        };
    }
}
