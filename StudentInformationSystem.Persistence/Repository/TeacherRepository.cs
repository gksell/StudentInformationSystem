using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Context;
using StudentInformationSystem.Persistence.Interfaces.Repository.TeacherRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Persistence.Repository
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
