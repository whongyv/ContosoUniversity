using ContosoUniversity.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI.Data
{
    public class SchoolContext(DbContextOptions<SchoolContext> options) : DbContext(options)
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Person>()
                .Property(p => p.FirstMidName)
                .HasColumnName("FirstName")
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Course>()
                .Property(c => c.CourseID)
                .ValueGeneratedNever();
            modelBuilder.Entity<Course>()
               .Property(c => c.Title)
               .HasMaxLength(50);
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses);

            modelBuilder.Entity<Department>()
               .Property(c => c.Name)
               .HasMaxLength(50);
            modelBuilder.Entity<Department>()
               .Property(c => c.Budget)
               .HasColumnType("money");
            modelBuilder.Entity<Department>()
               .Property(c => c.ConcurrencyToken)
               .IsConcurrencyToken();

            modelBuilder.Entity<OfficeAssignment>()
                .HasKey(o => o.InstructorID);
            modelBuilder.Entity<OfficeAssignment>()
              .Property(c => c.Location)
              .HasMaxLength(50);
        }
    }
}
