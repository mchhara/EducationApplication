
namespace EducationAPI.Entities
{
    public class EducationalSubjectUser
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
        public int EducationalSubjectId { get; set; }
        public EducationalSubject EducationalSubject { get; set; }
    }
}
