using FluentValidation;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RegisteredStudentValidation : AbstractValidator<RegisteredStudentDto>
    {
        public RegisteredStudentValidation(IUserRepository userRepository)
        {
            RuleFor(u => u.Email).Custom(async (value, context) =>
            {
                var user = await userRepository.GetUserAsync(user => user.Contact.Email.Equals(value), isEagerLoadingEnabled: true);
                if (user != null)
                {
                    context.AddFailure("email", "That email is taken");
                }
            });

            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Password).Matches(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            RuleFor(u => u.Password).Equal(u => u.ConfirmPassword);
        }
    }
}