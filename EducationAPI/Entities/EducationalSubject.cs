namespace EducationAPI.Entities
{
    public class EducationalSubject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<User> Students { get; set; }
        
       }

}
