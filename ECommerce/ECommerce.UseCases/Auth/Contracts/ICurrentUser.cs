namespace ECommerce.UseCases.Auth.Contracts;

public interface ICurrentUser
{
    Guid? Id { get; }
    string? Email { get; }
    string? DisplayName { get; }
    IEnumerable<string> Roles { get; }
    bool IsAuthenticated { get; }
}