using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketCounter.Api.Controllers.Requests;
using PocketCounter.Api.Response;
using PocketCounter.Application.Authorization;
using PocketCounter.Application.Authorization.Commands.Register;
using PocketCounter.Domain.Share;

namespace PocketCounter.Api.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    [HttpPost("registration")]
    public async Task<ActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken
    )
    {
        if (request.Email != "polina.khamitova1219@gmail.com" || request.Email != "polianna1219@gmail.com")
                 return BadRequest(Error.Authentication("user.not.valid","This user cannot to register."));
        
        var command = new RegisterUserCommand(request.Email, request.UserName, request.Password);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(Envelope.Ok());
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginUserHandler handler,
        CancellationToken cancellationToken
    )
    {
        var command = request.ToCommand();
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(Envelope.Ok(result.Value));
    }
}