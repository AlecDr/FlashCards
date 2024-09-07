using FlashCards.Data.Dtos.Reports;

namespace FlashCards.Data.Daos.Interfaces;

internal interface IReportsDAO
{
    List<ResumedStudySessionsReportDTO> ResumedStudySessionsByUser(string username);
}
