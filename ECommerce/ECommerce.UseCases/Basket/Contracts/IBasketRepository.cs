using ECommerce.Domain.Basket;

namespace ECommerce.UseCases.Basket.Contracts;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default);
    Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, CancellationToken ct = default);
    Task<bool> DeleteBasketAsync(string basketId, CancellationToken ct = default);
}