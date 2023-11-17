using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Models.ResponseModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;

namespace StudentInformationSystem.API.Controllers
{
    [Route("api/teachers")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        // TODO: Endpoint düzenlemeleri yapılacak.
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);

            return teacher != null
                ? Ok(teacher)
                : NotFound("Öğretmen kaydı bulunamadı.");
        }

        [HttpGet("by-user-id/{userId}")]
        public async Task<IActionResult> GetTeacherByUserId(int userId)
        {
            var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);

            return teacher != null
                ? Ok(teacher)
                : NotFound($"User ID'si {userId} ile ilişkilendirilmiş öğretmen kaydı bulunamadı.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeacherAsync();
            return Ok(teachers.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacher = new TeacherDto
            {
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };

            await _teacherService.AddTeacherAsync(teacher);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTeacher = await _teacherService.GetTeacherByIdAsync(id);

            if (existingTeacher == null)
            {
                return NotFound();
            }

            existingTeacher.FirstName = requestModel.FirstName;
            existingTeacher.LastName = requestModel.LastName;
            existingTeacher.BirthDate = requestModel.BirthDate;

            await _teacherService.UpdateTeacherAsync(id, existingTeacher);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            return NoContent();
        }

        [HttpGet("{teacherId}/courses")]
        public async Task<IActionResult> GetCoursesByTeacherId(int teacherId)
        {
            var result = await _teacherService.GetClassesByTeacherIdAsync(teacherId);

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.Error => BadRequest(result.Message),
                _ => StatusCode(500, "Internal Server Error"),
            };
        }
    }
}
