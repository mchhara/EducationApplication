using System.ComponentModel.DataAnnotations;
using EducationAPI.Entities;

namespace EducationAPI.Models
{
    public class CreateEducationalMaterialDto
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)] 
        public string Description { get; set; }
        [Required]
        public string? SourceUrl { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
