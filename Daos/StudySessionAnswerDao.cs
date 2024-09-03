using Dapper;
using FlashCards.Dtos.Card;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class StudySessionAnswerDao
{
    internal static void StoreStudySessionAnswer(StudySessionAnswerStoreDTO studySessionAnswerStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO STUDY_SESSIONS_ANSWERS (answer, card_id, points, study_session_id) VALUES (@Answer, @CardId, @Points, @StudySessionId);";

        DatabaseHelper.SqliteConnection!.Execute(query, studySessionAnswerStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static void DeleteStudySessionAnswer(int id)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "DELETE FROM STUDY_SESSIONS_ANSWERS WHERE id = @Id;";

        DatabaseHelper.SqliteConnection!.Execute(query, new { Id = id });
        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static void DeleteStudySessionAnswersByCardId(int cardId)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "DELETE FROM STUDY_SESSIONS_ANSWERS WHERE card_id = @CardId;";

        DatabaseHelper.SqliteConnection!.Execute(query, new { CardId = cardId });
        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static void DeleteStudySessionAnswersByStudySessionId(int studySessionId)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "DELETE FROM STUDY_SESSIONS_ANSWERS WHERE study_session_id = @StudySessionId;";

        DatabaseHelper.SqliteConnection!.Execute(query, new { StudySessionId = studySessionId });
        DatabaseHelper.SqliteConnection!.Close();
    }


}
