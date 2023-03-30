using EducationAPI.Entities;
using EducationAPI.Models.Assignment;
using EducationAPI.Models.User;

namespace EducationAPI.Models.EducationalSubjectDto
{
    public class EducationalSubjectDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AssignmentDto> Assignments { get; set; }
        public List<UserDto> Students { get; set; }

    }
}
