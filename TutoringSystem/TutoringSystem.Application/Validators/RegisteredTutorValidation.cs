using FluentValidation;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RegisteredTutorValidation : AbstractValidator<RegisteredTutorDto>
    {
        public RegisteredTutorValidation(IUserRepository userRepository)
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.LastName).NotEmpty();

            RuleFor(u => u.Username).Custom((value, context) =>
            {
                if (userRepository.IsUserExist(user => user.Username.Equals(value)))
                {
                    context.AddFailure("username", "That username is taken");
                }
            });

            RuleFor(u => u.Email).Custom((value, context) =>
            {
                var user = userRepository.GetUserAsync(user => user.Contact.Email.Equals(value), isEagerLoadingEnabled: true).Result;
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