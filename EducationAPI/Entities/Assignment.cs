namespace EducationAPI.Entities
{
    public class Assignment
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public AssignmentResult Result { get; set; }
        public List<User> AssignToUsers { get; set; }

    }
}
