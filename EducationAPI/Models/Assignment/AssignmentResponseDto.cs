using EducationAPI.Entities;

namespace EducationAPI.Models.Assignment
{
    public class AssignmentResponseDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int EducationalSubjectId { get; set; }
    }
}
