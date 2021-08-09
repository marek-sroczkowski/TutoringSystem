using FluentValidation;
using System.Linq;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Validators
{
    public class RegisterTutorValidation : AbstractValidator<RegisterTutorDto>
    {
        public RegisterTutorValidation(AppDbContext dbContext)
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.Username).Custom((value, context) =>
            {
                var loginAlreadyExist = dbContext.Users.Any(user => user.Username.Equals(value));
                if (loginAlreadyExist)
                    context.AddFailure("username", "That username is taken");
            });

            RuleFor(u => u.Password).MinimumLength(4);
            RuleFor(u => u.Password).Equal(u => u.ConfirmPassword);
        }
    }
}
