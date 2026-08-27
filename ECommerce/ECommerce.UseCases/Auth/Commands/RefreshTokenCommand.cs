using ECommerce.Domain.Common;
using ECommerce.Domain.Identity;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Auth.Contracts;
using ECommerce.UseCases.Auth.Dtos;
using ECommerce.UseCases.Auth.Specifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Auth.Commands;

public sealed record RefreshTokenCommand(
    string RefreshToken) : IRequest<Result<AuthResponse>>;

internal sealed class RefreshTokenCommandHandler(
    IAuthTokenService tokenService,
    IRepository<RefreshToken> refreshTokenRepo,
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IOptions<AuthOptions> authOptions)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = tokenService.HashRefreshToken(request.RefreshToken);

        var storedToken = await refreshTokenRepo.GetEntityWithSpecAsync(
            new RefreshTokenByHashSpec(tokenHash), cancellationToken);

        if (storedToken is null || storedToken.IsRevoked || storedToken.IsExpired)
            return Result.Failure<AuthResponse>(
                Error.Unauthorized("Auth.InvalidRefreshToken", "الرمز غير صالح أو منتهي."));

        var revokeResult = storedToken.Revoke();
        if (revokeResult.IsFailure)
            return Result.Failure<AuthResponse>(revokeResult.Error);

        var user = await userManager.FindByIdAsync(storedToken.UserId.ToString());
        if (user is null)
            return Result.Failure<AuthResponse>(
                Error.Unauthorized("Auth.UserNotFound", "المستخدم غير موجود."));

        var roles = await userManager.GetRolesAsync(user);

        var rawToken = tokenService.GenerateRefreshToken();
        var refreshExpiresAt = DateTimeOffset.UtcNow.AddDays(authOptions.Value.RefreshTokenExpirationDays);

        var newTokenResult = RefreshToken.Create(
            tokenService.HashRefreshToken(rawToken),
            user.Id,
            refreshExpiresAt);

        if (newTokenResult.IsFailure)
            return Result.Failure<AuthResponse>(newTokenResult.Error);

        refreshTokenRepo.Add(newTokenResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var accessExpiresAt = DateTimeOffset.UtcNow.AddMinutes(authOptions.Value.AccessTokenExpirationMinutes);

        return Result.Success(new AuthResponse(accessToken, rawToken, accessExpiresAt));
    }
}