﻿using AutoMapper;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;
using StudentInformationSystem.Core.Results;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.TeacherRepository;

namespace StudentInformationSystem.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddTeacherAsync(TeacherDto teacherDto)
        {
            var teacherEntity = _mapper.Map<Teacher>(teacherDto);
            await _teacherRepository.AddAsync(teacherEntity);
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var existingTeacherEntity = await _teacherRepository.GetByIdAsync(id);
            if (existingTeacherEntity == null)
            {
                return;
            }
            // Global alınacak burası. 
            existingTeacherEntity.DeletedDate = DateTime.Now;
            existingTeacherEntity.IsDeleted = true;
            TeacherDto deletedTeacherDto = new TeacherDto();

            _mapper.Map(deletedTeacherDto, existingTeacherEntity);

            await _teacherRepository.UpdateAsync(existingTeacherEntity);
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeacherAsync()
        {
            var teacherEntity = await _teacherRepository.GetAllFilterAsync(x => !x.IsDeleted);
            var teacherDto = _mapper.Map<IEnumerable<TeacherDto>>(teacherEntity);
            return teacherDto;
        }

        public async Task<TeacherDto> GetTeacherByIdAsync(int id)
        {
            var teacherEntity = await _teacherRepository.GetByIdAsync(id);
            var teacherDto = _mapper.Map<TeacherDto>(teacherEntity);
            return teacherDto;
        }

        public async Task<TeacherDto> GetTeacherByUserIdAsync(int userId)
        {
            var teacherEntity = await _teacherRepository.GetFilterAsync(x => x.UserId == userId);
            var teacherDto = _mapper.Map<TeacherDto>(teacherEntity);
            return teacherDto;
        }

        public async Task UpdateTeacherAsync(int id, TeacherDto teacherDto)
        {
            var existingTeacherEntity = await _teacherRepository.GetByIdAsync(id);

            if (existingTeacherEntity == null)
            {
                return;
            }

            _mapper.Map(teacherDto, existingTeacherEntity);

            await _teacherRepository.UpdateAsync(existingTeacherEntity);
        }

        /// <summary>
        /// Id ile Teacher var mı kontrolü.
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<bool> TeacherExists(int teacherId)
        {
            var teacherDto = await _teacherRepository.GetByIdAsync(teacherId);
            return teacherDto != null;
        }

        /// <summary>
        /// TeacherId ile ilgili öğretmenin sınıflarını dönen metod. 
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
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

