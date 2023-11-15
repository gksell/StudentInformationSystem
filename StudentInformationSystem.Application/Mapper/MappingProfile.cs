using AutoMapper;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<RegisterUserDto, RegisterRequestModel>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<UserLoginDto, User>().ReverseMap();
            CreateMap<UserResponseDto, RegisterRequestModel>().ReverseMap();
            CreateMap<CourseRequestModel, CourseDto>().ReverseMap();
            CreateMap<StudentCourseRequestModel, StudentCourseDto>().ReverseMap();
            CreateMap<StudentCourseDto, StudentCourse>().ReverseMap();
            CreateMap<Note, NoteDto>().ReverseMap();
            CreateMap<NoteRequestModel, Note>().ReverseMap();
        }
    }
}
