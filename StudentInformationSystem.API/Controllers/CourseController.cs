using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Models.ResponseModels;
using StudentInformationSystem.Application.Services;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;

namespace StudentInformationSystem.API.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourse([FromBody] CourseRequestModel courseRequestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var addCourseResult = await _courseService.AddCourseAsync(courseRequestModel);

                return addCourseResult.ResultStatus switch
                {
                    ResultStatus.Success => Ok(addCourseResult.Data),
                    ResultStatus.Error => BadRequest(new { Message = addCourseResult.Message }),
                    _ => StatusCode(500, "Internal Server Error")
                };
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var students = await _courseService.GetAllCoursesAsync();

            return Ok(students.ToList());
        }
    }
}
