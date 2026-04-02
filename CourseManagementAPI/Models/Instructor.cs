namespace CourseManagementAPI.Models;

public class Instructor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    
    // One-to-One relationship with InstructorProfile
    public int? InstructorProfileId { get; set; }
    public InstructorProfile? InstructorProfile { get; set; }
    
    // One-to-Many relationship
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
