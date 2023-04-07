using EducationAPI.Models.Assignment;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Models.User;

namespace EducationAPI.Services.EducationalSubject
{
    public interface IEducationalSubjectServices
    {
        EducationalSubjectDtoResponse GetById(int id);
        IEnumerable<EducationalSubjectDtoResponse> GetAll();
        int Create(EducationalSubjectDto dto);
        bool Delete(int id);
        bool UpdateEducationalSubject(EducationalSubjectDto dto, int id);
        AssignmentResponseDto AddAssigmentToSubject(AssignmentDto dto, int projectId);
        bool DeleteAssignmentFromEducationalSubject(int taskId);
        bool DeleteStudentFromEducationSubject(int subjectId, int studentId);
        bool EditAssignment(int assignmentId, AssignmentDto dto);
        bool AddStudentToEducationalSubject(int subjectId, int studentId);
        bool AddStudentToAssignment(int assignmentId, int studentId);
        IEnumerable<UserGradeResult> GetUsersGrades();
        IEnumerable<UserGradeResult> GetUserGrades(int userId);
        bool AddUserGradeToAssignment(int assignmentId, int userId, int gradeValue);
        bool DeleteUserGradeToAssignment(int assignmentId, int studentId);
        IEnumerable<EducationalSubjectDtoResponse> GetAllUserSubjects(int studentId);
    }
}
