using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterUserDto model);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<User> ValidateUserAsync(string email, string password);
    }
}
