using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.JWT
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        List<string> GetRolesFromToken(string jwtToken);
    }
}
