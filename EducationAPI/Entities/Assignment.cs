namespace EducationAPI.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime Deadline { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
