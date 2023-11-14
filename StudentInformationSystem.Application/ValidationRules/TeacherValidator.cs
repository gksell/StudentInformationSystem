using FluentValidation;
using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.ValidationRules
{
    public class TeacherValidator : AbstractValidator<Teacher>
    {
        public TeacherValidator()
        {
            RuleFor(teacher => teacher.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş olamaz.")
                .MinimumLength(3).WithMessage("Ad alanı en az üç karakter olmalıdır.");
            RuleFor(teacher => teacher.LastName)
                .NotEmpty().WithMessage("Soyadı alanı boş olamaz.")
                .MinimumLength(2).WithMessage("Soyadı alanı en az iki karakter olmalıdır.");
            RuleFor(teacher => teacher.BirthDate)
                .NotEmpty().WithMessage("Doğum Tarihi alanı boş olamaz.")
                .Must(BeAValidDate).WithMessage("Doğum tarihi geçerli bir tarih olmalıdır.");

        }

        private bool BeAValidDate(DateTime birthDate)
        {
            return birthDate != default(DateTime);
        }
    }
}
