using Dapper;
using FlashCards.Dtos.Card;
using FlashCards.Dtos.StudySession;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class StudySessionDao
{
    internal static StudySessionShowDTO? StoreStudySession(StudySessionStoreDTO studySessionStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = @"
            INSERT INTO STUDY_SESSIONS 
            (started_at, finished_at, stack_id) 
            OUTPUT INSERTED.id
            VALUES 
            (@StartedAt, @FinishedAt, @StackId)
            ;";

        int id = DatabaseHelper.SqliteConnection!.QuerySingle<int>(query, studySessionStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();

        return FindStudySessionById(id);
    }

    internal static List<StudySessionShowDTO> FindStudySessionsByStackId(int stackId)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = @"
            SELECT id as Id, started_at as StartedAt, finished_at as FinishedAt, stack_id as StackId
            
            FROM STUDY_SESSIONS 

            WHERE stack_id = @StackId";

        List<StudySessionShowDTO> studySessions = DatabaseHelper.SqliteConnection.Query<StudySessionShowDTO>(query, new { StackId = stackId }).ToList();

        DatabaseHelper.SqliteConnection!.Close();

        return studySessions;
    }

    internal static StudySessionShowDTO? FindStudySessionById(int id)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = @"
            SELECT id as Id, started_at as StartedAt, finished_at as FinishedAt, stack_id as StackId
            
            FROM STUDY_SESSIONS 

            WHERE id = @id";

        StudySessionShowDTO? studySessionShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<StudySessionShowDTO>(query, new { id });

        DatabaseHelper.SqliteConnection!.Close();

        return studySessionShowDTO;
    }

    internal static bool UpdateStudySession(StudySessionUpdateDTO studySessionUpdateDTO)
    {
        StudySessionShowDTO? studySession = FindStudySessionById(studySessionUpdateDTO.Id);

        if (studySession != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            string query = "UPDATE STUDY_SESSIONS SET started_at = @StartedAt, finished_at = @FinishedAt, stack_id = @StackId WHERE id = @Id;";

            DatabaseHelper.SqliteConnection!.Execute(query, studySessionUpdateDTO.ToAnonymousObject());
            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    internal static void DeleteStudySessionByStackId(int stackId)
    {
        List<StudySessionShowDTO> studySessions = FindStudySessionsByStackId(stackId);

        foreach (StudySessionShowDTO studySession in studySessions)
        {
            DeleteStudySessionById(studySession.Id);
        }
    }

    internal static bool DeleteStudySessionById(int id)
    {
        StudySessionShowDTO? studySession = FindStudySessionById(id);

        if (studySession != null)
        {
            StudySessionAnswerDao.DeleteStudySessionAnswersByStudySessionId(studySession.Id);

            DatabaseHelper.SqliteConnection!.Open();

            string query = "DELETE FROM STUDY_SESSIONS WHERE id = @Id;";

            DatabaseHelper.SqliteConnection.Execute(query, new
            {
                Id = id,
            });

            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }
}
