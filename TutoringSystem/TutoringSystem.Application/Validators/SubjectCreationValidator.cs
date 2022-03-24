using FluentValidation;
using Microsoft.AspNetCore.Http;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class SubjectCreationValidator : AbstractValidator<NewSubjectDto>
    {
        public SubjectCreationValidator(ISubjectRepository subjectRepository, IHttpContextAccessor httpContext)
        {
            RuleFor(s => s.Name).Custom((value, context) =>
            {
                var userId = httpContext.HttpContext.User.GetUserId();
                if (subjectRepository.IsSubjectExist(s => s.TutorId.Equals(userId) && s.Name.Equals(value)))
                {
                    context.AddFailure("name", "That subject name is taken");
                }
            });
        }
    }
}