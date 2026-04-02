using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public interface IInstructorService
{
    Task<InstructorReadDto?> GetInstructorByIdAsync(int id);
    Task<List<InstructorReadDto>> GetAllInstructorsAsync();
    Task<InstructorReadDto> CreateInstructorAsync(InstructorCreateDto createDto);
    Task<bool> UpdateInstructorAsync(int id, InstructorUpdateDto updateDto);
    Task<bool> DeleteInstructorAsync(int id);
}

public class InstructorService : IInstructorService
{
    private readonly ApplicationDbContext _context;

    public InstructorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InstructorReadDto?> GetInstructorByIdAsync(int id)
    {
        return await _context.Instructors
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => new InstructorReadDto
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Department = i.Department,
                CourseCount = i.Courses.Count,
                CreatedAt = i.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<InstructorReadDto>> GetAllInstructorsAsync()
    {
        return await _context.Instructors
            .AsNoTracking()
            .Select(i => new InstructorReadDto
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Department = i.Department,
                CourseCount = i.Courses.Count,
                CreatedAt = i.CreatedAt
            })
            .OrderBy(i => i.LastName)
            .ToListAsync();
    }

    public async Task<InstructorReadDto> CreateInstructorAsync(InstructorCreateDto createDto)
    {
        var instructor = new Instructor
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            Department = createDto.Department
        };

        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        return new InstructorReadDto
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Email = instructor.Email,
            Department = instructor.Department,
            CourseCount = 0,
            CreatedAt = instructor.CreatedAt
        };
    }

    public async Task<bool> UpdateInstructorAsync(int id, InstructorUpdateDto updateDto)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor == null)
            return false;

        instructor.FirstName = updateDto.FirstName;
        instructor.LastName = updateDto.LastName;
        if (!string.IsNullOrEmpty(updateDto.Email))
            instructor.Email = updateDto.Email;
        if (!string.IsNullOrEmpty(updateDto.Department))
            instructor.Department = updateDto.Department;

        _context.Instructors.Update(instructor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteInstructorAsync(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor == null)
            return false;

        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync();
        return true;
    }
}
