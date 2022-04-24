﻿using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Reservation;

namespace TutoringSystem.Application.Authorization
{
    class ReservationResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, ReservationDetailsDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, ReservationDetailsDto resource)
        {
            if (requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.GetUserId();

            if (resource.TutorId == userId || resource.StudentId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}