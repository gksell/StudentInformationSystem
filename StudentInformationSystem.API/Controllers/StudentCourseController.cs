﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;

namespace StudentInformationSystem.API.Controllers
{
    [ApiController]
    [Route("api/student-courses")]
    public class StudentCourseController : ControllerBase
    {
        private readonly IStudentCourseService _studentCourseService;

        public StudentCourseController(IStudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }

        [HttpPost("add-student-to-course")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddStudentToCourse([FromBody] StudentCourseRequestModel requestModel)
        {
            try
            {
                var result = await _studentCourseService.AddStudentToCourseAsync(requestModel);

                return result.ResultStatus switch
                {
                    ResultStatus.Success => Ok(result.Data),
                    ResultStatus.Error => BadRequest(new { Message = result.Message }),
                    _ => StatusCode(500, "Internal Server Error")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
