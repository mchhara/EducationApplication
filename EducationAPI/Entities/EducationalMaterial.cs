namespace EducationAPI.Entities
{
    public class EducationalMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string FilePath { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }


    }
}
