﻿using System.Security.Claims;

namespace TutoringSystem.Application.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static long GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return long.Parse(userId);
        }

        public static string GetUsername(this ClaimsPrincipal principal)
        {
            var username = principal.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            return username;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            var role = principal.FindFirst(c => c.Type == ClaimTypes.Role).Value;

            return role;
        }
    }
}
