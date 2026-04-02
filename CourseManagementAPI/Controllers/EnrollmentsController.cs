using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEnrollment(int id)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
        if (enrollment == null)
            return NotFound();

        return Ok(enrollment);
    }

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetEnrollmentsByStudent(int studentId)
    {
        var enrollments = await _enrollmentService.GetEnrollmentsByStudentAsync(studentId);
        return Ok(enrollments);
    }

    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetEnrollmentsByCourse(int courseId)
    {
        var enrollments = await _enrollmentService.GetEnrollmentsByCourseAsync(courseId);
        return Ok(enrollments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor,User")]
    public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var enrollment = await _enrollmentService.CreateEnrollmentAsync(createDto);
        if (enrollment == null)
            return BadRequest("Cannot enroll: Course may be full or student already enrolled");

        return Created($"api/enrollments/{enrollment.Id}", enrollment);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] EnrollmentUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _enrollmentService.UpdateEnrollmentAsync(id, updateDto);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var success = await _enrollmentService.DeleteEnrollmentAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
