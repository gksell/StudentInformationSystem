using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services.Interfaces
{
    public interface IStudentCourseService
    {
        Task<DataResult<StudentCourseDto>> AddStudentToCourseAsync(StudentCourseRequestModel requestModel);
    }
}
