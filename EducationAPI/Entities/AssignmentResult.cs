namespace EducationAPI.Entities
{
    public class AssignmentResult
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public string Result { get; set; }
        public AssignmentUser AssignmentUser { get; set; }
        public int AssignmentUserId { get; set; }

    }
}
