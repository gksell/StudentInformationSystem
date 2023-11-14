using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<UserRole> GetOrCreateRoleAsync(string roleName);
        Task<UserRole> GetByRoleAsync(int roleId);
    }
}
