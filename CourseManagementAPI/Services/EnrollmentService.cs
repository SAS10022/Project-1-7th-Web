using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public interface IEnrollmentService
{
    Task<EnrollmentReadDto?> GetEnrollmentByIdAsync(int id);
    Task<List<EnrollmentReadDto>> GetEnrollmentsByStudentAsync(int studentId);
    Task<List<EnrollmentReadDto>> GetEnrollmentsByCourseAsync(int courseId);
    Task<EnrollmentReadDto?> CreateEnrollmentAsync(EnrollmentCreateDto createDto);
    Task<bool> UpdateEnrollmentAsync(int id, EnrollmentUpdateDto updateDto);
    Task<bool> DeleteEnrollmentAsync(int id);
}

public class EnrollmentService : IEnrollmentService
{
    private readonly ApplicationDbContext _context;

    public EnrollmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EnrollmentReadDto?> GetEnrollmentByIdAsync(int id)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EnrollmentReadDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = $"{e.Student!.FirstName} {e.Student.LastName}",
                CourseId = e.CourseId,
                CourseCode = e.Course!.CourseCode,
                CourseName = e.Course.Title,
                Grade = e.Grade,
                EnrollmentDate = e.EnrollmentDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<EnrollmentReadDto>> GetEnrollmentsByStudentAsync(int studentId)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .Select(e => new EnrollmentReadDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = $"{e.Student!.FirstName} {e.Student.LastName}",
                CourseId = e.CourseId,
                CourseCode = e.Course!.CourseCode,
                CourseName = e.Course.Title,
                Grade = e.Grade,
                EnrollmentDate = e.EnrollmentDate
            })
            .OrderBy(e => e.CourseName)
            .ToListAsync();
    }

    public async Task<List<EnrollmentReadDto>> GetEnrollmentsByCourseAsync(int courseId)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .Select(e => new EnrollmentReadDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = $"{e.Student!.FirstName} {e.Student.LastName}",
                CourseId = e.CourseId,
                CourseCode = e.Course!.CourseCode,
                CourseName = e.Course.Title,
                Grade = e.Grade,
                EnrollmentDate = e.EnrollmentDate
            })
            .OrderBy(e => e.StudentName)
            .ToListAsync();
    }

    public async Task<EnrollmentReadDto?> CreateEnrollmentAsync(EnrollmentCreateDto createDto)
    {
        // Check if course has available capacity
        var course = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == createDto.CourseId);

        if (course == null)
            return null;

        var enrolledCount = await _context.Enrollments
            .AsNoTracking()
            .CountAsync(e => e.CourseId == createDto.CourseId);

        if (enrolledCount >= course.MaxCapacity)
            return null;

        // Check if student is already enrolled
        var existingEnrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == createDto.StudentId && e.CourseId == createDto.CourseId);

        if (existingEnrollment != null)
            return null;

        var enrollment = new Enrollment
        {
            StudentId = createDto.StudentId,
            CourseId = createDto.CourseId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == createDto.StudentId);

        return new EnrollmentReadDto
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            StudentName = student != null ? $"{student.FirstName} {student.LastName}" : string.Empty,
            CourseId = enrollment.CourseId,
            CourseCode = course.CourseCode,
            CourseName = course.Title,
            Grade = enrollment.Grade,
            EnrollmentDate = enrollment.EnrollmentDate
        };
    }

    public async Task<bool> UpdateEnrollmentAsync(int id, EnrollmentUpdateDto updateDto)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
            return false;

        if (!string.IsNullOrEmpty(updateDto.Grade))
            enrollment.Grade = updateDto.Grade;

        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
            return false;

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
        return true;
    }
}
