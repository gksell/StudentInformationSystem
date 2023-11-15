using FluentValidation;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.ValidationRules
{
    public class StudentCourseRequestModelValidator : AbstractValidator<StudentCourseRequestModel>
    {
        public StudentCourseRequestModelValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Öğrenci ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçerli bir öğrenci ID giriniz.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Ders ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçerli bir ders ID giriniz.");
        }
    }
}
