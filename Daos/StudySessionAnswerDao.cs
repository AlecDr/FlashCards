using Dapper;
using FlashCards.Dtos.Card;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class StudySessionAnswerDao
{
    internal static void StoreStudySessionAnswer(StudySessionAnswerStoreDTO studySessionAnswerStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO STUDY_SESSIONS_ANSWERS (answer, card_id, points) VALUES (@Answer, @CardId, @Points);";

        DatabaseHelper.SqliteConnection!.Execute(query, studySessionAnswerStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }
}
