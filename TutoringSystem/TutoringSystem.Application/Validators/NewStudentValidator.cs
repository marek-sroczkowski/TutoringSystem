using FluentValidation;
using TutoringSystem.Application.Dtos.Account;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class NewStudentValidator : AbstractValidator<NewStudentDto>
    {
        private readonly IUserRepository userRepository;

        public NewStudentValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(s => s.HourlRate).GreaterThan(0).LessThan(10000);
            RuleFor(u => u.Username).NotEmpty();

            ValidateUsernameExistence();
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