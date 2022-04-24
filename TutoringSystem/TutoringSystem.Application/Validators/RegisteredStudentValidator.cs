using FluentValidation;
using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RegisteredStudentValidator : AbstractValidator<RegisteredStudentDto>
    {
        private readonly IContactRepository contactRepository;

        public RegisteredStudentValidator(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;

            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Password).Matches(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            RuleFor(u => u.Password).Equal(u => u.ConfirmPassword);

            ValidateEmailExistence();
        }

        private void ValidateEmailExistence()
        {
            RuleFor(u => u.Email).Custom((value, context) =>
            {
                if (!contactRepository.IsContactExist(c => c.Email == value))
                {
                    context.AddFailure("email", "That email is taken");
                }
            });
        }
    }
}