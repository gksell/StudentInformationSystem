using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Context;
using StudentInformationSystem.Persistence.Interfaces.Repository.StudentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Persistence.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
