namespace FlashCards.Data.Dtos.StudySessionAnswer;

internal class StudySessionAnswerPromptDTO
{
    internal string Answer;
    internal int Points;

    internal StudySessionAnswerPromptDTO(string answer, int points)
    {
        Answer = answer;
        Points = points;
    }
}
