using FluentValidation;
using Microsoft.AspNetCore.Http;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Subject;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class SubjectCreationValidator : AbstractValidator<NewSubjectDto>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IHttpContextAccessor httpContext;

        public SubjectCreationValidator(ISubjectRepository subjectRepository, IHttpContextAccessor httpContext)
        {
            this.subjectRepository = subjectRepository;
            this.httpContext = httpContext;

            ValidateSubjectNameExistence();
        }

        private void ValidateSubjectNameExistence()
        {
            RuleFor(s => s.Name).Custom((value, context) =>
            {
                var userId = httpContext.HttpContext.User.GetUserId();
                if (subjectRepository.SubjectExists(s => s.TutorId.Equals(userId) && s.Name.Equals(value)))
                {
                    context.AddFailure("name", "That subject name is taken");
                }
            });
        }
    }
}