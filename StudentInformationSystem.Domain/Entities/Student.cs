﻿using StudentInformationSystem.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<StudentCourse> Courses { get; set; } = new List<StudentCourse>();
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}
