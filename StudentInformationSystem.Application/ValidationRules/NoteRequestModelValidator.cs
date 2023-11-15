using FluentValidation;
using StudentInformationSystem.Application.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.ValidationRules
{
    public class NoteRequestModelValidator : AbstractValidator<NoteRequestModel>
    {
        public NoteRequestModelValidator()
        {
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("StudentId boş olamaz.");
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("CourseId boş olamaz.");
            RuleFor(x => x.Grade).NotEmpty().WithMessage("Grade boş olamaz.")
                                 .GreaterThanOrEqualTo(0).WithMessage("Grade 0'dan küçük olamaz.")
                                 .LessThanOrEqualTo(100).WithMessage("Grade 100'den büyük olamaz.");
        }
    }
}
