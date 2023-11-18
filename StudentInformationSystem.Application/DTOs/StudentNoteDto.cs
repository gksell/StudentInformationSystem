using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.DTOs
{
    public class StudentNoteDto
    {
        public string CourseName { get; set; }
        public double Grade { get; set; }

        public string TeacherName { get; set; }
    }
}
