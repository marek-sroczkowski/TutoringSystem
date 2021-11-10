using FluentValidation;
using System.Linq;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RegisterStudentValidation : AbstractValidator<RegisterStudentDto>
    {
        public RegisterStudentValidation(IUserRepository userRepository)
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.Username).Custom((value, context) =>
            {
                var users = userRepository.GetUsersCollection(null);
                var loginAlreadyExist = users.Any(user => user.Username.Equals(value));
                if (loginAlreadyExist)
                    context.AddFailure("username", "That username is taken");
            });

            RuleFor(u => u.Password).Matches(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,32}$");
            RuleFor(u => u.Password).Equal(u => u.ConfirmPassword);
        }
    }
}
