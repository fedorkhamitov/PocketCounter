namespace PocketCounter.Application.Authorization.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, string Password);