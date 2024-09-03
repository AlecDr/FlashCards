using FlashCards.Dtos.StudySessionAnswer;

namespace FlashCards.Dtos.Card;

internal class StudySessionAnswerStoreDTO
{
    internal int CardId { get; }
    internal int StudySessionId { get; }

    internal string Answer { get; }
    internal int Points { get; }

    internal StudySessionAnswerStoreDTO(int cardId, string answer, int points, int studySessionId)
    {
        CardId = cardId;
        Answer = answer;
        Points = points;
        StudySessionId = studySessionId;
    }

    internal static StudySessionAnswerStoreDTO FromPromptDTO(int cardId, int studySessionId, StudySessionAnswerPromptDTO studySessionAnswerPromptDTO)
    {
        return new StudySessionAnswerStoreDTO(
            cardId,
            studySessionAnswerPromptDTO.Answer,
            studySessionAnswerPromptDTO.Points,
            studySessionId
        );
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            CardId,
            Answer,
            Points,
            StudySessionId,
        };
    }
}
