using PocketCounter.Application.Authorization.Commands.Login;

namespace PocketCounter.Api.Controllers.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new (Email, Password);
}