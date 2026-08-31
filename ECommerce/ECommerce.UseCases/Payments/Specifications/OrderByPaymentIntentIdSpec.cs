using ECommerce.Domain.OrderAggregate;
using ECommerce.Domain.Specifications;

namespace ECommerce.UseCases.Payments.Specifications;

public sealed class OrderByPaymentIntentIdSpec(string paymentIntentId)
    : BaseSpecification<Order>(o => o.PaymentIntentId == paymentIntentId)
{
    public OrderByPaymentIntentIdSpec() : this(string.Empty) { }

    public OrderByPaymentIntentIdSpec AddDetails()
    {
        AddInclude("Items");
        AddInclude("DeliveryMethod");
        return this;
    }
}