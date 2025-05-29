using Microsoft.AspNetCore.Authorization;

namespace PocketCounter.Infrastructure.Authentication;

public class CreateIssueRequirement : IAuthorizationRequirement
{
    public CreateIssueRequirement(string code) => Code = code;

    public string Code { get; }
}