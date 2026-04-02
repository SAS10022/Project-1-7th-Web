using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public interface IStudentService
{
    Task<StudentReadDto?> GetStudentByIdAsync(int id);
    Task<List<StudentReadDto>> GetAllStudentsAsync();
    Task<StudentReadDto> CreateStudentAsync(StudentCreateDto createDto);
    Task<bool> UpdateStudentAsync(int id, StudentUpdateDto updateDto);
    Task<bool> DeleteStudentAsync(int id);
}

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentReadDto?> GetStudentByIdAsync(int id)
    {
        return await _context.Students
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new StudentReadDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                StudentId = s.StudentId,
                Major = s.Major,
                EnrolledCourseCount = s.Enrollments.Count,
                CreatedAt = s.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<StudentReadDto>> GetAllStudentsAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .Select(s => new StudentReadDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                StudentId = s.StudentId,
                Major = s.Major,
                EnrolledCourseCount = s.Enrollments.Count,
                CreatedAt = s.CreatedAt
            })
            .OrderBy(s => s.LastName)
            .ToListAsync();
    }

    public async Task<StudentReadDto> CreateStudentAsync(StudentCreateDto createDto)
    {
        var student = new Student
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            StudentId = createDto.StudentId,
            Major = createDto.Major
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return new StudentReadDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            StudentId = student.StudentId,
            Major = student.Major,
            EnrolledCourseCount = 0,
            CreatedAt = student.CreatedAt
        };
    }

    public async Task<bool> UpdateStudentAsync(int id, StudentUpdateDto updateDto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return false;

        if (!string.IsNullOrEmpty(updateDto.FirstName))
            student.FirstName = updateDto.FirstName;
        if (!string.IsNullOrEmpty(updateDto.LastName))
            student.LastName = updateDto.LastName;
        if (!string.IsNullOrEmpty(updateDto.Email))
            student.Email = updateDto.Email;
        if (!string.IsNullOrEmpty(updateDto.Major))
            student.Major = updateDto.Major;

        _context.Students.Update(student);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return false;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
}
