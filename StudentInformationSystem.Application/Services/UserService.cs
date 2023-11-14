using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.JWT;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.UserRepository;

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
        // TODO : Yapı parçalanabilir. Solide aykırı yaklaşım
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

        // TODO : Bu metod JWT Service alınacak.
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
            // TODO : Include yapısı kurulacak. Bu şekilde alınmaması gerekli alt nesnelerin.
            user.UserRole = await _userRoleService.GetByRoleAsync(user.UserRoleId);
            return user;
        }
        /// <summary>
        /// Email daha önce kaydedilmiş mi kontrolü yapılıyor. 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CheckUserExist(string email)
        {
            var user = _userRepository.GetFilterAsync(x => x.Email.Equals(email));
            bool isUserExist = user == null ? true : false;
            return isUserExist;
        }
        // TODO: Detaylı kontrol yapılmalı.
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;
        }
    }
}
