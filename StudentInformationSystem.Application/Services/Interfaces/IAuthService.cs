using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Core.Results;
using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<DataResult<UserResponseDto>> RegisterUserAsync(RegisterRequestModel model);
        Task<DataResult<string>> GenerateJwtTokenAsync(UserResponseDto registerUserResponseDto);
        Task<DataResult<UserResponseDto>> ValidateUserAsync(string email, string password);
        Task<bool> CheckUserExist(string email);
    }
}
