using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerce.PL.Authorization
{
    public class OwnerAuthorizationHandler
        : AuthorizationHandler<OwnerRequirement, string>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnerRequirement requirement,
            string resourceOwnerId)
        {
            var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(currentUserId)
                && currentUserId == resourceOwnerId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
