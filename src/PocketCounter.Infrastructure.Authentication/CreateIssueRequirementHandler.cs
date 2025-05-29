using Microsoft.AspNetCore.Authorization;

namespace PocketCounter.Infrastructure.Authentication;

public class CreateIssueRequirementHandler : AuthorizationHandler<CreateIssueRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CreateIssueRequirement requirement)
    {
        context.Succeed(requirement);
    }
}