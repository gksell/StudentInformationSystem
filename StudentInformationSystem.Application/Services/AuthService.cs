using AutoMapper;
using Newtonsoft.Json.Linq;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.JWT;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;
using StudentInformationSystem.Core.Results;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.UserRepository;

namespace StudentInformationSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleService _userRoleService;
        private readonly IJwtService _jwtService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IMapper _mapper;
        public AuthService(IUserRepository userRepository, IUserRoleService userRoleService, IJwtService jwtService, IStudentService studentService, ITeacherService teacherService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userRoleService = userRoleService;
            _jwtService = jwtService;
            _studentService = studentService;
            _teacherService = teacherService;
            _mapper = mapper;
        }
        // TODO : Yapı parçalanabilir. Solide aykırı yaklaşım
        public async Task<DataResult<UserResponseDto>> RegisterUserAsync(RegisterRequestModel model)
        {
            var userRegisterDto = _mapper.Map<RegisterUserDto>(model);
            if (await CheckUserExist(model.Email))
            {
                var newUser = new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    UserRole = await _userRoleService.GetOrCreateRoleAsync(model.RoleName)
                };

                await _userRepository.AddAsync(newUser);

                User addedUser = await _userRepository.GetFilterAsync(x => x.Email == newUser.Email,
                                                                      x => x.UserRole);
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
                        UserId = addedUser.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate
                    };

                    await _teacherService.AddTeacherAsync(teacherDto);
                }
                var userRegisterResponseDto = _mapper.Map<UserResponseDto>(addedUser);
                return new DataResult<UserResponseDto>(ResultStatus.Success, userRegisterResponseDto);
            }
            else
                return new DataResult<UserResponseDto>(ResultStatus.Error, "Hata",null) ;
        }

        // TODO : Bu metod JWT Service alınacak.
        public async Task<DataResult<string>> GenerateJwtTokenAsync(UserResponseDto userResponseDto)
        {
            var user = _mapper.Map<User>(userResponseDto);
            var token = _jwtService.GenerateToken(user);

            return new DataResult<string>(ResultStatus.Success,token);
        }
        public async Task<DataResult<UserResponseDto>> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetFilterAsync(x => x.Email == email);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                return new DataResult<UserResponseDto>(ResultStatus.Error, "Validasyon hatalı.",null);
            }
            // TODO : Include yapısı kurulacak. Bu şekilde alınmaması gerekli alt nesnelerin.

            user.UserRole = await _userRoleService.GetByRoleAsync(user.UserRoleId);

            var userValidate = _mapper.Map<UserResponseDto>(user);

            return new DataResult<UserResponseDto>(ResultStatus.Success, userValidate);
        }
        /// <summary>
        /// Email daha önce kaydedilmiş mi kontrolü yapılıyor. 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserExist(string email)
        {
            var user = await _userRepository.GetFilterAsync(x => x.Email.Equals(email));
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
