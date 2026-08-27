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

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;

internal sealed class LoginCommandHandler(
    IAuthTokenService tokenService,
    IRepository<RefreshToken> refreshTokenRepo,
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IOptions<AuthOptions> authOptions)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            return Result.Failure<AuthResponse>(
                Error.Unauthorized("Auth.InvalidCredentials", "بيانات الدخول غير صحيحة."));

        await RevokeExistingTokensAsync(user.Id, cancellationToken);

        var roles = await userManager.GetRolesAsync(user);
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

private async Task RevokeExistingTokensAsync(Guid userId, CancellationToken cancellationToken)
    {
        var activeTokens = await refreshTokenRepo.GetAllWithSpecAsync(
            new ActiveRefreshTokensForUserSpec(userId), cancellationToken);

        foreach (var token in activeTokens)
            token.Revoke();
    }
}