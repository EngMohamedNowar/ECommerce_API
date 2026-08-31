using ECommerce.Domain.Common;
using ECommerce.Domain.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Auth.Contracts;
using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Payments.Commands;
using ECommerce.UseCases.Payments.Contracts;
using MapsterMapper;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace ECommerce.Tests;

public class CreateOrUpdatePaymentIntentCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenUnauthenticated_ReturnsUnauthorized()
    {
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.IsAuthenticated.Returns(false);

        var handler = new CreateOrUpdatePaymentIntentCommandHandler(
            Substitute.For<IPaymentService>(),
            currentUser);

        var result = await handler.Handle(
            new CreateOrUpdatePaymentIntentCommand(Guid.NewGuid()),
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.UnAuthorized, result.Error.Type);
        Assert.Equal("Auth.Unauthorized", result.Error.Code);
    }

    [Fact]
    public async Task Handle_WhenAuthenticated_DelegatesToPaymentService()
    {
        var orderId = Guid.NewGuid();
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.IsAuthenticated.Returns(true);
        currentUser.Email.Returns("buyer@test.com");

        var paymentService = Substitute.For<IPaymentService>();
        var expected = Result.Success(CreateOrderResponse());
        paymentService
            .CreateOrUpdatePaymentIntentAsync(orderId, "buyer@test.com", Arg.Any<CancellationToken>())
            .Returns(expected);

        var handler = new CreateOrUpdatePaymentIntentCommandHandler(paymentService, currentUser);

        var result = await handler.Handle(
            new CreateOrUpdatePaymentIntentCommand(orderId),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        await paymentService.Received(1)
            .CreateOrUpdatePaymentIntentAsync(orderId, "buyer@test.com", Arg.Any<CancellationToken>());
    }

    private static OrderResponse CreateOrderResponse() =>
        new(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            "buyer@test.com",
            "Pending",
            100,
            110,
            Guid.NewGuid(),
            "UPS",
            "1-3 Days",
            10,
            new AddressResponse("Mohamed", "Nowar", "Street 1", "Cairo", "Cairo", "12345"),
            null,
            []);
}

public class StripePaymentServiceTests
{
    [Fact]
    public void ToMinorUnits_RoundsAwayFromZero()
    {
        Assert.Equal(11000, StripePaymentService.ToMinorUnits(110m));
        Assert.Equal(1051, StripePaymentService.ToMinorUnits(10.505m));
    }

