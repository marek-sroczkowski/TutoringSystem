using FluentValidation;
using System.Text.RegularExpressions;
using TutoringSystem.Application.Dtos.Account;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class PasswordResetValidator : AbstractValidator<NewPasswordDto>
    {
        private readonly IContactRepository contactRepository;
        private readonly IPasswordResetCodeRepository passwordResetCodeRepository;

        public PasswordResetValidator(IContactRepository contactRepository, IPasswordResetCodeRepository passwordResetCodeRepository)
        {
            this.contactRepository = contactRepository;
            this.passwordResetCodeRepository = passwordResetCodeRepository;

            ValidateEmailAndCode();
            ValidatePasswordsEquality();
            ValidatePasswordsRequirements();
        }

        private void ValidatePasswordsRequirements()
        {
            RuleFor(p => p.Password).Custom((value, context) =>
            {
                if (!Regex.IsMatch(value, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$"))
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.NotMeetRequirements.ToString());
                }
            });
        }

        private void ValidatePasswordsEquality()
        {
            RuleFor(p => p).Custom((value, context) =>
            {
                if (value.Password != value.ConfirmPassword)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.PasswordsVary.ToString());
                }
            });
        }

        private void ValidateEmailAndCode()
        {
            RuleFor(p => p).Custom(async (value, context) =>
            {
                var contact = await contactRepository.GetContactAsync(c => c.Email == value.Email);
                if (contact is null)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.InvalidEmail.ToString());
                    return;
                }

                if (!passwordResetCodeRepository.CodeExists(c => c.Code == value.ResetCode && c.Email == value.Email && c.UserId == contact.UserId && c.IsActive))
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.InvalidResetCode.ToString());
                }
            });
        }
    }
}