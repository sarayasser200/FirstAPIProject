﻿using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FirstAPIProject.Authorization
{
    public class AgeAuthorizationHandler : AuthorizationHandler<AgeGreaterThan25Requirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeGreaterThan25Requirement requirement)
        {
            var dob = DateTime.Parse(context.User.FindFirstValue("DateogBirth"));
            if (DateTime.Today.Year - dob.Year >= 25)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
