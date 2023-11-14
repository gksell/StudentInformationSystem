using StudentInformationSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDto> GetCourseByIdAsync(int id);
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task AddCourseAsync(CourseDto courseDto);
        Task UpdateCourseAsync(int id, CourseDto courseDto);
        Task DeleteCourseAsync(int id);
    }
}
