using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PocketCounter.Application.Authorization.Commands.Login;
using PocketCounter.Application.Authorization.DataModels;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Authorization;

public class LoginUserHandler(
    UserManager<User> userManager, 
    ILogger<RegisterUserHandler> logger, 
    ITokenProvider tokenProvider)
{
    private UserManager<User> _userManager = userManager;
    private ILogger<RegisterUserHandler> _logger = logger;
    private ITokenProvider _tokenProvider = tokenProvider;

    public async Task<Result<string, List<Error>>> Handle(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return new List<Error> { Error.Authentication("user.not.found", "user not found") };

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (passwordConfirmed == false)
            return new List<Error> { Error.Authentication("user.fail", "user fail") };

        var token = _tokenProvider.GenerateAccessToken(user);
        
        _logger.LogInformation("Successful login");
        
        return token;
    }
}