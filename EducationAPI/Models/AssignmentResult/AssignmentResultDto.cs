using EducationAPI.Entities;

namespace EducationAPI.Models.Assignment
{
    public class AssignmentResultDto
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public string Result { get; set; }
        public int AssignmentDtoId { get; set; }
    }
}
