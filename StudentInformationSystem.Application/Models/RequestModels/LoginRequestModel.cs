using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Models.RequestModels
{
    public class LoginRequestModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
