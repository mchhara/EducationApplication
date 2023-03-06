using System.Security.Claims;

namespace EducationAPI.Entities
{
    public class Teacher : User
    {
        public List<EducationalMaterial> EducationalMaterials { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
