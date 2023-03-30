using System.ComponentModel.DataAnnotations;
using EducationAPI.Entities;

namespace EducationAPI.Models.EducationalSubjectDto
{
    public class EducationalSubjectDto
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
    }
}
