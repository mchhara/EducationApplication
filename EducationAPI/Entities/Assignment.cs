
namespace EducationAPI.Entities
{
    public class Assignment
    {

        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; } 
        public EducationalSubject EducationalSubject { get; set; }
        public int EducationalSubjectId { get; set; }
        public AssignmentResult? AssignmentResult { get; set; }
        public int? StudentId { get; set; }
        public User Student { get; set; }

    }
}
