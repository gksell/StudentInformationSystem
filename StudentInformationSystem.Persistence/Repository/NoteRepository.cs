using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.CourseRepository;
using StudentInformationSystem.Persistence.Interfaces.Repository.NoteRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Persistence.Repository
{
    public class NoteRepository : Repository<Note>, INoteRepository
    {
        public NoteRepository(DbContext context) : base(context)
        {
        }
    }
}
