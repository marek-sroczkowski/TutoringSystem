using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text.RegularExpressions;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Extensions;
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

            RuleFor(p => p.OldPassword).Custom((value, context) =>
            {
                if(passwordHasher.VerifyHashedPassword(user, user.PasswordHash, value) != PasswordVerificationResult.Success)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.InvalidOldPassword.ToString());
                }
            });

            RuleFor(p => p).Custom((value, context) =>
            {
                if (value.NewPassword != value.ConfirmPassword)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.PasswordsVary.ToString());
                }
            });

            RuleFor(p => p.NewPassword).Custom((value, context) =>
            {
                if (!Regex.IsMatch(value, @"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$"))
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.NotMeetRequirements.ToString());
                }
            });

            RuleFor(p => p.NewPassword).Custom((value, context) =>
            {
                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, value) == PasswordVerificationResult.Success)
                {
                    context.AddFailure("passwordErrors", WrongPasswordStatus.DuplicateOfOld.ToString());
                }
            });
        }
    }
}