using Dapper;
using FlashCards.Data.Dtos.Reports;
using FlashCards.Helpers;

namespace FlashCards.Data.Daos;

internal class ReportsDao
{

    internal static List<ResumedStudySessionsReportDTO> ResumedStudySessionsByUser(string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = @"
            SELECT STUDY_SESSIONS.id as SessionId, 
            STACKS.name as StackName,
            MIN(STUDY_SESSIONS.started_at) as StartedAt,
            MAX(STUDY_SESSIONS.finished_at) as FinishedAt,
            SUM(points) as TotalPoints

            FROM STUDY_SESSIONS 
               
            JOIN STACKS ON STACKS.id = STUDY_SESSIONS.stack_id
            LEFT JOIN STUDY_SESSIONS_ANSWERS ON STUDY_SESSIONS_ANSWERS.study_session_id = STUDY_SESSIONS.id

            WHERE username = @username
            
            GROUP BY STUDY_SESSIONS.id, STACKS.name
            ";

        List<ResumedStudySessionsReportDTO> resumedStudySessions = DatabaseHelper.SqliteConnection.Query<ResumedStudySessionsReportDTO>(query, new { Username = username }).ToList();

        DatabaseHelper.SqliteConnection!.Close();

        return resumedStudySessions;
    }
}
