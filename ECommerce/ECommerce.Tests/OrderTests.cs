using ECommerce.Domain.Common;
using ECommerce.Domain.OrderAggregate;
using Xunit;

namespace ECommerce.Tests;

public class OrderTests
{
    [Fact]
    public void Create_WithValidData_SucceedsAndTotalsIncludeDelivery()
    {
        var order = OrderFactory.ValidOrder();

        Assert.Equal(100m, order.Subtotal);
        Assert.Equal(110m, order.GetTotal());
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Null(order.PaymentIntentId);
    }

    [Fact]
    public void Create_WithoutItems_Fails()
    {
        var result = Order.Create(
            "buyer@test.com",
            OrderFactory.ValidAddress(),
            OrderFactory.ValidDelivery(),
            []);

        Assert.True(result.IsFailure);
        Assert.Equal("Order.Items", result.Error.Code);
    }

    [Fact]
    public void SetPaymentIntentId_ThenMarkAsPaid_UpdatesStatus()
    {
        var order = OrderFactory.ValidOrder();

        var set = order.SetPaymentIntentId("pi_123");
        var paid = order.MarkAsPaid();

        Assert.True(set.IsSuccess);
        Assert.True(paid.IsSuccess);
        Assert.Equal("pi_123", order.PaymentIntentId);
        Assert.Equal(OrderStatus.PaymentReceived, order.Status);
    }

    [Fact]
    public void MarkAsPaid_WithoutPaymentIntent_Fails()
    {
        var order = OrderFactory.ValidOrder();

        var result = order.MarkAsPaid();

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }

    [Fact]
    public void MarkAsPaymentFailed_SetsFailedStatus()
    {
        var order = OrderFactory.ValidOrder();
        order.SetPaymentIntentId("pi_123");

        var result = order.MarkAsPaymentFailed();

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.PaymentFailed, order.Status);
    }
}

public class AddressTests
{
    [Fact]
    public void Create_MissingCity_FailsValidation()
    {
        var result = Address.Create("A", "B", "Street", "", "State", "12345");

        Assert.True(result.IsFailure);
        Assert.Equal("Address.City", result.Error.Code);
    }
}

public class ResultTests
{
    [Fact]
    public void Success_ExposesValue()
    {
        var result = Result.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failure_ThrowsWhenReadingValue()
    {
        var result = Result.Failure<int>(Error.NotFound("X", "missing"));

        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