    [Fact]
    public async Task CreateOrUpdatePaymentIntent_WhenOrderMissing_ReturnsNotFound()
    {
        var orderRepo = Substitute.For<IRepository<Order>>();
        orderRepo
            .GetEntityWithSpecAsync(Arg.Any<Domain.Specifications.ISpecification<Order>>(), Arg.Any<CancellationToken>())
            .Returns((Order?)null);

        var sut = CreateSut(orderRepo, Substitute.For<IStripePaymentIntentGateway>());

        var result = await sut.CreateOrUpdatePaymentIntentAsync(
            Guid.NewGuid(), "buyer@test.com");

        Assert.True(result.IsFailure);
        Assert.Equal("Order.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task CreateOrUpdatePaymentIntent_CreatesIntentAndPersistsId()
    {
        var order = OrderFactory.ValidOrder();
        var orderRepo = Substitute.For<IRepository<Order>>();
        orderRepo
            .GetEntityWithSpecAsync(Arg.Any<Domain.Specifications.ISpecification<Order>>(), Arg.Any<CancellationToken>())
            .Returns(order);

        var gateway = Substitute.For<IStripePaymentIntentGateway>();
        gateway
            .CreateAsync(11000, "usd", order.Id, Arg.Any<CancellationToken>())
            .Returns(Result.Success("pi_new"));

        var unitOfWork = Substitute.For<IUnitOfWork>();
        var mapper = Substitute.For<IMapper>();
        mapper.Map<OrderResponse>(order).Returns(CreateMappedResponse(order, "pi_new"));

        var sut = CreateSut(orderRepo, gateway, unitOfWork, mapper);

        var result = await sut.CreateOrUpdatePaymentIntentAsync(order.Id, order.BuyerEmail);

        Assert.True(result.IsSuccess);
        Assert.Equal("pi_new", order.PaymentIntentId);
        Assert.Equal("pi_new", result.Value.PaymentIntentId);
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateOrUpdatePaymentIntent_WhenIntentExists_UpdatesAmount()
    {
        var order = OrderFactory.ValidOrder();
        order.SetPaymentIntentId("pi_existing");

        var orderRepo = Substitute.For<IRepository<Order>>();
        orderRepo
            .GetEntityWithSpecAsync(Arg.Any<Domain.Specifications.ISpecification<Order>>(), Arg.Any<CancellationToken>())
            .Returns(order);

        var gateway = Substitute.For<IStripePaymentIntentGateway>();
        gateway
            .UpdateAmountAsync("pi_existing", 11000, Arg.Any<CancellationToken>())
            .Returns(Result.Success("pi_existing"));

        var mapper = Substitute.For<IMapper>();
        mapper.Map<OrderResponse>(order).Returns(CreateMappedResponse(order, "pi_existing"));

        var sut = CreateSut(orderRepo, gateway, mapper: mapper);

        var result = await sut.CreateOrUpdatePaymentIntentAsync(order.Id, order.BuyerEmail);

        Assert.True(result.IsSuccess);
        await gateway.Received(1).UpdateAmountAsync("pi_existing", 11000, Arg.Any<CancellationToken>());
        await gateway.DidNotReceive().CreateAsync(
            Arg.Any<long>(), Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessWebhook_OnSucceeded_MarksOrderPaid()
    {
        var order = OrderFactory.ValidOrder();
        order.SetPaymentIntentId("pi_paid");

        var orderRepo = Substitute.For<IRepository<Order>>();
        orderRepo
            .GetEntityWithSpecAsync(Arg.Any<Domain.Specifications.ISpecification<Order>>(), Arg.Any<CancellationToken>())
            .Returns(order);

        var gateway = Substitute.For<IStripePaymentIntentGateway>();
        gateway.ParseWebhook("{}", "sig").Returns(Result.Success(
            new StripeWebhookEvent(StripeWebhookEventTypes.PaymentIntentSucceeded, "pi_paid")));

        var sut = CreateSut(orderRepo, gateway);

        var result = await sut.ProcessWebhookAsync("{}", "sig");

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.PaymentReceived, order.Status);
    }

    [Fact]
    public async Task ProcessWebhook_OnFailed_MarksOrderPaymentFailed()
    {
        var order = OrderFactory.ValidOrder();
        order.SetPaymentIntentId("pi_fail");

        var orderRepo = Substitute.For<IRepository<Order>>();
        orderRepo
            .GetEntityWithSpecAsync(Arg.Any<Domain.Specifications.ISpecification<Order>>(), Arg.Any<CancellationToken>())
            .Returns(order);

        var gateway = Substitute.For<IStripePaymentIntentGateway>();
        gateway.ParseWebhook("{}", "sig").Returns(Result.Success(
            new StripeWebhookEvent(StripeWebhookEventTypes.PaymentIntentPaymentFailed, "pi_fail")));

        var sut = CreateSut(orderRepo, gateway);

        var result = await sut.ProcessWebhookAsync("{}", "sig");

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.PaymentFailed, order.Status);
    }

    private static StripePaymentService CreateSut(
        IRepository<Order> orderRepo,
        IStripePaymentIntentGateway gateway,
        IUnitOfWork? unitOfWork = null,
        IMapper? mapper = null)
    {
        var options = Options.Create(new StripeOptions
        {
            SecretKey = "sk_test",
            PublishableKey = "pk_test",
            WebhookSecret = "whsec",
            Currency = "usd"
        });

        return new StripePaymentService(
            orderRepo,
            unitOfWork ?? Substitute.For<IUnitOfWork>(),
            mapper ?? Substitute.For<IMapper>(),
            gateway,
            options);
    }

    private static OrderResponse CreateMappedResponse(Order order, string paymentIntentId) =>
        new(
            order.Id,
            order.OrderDate,
            order.BuyerEmail,
            order.Status.ToString(),
            order.Subtotal,
            order.GetTotal(),
            order.DeliveryMethodId ?? Guid.Empty,
            order.DeliveryMethod!.ShortName,
            order.DeliveryMethod.DeliveryTime,
            order.DeliveryMethod.Price,
            new AddressResponse(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.Street,
                order.ShippingAddress.City,
                order.ShippingAddress.State,
                order.ShippingAddress.ZipCode),
            paymentIntentId,
            []);
}
