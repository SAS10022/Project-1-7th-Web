using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs;

public class CourseCreateDto
{
    [Required(ErrorMessage = "Course code is required")]
    [MaxLength(20)]
    public string CourseCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Credits is required")]
    [Range(1, 12, ErrorMessage = "Credits must be between 1 and 12")]
    public int Credits { get; set; }

    [Required(ErrorMessage = "Max capacity is required")]
    [Range(1, 500, ErrorMessage = "Max capacity must be between 1 and 500")]
    public int MaxCapacity { get; set; }

    [Required(ErrorMessage = "Instructor ID is required")]
    public int InstructorId { get; set; }
}

public class CourseUpdateDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(1, 12, ErrorMessage = "Credits must be between 1 and 12")]
    public int? Credits { get; set; }

    [Range(1, 500, ErrorMessage = "Max capacity must be between 1 and 500")]
    public int? MaxCapacity { get; set; }
}

public class CourseReadDto
{
    public int Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int MaxCapacity { get; set; }
    public int EnrolledStudentCount { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
