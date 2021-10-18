using FluentValidation;
using System.Linq;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RegisterTutorValidation : AbstractValidator<RegisterTutorDto>
    {
        public RegisterTutorValidation(IUserRepository userRepository)
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.Username).Custom((value, context) =>
            {
                var users = userRepository.GetUsersCollection(null);
                var loginAlreadyExist = users.Any(user => user.Username.Equals(value));
                if (loginAlreadyExist)
                    context.AddFailure("username", "That username is taken");
            });
            RuleFor(u => u.Email).Custom((value, context) =>
            {
                var users = userRepository.GetUsersCollection(null);
                var emailAlreadyExist = users.Any(user => user.Contact.Email == value);
                if (emailAlreadyExist)
                    context.AddFailure("email", "That email is taken");
            });

            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Password).Matches(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            RuleFor(u => u.Password).Equal(u => u.ConfirmPassword);
        }
    }
}
