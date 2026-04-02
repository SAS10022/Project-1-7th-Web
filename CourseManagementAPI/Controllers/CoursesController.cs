using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
            return NotFound();

        return Ok(course);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("instructor/{instructorId}")]
    public async Task<IActionResult> GetCoursesByInstructor(int instructorId)
    {
        var courses = await _courseService.GetCoursesByInstructorAsync(instructorId);
        return Ok(courses);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var course = await _courseService.CreateCourseAsync(createDto);
        return Created($"api/courses/{course.Id}", course);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _courseService.UpdateCourseAsync(id, updateDto);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var success = await _courseService.DeleteCourseAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
