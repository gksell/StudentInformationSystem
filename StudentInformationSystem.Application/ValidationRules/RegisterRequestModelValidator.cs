using FluentValidation;
using StudentInformationSystem.Application.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Application.ValidationRules
{
    public class RegisterRequestModelValidator : AbstractValidator<RegisterRequestModel>
    {
        public RegisterRequestModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
                .Length(6, 10).WithMessage("Şifre en az 6, en fazla 10 karakter olmalıdır.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Rol adı boş olamaz.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş olamaz.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Doğum tarihi boş olamaz.")
                .Must(BeAValidDate).WithMessage("Geçerli bir doğum tarihi giriniz.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
