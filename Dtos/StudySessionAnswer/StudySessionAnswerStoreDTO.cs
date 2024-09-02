using FlashCards.Dtos.StudySessionAnswer;

namespace FlashCards.Dtos.Card;

internal class StudySessionAnswerStoreDTO
{
    internal int CardId { get; }
    internal string Answer { get; }
    internal int Points { get; }

    internal StudySessionAnswerStoreDTO(int cardId, string answer, int points)
    {
        CardId = cardId;
        Answer = answer;
        Points = points;
    }

    internal static StudySessionAnswerStoreDTO FromPromptDTO(int cardId, StudySessionAnswerPromptDTO studySessionAnswerPromptDTO)
    {
        return new StudySessionAnswerStoreDTO(
            cardId,
            studySessionAnswerPromptDTO.Answer,
            studySessionAnswerPromptDTO.Points
        );
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            CardId,
            Answer,
            Points,
        };
    }
}
