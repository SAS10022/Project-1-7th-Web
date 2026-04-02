namespace CourseManagementAPI.Models;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    
    // One-to-Many relationship (inverse)
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
