using EducationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducationAPI.Models.Assignment
{
    public class AssignmentDto
    {
        [Required]
        [MaxLength(25)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        [Required]
        public int EducationalSubjectId { get; set; }
    }
}
