using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInstructor(int id)
    {
        var instructor = await _instructorService.GetInstructorByIdAsync(id);
        if (instructor == null)
            return NotFound();

        return Ok(instructor);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInstructors()
    {
        var instructors = await _instructorService.GetAllInstructorsAsync();
        return Ok(instructors);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateInstructor([FromBody] InstructorCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var instructor = await _instructorService.CreateInstructorAsync(createDto);
        return Created($"api/instructors/{instructor.Id}", instructor);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateInstructor(int id, [FromBody] InstructorUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _instructorService.UpdateInstructorAsync(id, updateDto);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteInstructor(int id)
    {
        var success = await _instructorService.DeleteInstructorAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
