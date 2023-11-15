using FluentValidation;
using StudentInformationSystem.Application.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.ValidationRules
{
    public class CourseRequestModelValidator : AbstractValidator<CourseRequestModel>
    {
        public CourseRequestModelValidator()
        {
            RuleFor(x => x.CourseName)
                .NotEmpty().WithMessage("Ders adı boş olamaz.");

            RuleFor(x => x.TeacherId)
                .NotEmpty().WithMessage("Öğretmen ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen ID giriniz.");
        }
    }
}
