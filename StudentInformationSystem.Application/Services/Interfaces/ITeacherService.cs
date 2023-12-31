﻿using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Core.Results;
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
        Task<IDataResult<List<CourseDto>>> GetClassesByTeacherIdAsync(int teacherId);
        Task<TeacherDto> GetTeacherByUserIdAsync(int userId);
    }
}
