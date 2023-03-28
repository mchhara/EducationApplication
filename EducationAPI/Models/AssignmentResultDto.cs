using EducationAPI.Entities;

namespace EducationAPI.Models
{
    public class AssignmentResultDto
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public string Result { get; set; }
        public AssignmentDto AssignmentDto { get; set; }
        public int AssignmentDtoId { get; set; }
    }
}
