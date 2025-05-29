using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PocketCounter.Application.Authorization.Commands.Register;
using PocketCounter.Application.Authorization.DataModels;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Authorization;

public class RegisterUserHandler(UserManager<User> userManager, ILogger<RegisterUserHandler> logger)
{
    private UserManager<User> _userManager = userManager;
    private ILogger<RegisterUserHandler> _logger = logger;

    public async Task<UnitResult<List<Error>>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            Email = command.Email,
            UserName = command.UserName
        };

        var result = await _userManager.CreateAsync(user, command.Password);
        _logger.LogInformation("Created new user {0} ({1})", user.UserName, user.Email);
        if (result.Succeeded) return Result.Success<List<Error>>();
        var errors = result.Errors
            .Select(e => Error.Authentication(e.Code, e.Description)).ToList();
        return errors;
    }
}