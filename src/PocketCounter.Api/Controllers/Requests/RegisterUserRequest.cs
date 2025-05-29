namespace PocketCounter.Api.Controllers.Requests;

public record RegisterUserRequest(string Email, string UserName, string Password);