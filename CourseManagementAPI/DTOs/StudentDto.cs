using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs;

public class StudentCreateDto
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Student ID is required")]
    [MaxLength(20)]
    public string StudentId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Major is required")]
    [MaxLength(100)]
    public string Major { get; set; } = string.Empty;
}

public class StudentUpdateDto
{
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    [MaxLength(100)]
    public string? Major { get; set; }
}

public class StudentReadDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public int EnrolledCourseCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
