using ECommerce.Domain.Common;

namespace ECommerce.Domain.OrderAggregate;

public sealed record Address(
    string FirstName,
    string LastName,
    string Street,
    string City,
    string State,
    string ZipCode)
{
    public static Result<Address> Create(
        string firstName, string lastName, string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<Address>(Error.Validation("Address.FirstName", "الاسم الأول مطلوب."));

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<Address>(Error.Validation("Address.LastName", "الاسم الأخير مطلوب."));

        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>(Error.Validation("Address.Street", "العنوان مطلوب."));

        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>(Error.Validation("Address.City", "المدينة مطلوبة."));

        if (string.IsNullOrWhiteSpace(state))
            return Result.Failure<Address>(Error.Validation("Address.State", "المحافظة مطلوبة."));

        if (string.IsNullOrWhiteSpace(zipCode))
            return Result.Failure<Address>(Error.Validation("Address.ZipCode", "الرمز البريدي مطلوب."));

        return Result.Success(new Address(firstName, lastName, street, city, state, zipCode));
    }
}