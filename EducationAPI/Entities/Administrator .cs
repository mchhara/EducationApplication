namespace EducationAPI.Entities
{
    public class Administrator :User
    {
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public List<Class> Classes { get; set; }
    }
}
