using StudentInformationSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<TeacherDto> GetTeacherByIdAsync(int id);
        Task<IEnumerable<TeacherDto>> GetAllTeacherAsync();
        Task AddTeacherAsync(TeacherDto teacherDto);
        Task UpdateTeacherAsync(int id, TeacherDto teacherDto);
        Task DeleteTeacherAsync(int id);
        Task<bool> TeacherExists(int teacherId);
    }
}
