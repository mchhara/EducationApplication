using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Entities
{
    public class EducationDbContext : DbContext
    {
        private string _connectionString =
            "Server=DESKTOP-ALLUHR9;Database=EducationDb;Trusted_Connection=True;Encrypt=False;";

        public DbSet<EducationalMaterial> EducationalMaterials { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Administrator> Administrators { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EducationalMaterial>()
                .Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Assignment>()
                .Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Assignment>()
                .Property(r => r.Description)
                .IsRequired();
        }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
