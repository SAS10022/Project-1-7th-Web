using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Instructor> Instructors { get; set; } = null!;
    public DbSet<InstructorProfile> InstructorProfiles { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Instructor configuration
        modelBuilder.Entity<Instructor>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Instructor>()
            .HasIndex(i => i.Email)
            .IsUnique();

        // One-to-One: Instructor <-> InstructorProfile
        modelBuilder.Entity<Instructor>()
            .HasOne(i => i.InstructorProfile)
            .WithOne(ip => ip.Instructor)
            .HasForeignKey<InstructorProfile>(ip => ip.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: Instructor -> Course
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student configuration
        modelBuilder.Entity<Student>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Email)
            .IsUnique();
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.StudentId)
            .IsUnique();

        // Course configuration
        modelBuilder.Entity<Course>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Course>()
            .HasIndex(c => c.CourseCode)
            .IsUnique();

        // Many-to-Many: Student <-> Course (through Enrollment)
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint on StudentId + CourseId
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique();
    }
}
