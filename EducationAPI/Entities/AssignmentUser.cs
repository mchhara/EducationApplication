namespace EducationAPI.Entities
{
    public class AssignmentUser
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public AssignmentResult AssignmentResult { get; set; }
        public int AssignmentResultId { get; set; }
    }
}
