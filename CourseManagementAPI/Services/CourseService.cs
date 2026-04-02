using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public interface ICourseService
{
    Task<CourseReadDto?> GetCourseByIdAsync(int id);
    Task<List<CourseReadDto>> GetAllCoursesAsync();
    Task<List<CourseReadDto>> GetCoursesByInstructorAsync(int instructorId);
    Task<CourseReadDto> CreateCourseAsync(CourseCreateDto createDto);
    Task<bool> UpdateCourseAsync(int id, CourseUpdateDto updateDto);
    Task<bool> DeleteCourseAsync(int id);
}

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseReadDto?> GetCourseByIdAsync(int id)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseReadDto
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                Title = c.Title,
                Description = c.Description,
                Credits = c.Credits,
                MaxCapacity = c.MaxCapacity,
                EnrolledStudentCount = c.Enrollments.Count,
                InstructorName = $"{c.Instructor!.FirstName} {c.Instructor.LastName}",
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CourseReadDto>> GetAllCoursesAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .Select(c => new CourseReadDto
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                Title = c.Title,
                Description = c.Description,
                Credits = c.Credits,
                MaxCapacity = c.MaxCapacity,
                EnrolledStudentCount = c.Enrollments.Count,
                InstructorName = $"{c.Instructor!.FirstName} {c.Instructor.LastName}",
                CreatedAt = c.CreatedAt
            })
            .OrderBy(c => c.CourseCode)
            .ToListAsync();
    }

    public async Task<List<CourseReadDto>> GetCoursesByInstructorAsync(int instructorId)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.InstructorId == instructorId)
            .Select(c => new CourseReadDto
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                Title = c.Title,
                Description = c.Description,
                Credits = c.Credits,
                MaxCapacity = c.MaxCapacity,
                EnrolledStudentCount = c.Enrollments.Count,
                InstructorName = $"{c.Instructor!.FirstName} {c.Instructor.LastName}",
                CreatedAt = c.CreatedAt
            })
            .OrderBy(c => c.CourseCode)
            .ToListAsync();
    }

    public async Task<CourseReadDto> CreateCourseAsync(CourseCreateDto createDto)
    {
        var course = new Course
        {
            CourseCode = createDto.CourseCode,
            Title = createDto.Title,
            Description = createDto.Description ?? string.Empty,
            Credits = createDto.Credits,
            MaxCapacity = createDto.MaxCapacity,
            InstructorId = createDto.InstructorId
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var instructor = await _context.Instructors
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == createDto.InstructorId);

        return new CourseReadDto
        {
            Id = course.Id,
            CourseCode = course.CourseCode,
            Title = course.Title,
            Description = course.Description,
            Credits = course.Credits,
            MaxCapacity = course.MaxCapacity,
            EnrolledStudentCount = 0,
            InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : string.Empty,
            CreatedAt = course.CreatedAt
        };
    }

    public async Task<bool> UpdateCourseAsync(int id, CourseUpdateDto updateDto)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return false;

        if (!string.IsNullOrEmpty(updateDto.Title))
            course.Title = updateDto.Title;
        if (!string.IsNullOrEmpty(updateDto.Description))
            course.Description = updateDto.Description;
        if (updateDto.Credits.HasValue)
            course.Credits = updateDto.Credits.Value;
        if (updateDto.MaxCapacity.HasValue)
            course.MaxCapacity = updateDto.MaxCapacity.Value;

        course.UpdatedAt = DateTime.UtcNow;
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return false;

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return true;
    }
}
