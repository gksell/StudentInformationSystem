using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Context;
using StudentInformationSystem.Persistence.Interfaces.Repository;
using StudentInformationSystem.Persistence.Interfaces.Repository.CourseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Persistence.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
