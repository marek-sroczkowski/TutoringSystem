using FluentValidation;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class NewStudentValidation : AbstractValidator<NewStudentDto>
    {
        public NewStudentValidation(IUserRepository userRepository)
        {
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(s => s.HourlRate).GreaterThan(0).LessThan(10000);
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.Username).Custom((value, context) =>
            {
                if (userRepository.IsUserExist(user => user.Username.Equals(value)))
                {
                    context.AddFailure("username", "That username is taken");
                }
            });
        }
    }
}