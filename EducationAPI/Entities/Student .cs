namespace EducationAPI.Entities
{
    public class Student : User
    {
        public List<Assignment> Assignments { get; set; }
    }
}
