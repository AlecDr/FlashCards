using FlashCards.Data.Dtos.StudySessionAnswer;

namespace FlashCards.Data.Daos.Interfaces;

internal interface IStudySessionAnswerDAO
{
    void Store(StudySessionAnswerStoreDTO studySessionAnswerStoreDTO);
    void Delete(int id);
    void DeleteByCardId(int cardId);
    void DeleteByStudySessionId(int studySessionId);
}
