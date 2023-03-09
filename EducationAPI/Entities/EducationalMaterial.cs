namespace EducationAPI.Entities
{
    public class EducationalMaterial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Assignment> Assignments { get; set; }
        
       }

}
