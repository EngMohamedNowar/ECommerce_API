using ECommerce.UseCases.Auth.Commands;
using ECommerce.UseCases.Auth.Dtos;
using ECommerce.UseCases.Auth.Queries;

namespace ECommerce.API.Controllers;

public class AccountsController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(
        RegisterCommand command, CancellationToken ct = default)
    {
        var result = await mediator.Send(command, ct);
        return FromResult(result, "تم تسجيل الحساب بنجاح.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(
        LoginCommand command, CancellationToken ct = default)
    {
        var result = await mediator.Send(command, ct);
        return FromResult(result, "تم تسجيل الدخول بنجاح.");
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken(
        RefreshTokenCommand command, CancellationToken ct = default)
    {
        var result = await mediator.Send(command, ct);
        return FromResult(result);
    }

    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> Me(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetCurrentUserQuery(), ct);
        return FromResult(result);
    }
}