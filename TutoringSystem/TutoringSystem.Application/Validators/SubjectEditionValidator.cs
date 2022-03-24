using FluentValidation;
using Microsoft.AspNetCore.Http;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class SubjectEditionValidator : AbstractValidator<UpdatedSubjectDto>
    {
        public SubjectEditionValidator(ISubjectRepository subjectRepository, IHttpContextAccessor httpContext)
        {
            RuleFor(subject => subject).Custom((value, context) =>
            {
                var userId = httpContext.HttpContext.User.GetUserId();
                if (subjectRepository.IsSubjectExist(s => s.TutorId.Equals(userId) && s.Name.Equals(value.Name) && !s.Id.Equals(value.Id)))
                {
                    context.AddFailure("name", "That subject name is taken");
                }
            });
        }
    }
}