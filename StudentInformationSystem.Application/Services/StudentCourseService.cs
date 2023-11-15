﻿using AutoMapper;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;
using StudentInformationSystem.Core.Results;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.StudentCourseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;

        public StudentCourseService(IStudentCourseRepository studentCourseRepository, IMapper mapper, ICourseService courseService, IStudentService studentService)
        {
            _studentCourseRepository = studentCourseRepository;
            _mapper = mapper;
            _courseService = courseService;
            _studentService = studentService;
        }

        public async Task<DataResult<StudentCourseDto>> AddStudentToCourseAsync(StudentCourseRequestModel studentCourseRequestModel)
        {
            var studentCourseDto = _mapper.Map<StudentCourseDto>(studentCourseRequestModel);

            if (!await _courseService.CourseExists(studentCourseDto.CourseId))
            {
                return new DataResult<StudentCourseDto>(ResultStatus.Error, "Böyle bir course kaydı yok.", null);
            }

            if (!await _studentService.StudentExists(studentCourseDto.StudentId))
            {
                return new DataResult<StudentCourseDto>(ResultStatus.Error, "Böyle bir öğrenci kaydı yok.", null);
            }

            var studentCourse = _mapper.Map<StudentCourse>(studentCourseDto);
            await _studentCourseRepository.AddAsync(studentCourse);

            return new DataResult<StudentCourseDto>(ResultStatus.Success,studentCourseDto);
        }
    }
}