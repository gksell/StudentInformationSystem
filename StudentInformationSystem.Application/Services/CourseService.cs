using AutoMapper;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Services.Interfaces;
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
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task AddCourseAsync(CourseDto courseDto)
        {
            var courseEntity = _mapper.Map<Course>(courseDto);
            await _courseRepository.AddAsync(courseEntity);
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
                // TODO : Hata işlemleri.
                return;
            }

            _mapper.Map(courseDto, existingCourseEntity);

            await _courseRepository.UpdateAsync(existingCourseEntity);
        }
    }
}
