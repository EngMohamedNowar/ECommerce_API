using ECommerce.Domain.Common;
using ECommerce.Domain.Identity;
using ECommerce.UseCases.Auth.Contracts;
using ECommerce.UseCases.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.UseCases.Auth.Queries;

public sealed record GetCurrentUserQuery() : IRequest<Result<UserResponse>>;

internal sealed class GetCurrentUserQueryHandler(
    ICurrentUser currentUser,
    UserManager<AppUser> userManager)
    : IRequestHandler<GetCurrentUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(
        GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.Id is null)
            return Result.Failure<UserResponse>(
                Error.Unauthorized("Auth.Unauthorized", "يجب تسجيل الدخول أولاً."));

        var user = await userManager.FindByIdAsync(currentUser.Id.Value.ToString());
        if (user is null)
            return Result.Failure<UserResponse>(
                Error.NotFound("Auth.UserNotFound", "المستخدم غير موجود."));

        var roles = (await userManager.GetRolesAsync(user)).ToList();

        return Result.Success(new UserResponse(user.Id, user.Email!, user.DisplayName, roles));
    }
}