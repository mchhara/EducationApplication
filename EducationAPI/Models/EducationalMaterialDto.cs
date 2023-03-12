using EducationAPI.Entities;

namespace EducationAPI.Models
{
    public class EducationalMaterialDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Assignment> Assignments { get; set; }

    }
}
