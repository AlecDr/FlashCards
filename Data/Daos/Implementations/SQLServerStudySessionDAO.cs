using Dapper;
using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.StudySession;
using FlashCards.Helpers;

namespace FlashCards.Data.Daos;

internal class SQLServerStudySessionDAO : IStudySessionDAO
{
    private readonly IStudySessionAnswerDAO _studySessionAnswerDao;

    public SQLServerStudySessionDAO(IStudySessionAnswerDAO studySessionAnswerDao)
    {
        _studySessionAnswerDao = studySessionAnswerDao;
    }

    public StudySessionShowDTO? Store(StudySessionStoreDTO studySessionStoreDTO)
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

        return Find(id);
    }

    public List<StudySessionShowDTO> FindByStackId(int stackId)
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

    public StudySessionShowDTO? Find(int id)
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

    public bool Update(StudySessionUpdateDTO studySessionUpdateDTO)
    {
        StudySessionShowDTO? studySession = Find(studySessionUpdateDTO.Id);

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

    public void DeleteByStackId(int stackId)
    {
        List<StudySessionShowDTO> studySessions = FindByStackId(stackId);

        foreach (StudySessionShowDTO studySession in studySessions)
        {
            Delete(studySession.Id);
        }
    }

    public bool Delete(int id)
    {
        StudySessionShowDTO? studySession = Find(id);

        if (studySession != null)
        {
            _studySessionAnswerDao.DeleteByStudySessionId(studySession.Id);

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
