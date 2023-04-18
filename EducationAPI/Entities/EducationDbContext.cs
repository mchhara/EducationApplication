using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Entities
{
    public class EducationDbContext : DbContext
    {
        public EducationDbContext(DbContextOptions<EducationDbContext> options) : base(options)
        {
            
        }

        public DbSet<EducationalSubject> EducationalSubjects { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentUser> AssignmentUsers { get; set; }
        public DbSet<AssignmentResult> AssignmentResults { get; set; }
        public DbSet<EducationalSubjectUser> EducationalSubjectUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

       
    }
}
