using FluentValidation;
using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RegisteredTutorValidator : AbstractValidator<RegisteredTutorDto>
    {
        private readonly IUserRepository userRepository;

        public RegisteredTutorValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.LastName).NotEmpty();

            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Password).Matches(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            RuleFor(u => u.Password).Equal(u => u.ConfirmPassword);

            ValidateUsernameExistence();
            ValidateEmailExistence();
        }

        private void ValidateEmailExistence()
        {
            RuleFor(u => u.Email).Custom((value, context) =>
            {
                var user = userRepository.GetUserAsync(user => user.Contact.Email.Equals(value), isEagerLoadingEnabled: true).Result;
                if (user != null)
                {
                    context.AddFailure("email", "That email is taken");
                }
            });
        }

        private void ValidateUsernameExistence()
        {
            RuleFor(u => u.Username).Custom((value, context) =>
            {
                if (userRepository.UserExists(user => user.Username.Equals(value)))
                {
                    context.AddFailure("username", "That username is taken");
                }
            });
        }
    }
}