using ECommerce.Domain.OrderAggregate;
using ECommerce.Domain.Specifications;

namespace ECommerce.UseCases.Orders.Specifications;

public sealed class OrdersWithItemsAndDeliveryMethodForUserSpec(string buyerEmail)
    : BaseSpecification<Order>(o => o.BuyerEmail == buyerEmail)
{
    public OrdersWithItemsAndDeliveryMethodForUserSpec() : this(string.Empty) { }

    public OrdersWithItemsAndDeliveryMethodForUserSpec AddDetails()
    {
        AddInclude("Items");
        AddInclude("DeliveryMethod");
        ApplyOrderByDescending(o => o.OrderDate);
        return this;
    }
}

public sealed class OrderWithItemsByUserSpec(Guid orderId, string buyerEmail)
    : BaseSpecification<Order>(o => o.Id == orderId && o.BuyerEmail == buyerEmail)
{
    public OrderWithItemsByUserSpec() : this(Guid.Empty, string.Empty) { }

    public OrderWithItemsByUserSpec AddDetails()
    {
        AddInclude("Items");
        AddInclude("DeliveryMethod");
        return this;
    }
}