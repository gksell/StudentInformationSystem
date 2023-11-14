using StudentInformationSystem.Application.JWT;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.RoleRepository;
using StudentInformationSystem.Persistence.Interfaces.Repository.UserRepository;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using StudentInformationSystem.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using StudentInformationSystem.Persistence.Interfaces.Repository.TeacherRepository;

namespace StudentInformationSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleService _userRoleService;
        private readonly IJwtService _jwtService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        public UserService(IUserRepository userRepository, IUserRoleService userRoleService, IJwtService jwtService, IStudentService studentService, ITeacherService teacherService)
        {
            _userRepository = userRepository;
            _userRoleService = userRoleService;
            _jwtService = jwtService;
            _studentService = studentService;
            _teacherService = teacherService;
        }
        public async Task<User> RegisterUserAsync(RegisterUserDto model)
        {
            var isUserExist = CheckUserExist(model.Email);
            if (!isUserExist)
            {
                var newUser = new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    UserRole = await _userRoleService.GetOrCreateRoleAsync(model.RoleName)
                };

                await _userRepository.AddAsync(newUser);

                User addedUser = await _userRepository.GetFilterAsync(x => x.Email == newUser.Email);
                if (addedUser != null)
                    addedUser.UserRole = await _userRoleService.GetByRoleAsync(addedUser.UserRoleId);
                if (addedUser.UserRole.RoleName.Equals("Öğrenci"))
                {
                    StudentDto studentDto = new StudentDto
                    {
                        UserId = addedUser.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate
                    };

                    await _studentService.AddStudentAsync(studentDto);
                }
                else if (addedUser.UserRole.RoleName.Equals("Öğretmen"))
                {
                    TeacherDto teacherDto = new TeacherDto
                    {
                        UserId =addedUser.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate
                    };

                    await _teacherService.AddTeacherAsync(teacherDto);
                }
                return newUser;
            }
            else
                return null;
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var token = _jwtService.GenerateToken(user);

            return token;
        }
        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetFilterAsync(x => x.Email == email);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null;
            }
            user.UserRole = await _userRoleService.GetByRoleAsync(user.UserRoleId);
            return user;
        }
        public bool CheckUserExist(string email)
        {
            var user = _userRepository.GetFilterAsync(x => x.Email.Equals(email));
            bool isUserExist = user == null ? true : false;
            return isUserExist;
        }
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Burada şifre karşılaştırması yapılmalıdır.
            // Gerçek uygulamalarda şifreleri güvenli bir şekilde karşılaştırmak için özel bir algoritma veya kütüphane kullanılmalıdır.
            // Örneğin, BCrypt, Argon2, PBKDF2 gibi algoritmalar kullanılabilir.
            // Bu örnekte basit bir string karşılaştırması yapılıyor, gerçek uygulamada böyle bir kullanım önerilmez.
            return enteredPassword == storedPassword;
        }
    }
}
