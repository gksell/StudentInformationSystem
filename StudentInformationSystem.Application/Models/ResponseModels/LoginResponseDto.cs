using StudentInformationSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Models.ResponseModels
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
    }
}
