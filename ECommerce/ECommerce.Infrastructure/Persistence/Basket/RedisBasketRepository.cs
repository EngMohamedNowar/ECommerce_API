using System.Text.Json;
using ECommerce.Domain.Basket;
using ECommerce.UseCases.Basket.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Persistence.Basket;

public sealed class RedisBasketRepository(
    IDistributedCache cache,
    ILogger<RedisBasketRepository> logger) : IBasketRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly DistributedCacheEntryOptions CacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
    };

    public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default)
    {
        try
        {
            var json = await cache.GetStringAsync(basketId, ct);

            return json is null
                ? null
                : JsonSerializer.Deserialize<CustomerBasket>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis down while reading basket {BasketId}", basketId);
            return null;
        }
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(basket, JsonOptions);

            await cache.SetStringAsync(basket.Id, json, CacheEntryOptions, ct);

            return basket;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis down while saving basket {BasketId}", basket.Id);
            return null;
        }
    }

    public async Task<bool> DeleteBasketAsync(string basketId, CancellationToken ct = default)
    {
        try
        {
            var exists = await cache.GetStringAsync(basketId, ct) is not null;

            if (exists)
                await cache.RemoveAsync(basketId, ct);

            return exists;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis down while deleting basket {BasketId}", basketId);
            return false;
        }
    }
}