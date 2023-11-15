using AutoMapper;
using FluentValidation;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.StudentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Student> _studentValidator;

        public StudentService(IStudentRepository studentRepository, IMapper mapper, IValidator<Student> studentValidator)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _studentValidator = studentValidator;
        }

        public async Task<StudentDto> GetStudentByIdAsync(int id)
        {
            var studentEntity = await _studentRepository.GetByIdAsync(id);
            var studentDto = _mapper.Map<StudentDto>(studentEntity);
            return studentDto;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var studentsEntity = await _studentRepository.GetAllAsync();
            var studentsDto = _mapper.Map<IEnumerable<StudentDto>>(studentsEntity);
            return studentsDto;
        }

        public async Task AddStudentAsync(StudentDto studentDto)
        {
            var studentEntity = _mapper.Map<Student>(studentDto);

            var validationResult = _studentValidator.Validate(studentEntity);
            await (validationResult.IsValid ? _studentRepository.AddAsync(studentEntity) : Task.FromException(new ApplicationException(validationResult.Errors.ToString())));
        }

        public async Task UpdateStudentAsync(int id, StudentDto studentDto)
        {
            var existingStudentEntity = await _studentRepository.GetByIdAsync(id);

            if (existingStudentEntity == null)
            {
                // Hata işlemleri burada
                return;
            }

            _mapper.Map(studentDto, existingStudentEntity);

            await _studentRepository.UpdateAsync(existingStudentEntity);
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteAsync(id);
        }

        public async Task<bool> StudentExists(int studentId)
        {
            var studentDto = await _studentRepository.GetByIdAsync(studentId);
            return studentDto != null;
        }
    }
}
