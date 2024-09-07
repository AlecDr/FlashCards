using FlashCards.Data.Dtos.StudySession;

namespace FlashCards.Data.Daos.Interfaces;

internal interface IStudySessionDAO
{
    StudySessionShowDTO? Store(StudySessionStoreDTO studySessionStoreDTO);
    List<StudySessionShowDTO> FindByStackId(int stackId);
    StudySessionShowDTO? Find(int id);
    bool Update(StudySessionUpdateDTO studySessionUpdateDTO);
    void DeleteByStackId(int stackId);
    bool Delete(int id);

}
