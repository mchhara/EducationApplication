using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Entities
{
    public class EducationDbContext : DbContext
    {
        private string _connectionString =
            "Server=DESKTOP-ALLUHR9;Database=EducationDb;Trusted_Connection=True;Encrypt=False;";

        public DbSet<EducationalSubject> EducationalSubjects { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentUser> AssignmentUsers { get; set; }
        public DbSet<AssignmentResult> AssignmentResults { get; set; }
        public DbSet<EducationalSubjectUser> EducationalSubjectUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
