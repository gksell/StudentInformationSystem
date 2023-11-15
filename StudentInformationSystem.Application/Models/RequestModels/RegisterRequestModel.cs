﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RoleName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
