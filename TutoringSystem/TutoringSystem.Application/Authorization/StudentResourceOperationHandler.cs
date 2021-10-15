using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Authorization
{
    public class StudentResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, StudentDetailsDto>
    {
        private readonly ITutorRepository tutorRepository;

        public StudentResourceOperationHandler(ITutorRepository tutorRepository)
        {
            this.tutorRepository = tutorRepository;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, StudentDetailsDto resource)
        {
            if (requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }

            var tutorId = context.User.GetUserId();
            var tutor = tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId)).Result;
            IEnumerable<long> studentIds = tutor.Students.Select(s => s.Id);

            if (studentIds.Contains(resource.Id))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
