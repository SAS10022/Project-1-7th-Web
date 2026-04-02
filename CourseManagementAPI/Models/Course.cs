namespace CourseManagementAPI.Models;

public class Course
{
    public int Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int MaxCapacity { get; set; }
    
    // One-to-Many relationship (inverse)
    public int InstructorId { get; set; }
    public Instructor? Instructor { get; set; }
    
    // Many-to-Many relationship (inverse)
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
