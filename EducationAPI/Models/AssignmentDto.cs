using EducationAPI.Entities;

namespace EducationAPI.Models
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public AssignmentResultDto Result { get; set; }

    }
}
