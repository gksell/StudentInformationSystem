using StudentInformationSystem.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public List<StudentCourse> Students { get; set; } = new List<StudentCourse>();
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}
