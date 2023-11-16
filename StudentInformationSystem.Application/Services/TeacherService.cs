﻿using AutoMapper;
using FluentValidation;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Application.ValidationRules;
using StudentInformationSystem.Core.Enums;
using StudentInformationSystem.Core.Results;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.TeacherRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;
        //private readonly IValidator<Teacher> _teacherValidator;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
            //_teacherValidator = validator;
        }

        public async Task AddTeacherAsync(TeacherDto teacherDto)
        {
            var teacherEntity = _mapper.Map<Teacher>(teacherDto);
            //var validationResult = _teacherValidator.Validate(teacherEntity);
            //await (validationResult.IsValid ? _teacherRepository.AddAsync(teacherEntity) : Task.FromException(new ApplicationException(validationResult.Errors.ToString())));
            await _teacherRepository.AddAsync(teacherEntity);
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var existingTeacherEntity = await _teacherRepository.GetByIdAsync(id);
            if (existingTeacherEntity == null)
            {
                // TODO : Hata işlemleri.
                return;
            }
            existingTeacherEntity.DeletedDate = DateTime.Now;
            existingTeacherEntity.IsDeleted = true;
            TeacherDto deletedTeacherDto = new TeacherDto();

            _mapper.Map(deletedTeacherDto, existingTeacherEntity);

            await _teacherRepository.UpdateAsync(existingTeacherEntity);
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeacherAsync()
        {
            // TODO: Mükerrer Kaydı ekleme
            var teacherEntity = await _teacherRepository.GetAllAsync();
            var teacherDto = _mapper.Map<IEnumerable<TeacherDto>>(teacherEntity);
            return teacherDto;
        }

        public async Task<TeacherDto> GetTeacherByIdAsync(int id)
        {
            var teacherEntity = await _teacherRepository.GetByIdAsync(id);
            var teacherDto = _mapper.Map<TeacherDto>(teacherEntity);
            return teacherDto;
        }

        public async Task UpdateTeacherAsync(int id, TeacherDto teacherDto)
        {
            var existingTeacherEntity = await _teacherRepository.GetByIdAsync(id);

            if (existingTeacherEntity == null)
            {
                // TODO : Hata işlemleri.
                return;
            }

            _mapper.Map(teacherDto, existingTeacherEntity);

            await _teacherRepository.UpdateAsync(existingTeacherEntity);
        }

        public async Task<bool> TeacherExists(int teacherId)
        {
            var teacherDto = await _teacherRepository.GetByIdAsync(teacherId);
            return teacherDto != null;
        }

        public async Task<IDataResult<List<CourseDto>>> GetClassesByTeacherIdAsync(int teacherId)
        {
            try
            {
                var teacher = await _teacherRepository.GetFilterAsync(x=>x.Id == teacherId,
                                                                      x=>x.Courses);

                if (teacher == null)
                {
                    return new DataResult<List<CourseDto>>(ResultStatus.Error, "Öğretmen bulunamadı.", null);
                }

                var courseDto = teacher.Courses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    CourseName = c.CourseName,
                    TeacherId = c.TeacherId
                }).ToList();

                return new DataResult<List<CourseDto>>(ResultStatus.Success, courseDto);
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                return new DataResult<List<CourseDto>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }
    }

}

