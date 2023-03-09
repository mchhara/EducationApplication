namespace EducationAPI.Entities
{
    public class AssignmentResult
    {
        public int Id { get; set; }
        public int Grade { get; set; }

        public Assignment Assignment { get; set; }
        public int AssignmentId { get; set; }
    }
}
