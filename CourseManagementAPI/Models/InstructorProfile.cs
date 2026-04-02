namespace CourseManagementAPI.Models;

public class InstructorProfile
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string OfficeLocation { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    
    // One-to-One relationship (inverse)
    public int InstructorId { get; set; }
    public Instructor? Instructor { get; set; }
}
