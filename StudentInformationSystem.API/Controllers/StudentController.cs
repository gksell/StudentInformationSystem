using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Models.ResponseModels;
using StudentInformationSystem.Application.Services.Interfaces;

namespace StudentInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        // TODO : CancelationToken Ekle
        // TODO : Endpoint düzenlemeleri yapılacak. Rest uygun olmalı.
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            var responseModel = new StudentResponseModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName
            };

            return Ok(responseModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();

            var responseModels = new List<StudentResponseModel>();

            foreach (var student in students)
            {
                var responseModel = new StudentResponseModel
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                };

                responseModels.Add(responseModel);
            }

            return Ok(responseModels);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentRequestModel requestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var student = new StudentDto
                {
                    FirstName = requestModel.FirstName,
                    LastName = requestModel.LastName,
                    BirthDate = requestModel.BirthDate
                };

                await _studentService.AddStudentAsync(student);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingStudent = await _studentService.GetStudentByIdAsync(id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.FirstName = requestModel.FirstName;
            existingStudent.LastName = requestModel.LastName;
            await _studentService.UpdateStudentAsync(id, existingStudent);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return Ok();
        }
    }
}
