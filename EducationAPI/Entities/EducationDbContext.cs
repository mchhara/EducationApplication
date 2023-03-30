using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Entities
{
    public class EducationDbContext : DbContext
    {
        private string _connectionString =
            "Server=DESKTOP-ALLUHR9;Database=EducationDb;Trusted_Connection=True;Encrypt=False;";

        public DbSet<EducationalSubject> EducationalSubjects { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentResult> AssignmentsResults { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
