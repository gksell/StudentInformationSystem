using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Models.ResponseModels;
using StudentInformationSystem.Application.Services;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;
using System.Reflection.Metadata.Ecma335;

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

            if (teacher == null)
            {
                return NotFound();
            }

            var responseModel = new TeacherResponseModel
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                BirthDate = teacher.BirthDate
            };

            return Ok(responseModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeacherAsync();

            var responseModels = new List<TeacherResponseModel>();

            foreach (var teacher in teachers)
            {
                var responseModel = new TeacherResponseModel
                {
                    Id = teacher.Id,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    BirthDate = teacher.BirthDate
                };

                responseModels.Add(responseModel);
            }

            return Ok(responseModels);
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
        public async Task<IActionResult> DeleteStudent(int id)
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
