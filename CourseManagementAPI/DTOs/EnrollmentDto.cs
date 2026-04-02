using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs;

public class EnrollmentCreateDto
{
    [Required(ErrorMessage = "Student ID is required")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "Course ID is required")]
    public int CourseId { get; set; }
}

public class EnrollmentUpdateDto
{
    [MaxLength(2, ErrorMessage = "Grade must be at most 2 characters")]
    public string? Grade { get; set; }
}

public class EnrollmentReadDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string? Grade { get; set; }
    public DateTime EnrollmentDate { get; set; }
}
