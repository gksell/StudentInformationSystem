using AutoMapper;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;
using StudentInformationSystem.Core.Results;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.CourseRepository;
using StudentInformationSystem.Persistence.Interfaces.Repository.TeacherRepository;
using StudentInformationSystem.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherService _teacherService;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper, ITeacherService teacherService)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _teacherService = teacherService;
        }

        public async Task<IDataResult<CourseDto>> AddCourseAsync(CourseRequestModel courseRequestModel)
        {
            var courseDto = _mapper.Map<CourseDto>(courseRequestModel);

            TeacherDto teacherExist = await _teacherService.GetTeacherByIdAsync(courseDto.TeacherId);
            if (teacherExist != null)
            {
                var courseEntity = _mapper.Map<Course>(courseDto);
                await _courseRepository.AddAsync(courseEntity);
                return new DataResult<CourseDto>(ResultStatus.Success, courseDto);
            }
            else
                return new DataResult<CourseDto>(ResultStatus.Error, "Öğretmen Bilgisi hatalı.", null);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var existingCourseEntity = await _courseRepository.GetByIdAsync(id);
            if (existingCourseEntity == null)
            {
                // TODO : Hata işlemleri.
                return;
            }
            existingCourseEntity.DeletedDate = DateTime.Now;
            existingCourseEntity.IsDeleted = true;
            CourseDto deletedCourseDto = new CourseDto();

            _mapper.Map(deletedCourseDto, existingCourseEntity);

            await _courseRepository.UpdateAsync(existingCourseEntity);
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            // TODO: Mükerrer Kaydı ekleme
            var courseEntity = await _courseRepository.GetAllAsync();
            var courseDto = _mapper.Map<IEnumerable<CourseDto>>(courseEntity);
            return courseDto;
        }

        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var courseEntity = await _courseRepository.GetByIdAsync(id);
            var courseDto = _mapper.Map<CourseDto>(courseEntity);
            return courseDto;
        }

        public async Task UpdateCourseAsync(int id, CourseDto courseDto)
        {
            var existingCourseEntity = await _courseRepository.GetByIdAsync(id);

            if (existingCourseEntity == null)
            {
                return;
            }

            _mapper.Map(courseDto, existingCourseEntity);

            await _courseRepository.UpdateAsync(existingCourseEntity);
        }

        public async Task<bool> CourseExists(int courseId)
        {
            var courseDto = await _courseRepository.GetByIdAsync(courseId);
            return courseDto != null;
        }
    }
}
