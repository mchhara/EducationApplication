namespace EducationAPI.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; } 
        public EducationalSubject EducationalSubject { get; set; }
        public int EducationalSubjectId { get; set; }
        public List<AssignmentUser> AssignmentUsers { get; set; }
    }
}
