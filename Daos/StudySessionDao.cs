using Dapper;
using FlashCards.Dtos.Card;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class StudySessionDao
{
    internal static void StoreStudySession(StudySessionStoreDTO studySessionStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO STUDY_SESSIONS (started_at, finished_at, stack_id) VALUES (@StartedAt, @FinishedAt, @StackId);";

        DatabaseHelper.SqliteConnection!.Execute(query, studySessionStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }
}
