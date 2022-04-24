using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text.RegularExpressions;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Password;
using TutoringSystem.Application.Models.Enums;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class PasswordChangeValidator : AbstractValidator<PasswordDto>
    {
        public PasswordChangeValidator(IUserRepository userRepository, IHttpContextAccessor httpContext, IPasswordHasher<User> passwordHasher)
        {
            var userId = httpContext.HttpContext.User.GetUserId();
            var user = userRepository.GetUsersCollection(u => u.Id.Equals(userId)).First();

            ValidateOldPassword(passwordHasher, user);
            ValidatePasswordsEquality();
            ValidatePasswordsRequirements();
            ValidatePasswordsDuplications(passwordHasher, user);
        }

        private void ValidatePasswordsDuplications(IPasswordHasher<User> passwordHasher, User user)
        {
            RuleFor(p => p.NewPassword).Custom((value, context) =>
            {
                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, value) == PasswordVerificationResult.Success)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.DuplicateOfOld.ToString());
                }
            });
        }

        private void ValidatePasswordsRequirements()
        {
            RuleFor(p => p.NewPassword).Custom((value, context) =>
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
                if (value.NewPassword != value.ConfirmPassword)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.PasswordsVary.ToString());
                }
            });
        }

        private void ValidateOldPassword(IPasswordHasher<User> passwordHasher, User user)
        {
            RuleFor(p => p.OldPassword).Custom((value, context) =>
            {
                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, value) != PasswordVerificationResult.Success)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.InvalidOldPassword.ToString());
                }
            });
        }
    }
}