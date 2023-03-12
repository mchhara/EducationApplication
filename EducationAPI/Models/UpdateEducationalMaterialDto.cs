using EducationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducationAPI.Models
{
    public class UpdateEducationalMaterialDto
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
        
    }
}
