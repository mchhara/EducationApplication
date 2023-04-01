using EducationAPI.Models.Assignment;
using EducationAPI.Models.EducationalSubjectDto;

namespace EducationAPI.Services.EducationalSubject
{
    public interface IEducationalSubjectServices
    {
        EducationalSubjectDtoResponse GetById(int id);
        IEnumerable<EducationalSubjectDtoResponse> GetAll();
        int Create(EducationalSubjectDto dto);
        bool Delete(int id);
        bool Update(EducationalSubjectDto dto, int id);
        AssignmentResponseDto AddAssigmentToSubject(AssignmentDto dto, int projectId);
        bool DeleteAssignmentFromEducationalSubject(int subjectId,int taskId);
        bool DeleteStudentFromEducationSubject(int subjectId, int studentId);
    }
}
