using ECommerce.Domain.Common;
using ECommerce.Domain.Identity;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Auth.Contracts;
using ECommerce.UseCases.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Auth.Commands;

public sealed record RegisterCommand(
    string DisplayName,
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;

internal sealed class RegisterCommandHandler(
    IAuthTokenService tokenService,
    IRepository<RefreshToken> refreshTokenRepo,
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IOptions<AuthOptions> authOptions)
    : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        RegisterCommand request, CancellationToken cancellationToken)
    {
        var userResult = AppUser.Create(request.DisplayName, request.Email);
        if (userResult.IsFailure)
            return Result.Failure<AuthResponse>(userResult.Error);

        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            return Result.Failure<AuthResponse>(
                Error.Conflict("Auth.EmailExists", "البريد الإلكتروني مسجل من قبل."));

        var identityResult = await userManager.CreateAsync(userResult.Value, request.Password);
        if (!identityResult.Succeeded)
            return Result.Failure<AuthResponse>(
                Error.Validation("Auth.RegisterFailed",
                    string.Join(", ", identityResult.Errors.Select(e => e.Description))));

        await userManager.AddToRoleAsync(userResult.Value, Roles.Customer);

        return await CreateAuthResponseAsync(userResult.Value, [Roles.Customer], cancellationToken);
    }

    private async Task<Result<AuthResponse>> CreateAuthResponseAsync(
        AppUser user, IEnumerable<string> roles, CancellationToken cancellationToken)
    {
        var rawToken = tokenService.GenerateRefreshToken();
        var refreshExpiresAt = DateTimeOffset.UtcNow.AddDays(authOptions.Value.RefreshTokenExpirationDays);

        var refreshTokenResult = RefreshToken.Create(
            tokenService.HashRefreshToken(rawToken),
            user.Id,
            refreshExpiresAt);

        if (refreshTokenResult.IsFailure)
            return Result.Failure<AuthResponse>(refreshTokenResult.Error);

        refreshTokenRepo.Add(refreshTokenResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var accessExpiresAt = DateTimeOffset.UtcNow.AddMinutes(authOptions.Value.AccessTokenExpirationMinutes);

        return Result.Success(new AuthResponse(accessToken, rawToken, accessExpiresAt));
    }
}