using StudentInformationSystem.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
