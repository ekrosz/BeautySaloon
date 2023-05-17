using AutoFixture;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.DAL.Entities;
using BeautySaloon.Common.Exceptions;

namespace Tests.Entities;

public class OrderTests
{
    private readonly IFixture _fixture;

    public OrderTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Update_WhenOrderStatusIsNotPaid_ShouldBePropertiesUpdated()
    {
        // Arrange
        var cost = _fixture.Create<decimal>();
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>());

        // Act
        entity.Update(cost, comment);

        // Assert
        Assert.Equal(cost, entity.Cost);
        Assert.Equal(comment, entity.Comment);
        Assert.Equal(PaymentStatus.NotPaid, entity.PaymentStatus);
    }

    [Fact]
    public void Update_WhenOrderStatusIsPaid_ShouldBeThrowsOrderAlreadyPaidException()
    {
        // Arrange
        var cost = _fixture.Create<decimal>();
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.Paid
                }
            }
        };

        // Act, Assert
        Assert.Throws<OrderAlreadyPaidException>(() =>  entity.Update(cost, comment));
    }

    [Fact]
    public void Update_WhenOrderStatusIsCancelled_ShouldBeThrowsOrderAlreadyCancelledException()
    {
        // Arrange
        var cost = _fixture.Create<decimal>();
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.Cancelled
                }
            }
        };

        // Act, Assert
        Assert.Throws<OrderAlreadyCancelledException>(() => entity.Update(cost, comment));
    }

    [Fact]
    public void Pay_WhenOrderStatusIsNotPaid_ShouldBePropertiesUpdated()
    {
        // Arrange
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.NotPaid
                }
            }
        }; ;

        // Act
        entity.Pay(PaymentMethod.Cash, comment, null);

        // Assert
        Assert.Equal(comment, entity.Comment);
        Assert.Equal(PaymentMethod.Cash, entity.PaymentMethod);
        Assert.Equal(PaymentStatus.Paid, entity.PaymentStatus);
    }

    [Fact]
    public void Pay_WhenOrderStatusIsPaid_ShouldBeThrowsOrderAlreadyPaidException()
    {
        // Arrange
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.Paid
                }
            }
        };

        // Act, Assert
        Assert.Throws<OrderAlreadyPaidException>(() => entity.Pay(PaymentMethod.Cash, comment, null));
    }

    [Fact]
    public void Pay_WhenOrderStatusIsCancelled_ShouldBeThrowsOrderAlreadyCancelledException()
    {
        // Arrange
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.Cancelled
                }
            }
        };

        // Act, Assert
        Assert.Throws<OrderAlreadyCancelledException>(() => entity.Pay(PaymentMethod.Cash, comment, null));
    }

    [Fact]
    public void Cancel_WhenOrderStatusIsNotPaid_ShouldBePropertiesUpdated()
    {
        // Arrange
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.NotPaid
                }
            }
        }; ;

        // Act
        entity.Cancel(comment);

        // Assert
        Assert.Equal(comment, entity.Comment);
        Assert.Equal(PaymentStatus.Cancelled, entity.PaymentStatus);
    }

    [Fact]
    public void Cancel_WhenOrderStatusIsPaid_ShouldBeThrowsOrderAlreadyPaidException()
    {
        // Arrange
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.Paid
                }
            }
        };

        // Act, Assert
        Assert.Throws<OrderAlreadyPaidException>(() => entity.Cancel(comment));
    }

    [Fact]
    public void Cancel_WhenOrderStatusIsCancelled_ShouldBeThrowsOrderAlreadyCancelledException()
    {
        // Arrange
        var comment = _fixture.Create<string>();

        var entity = new Order(
            _fixture.Create<decimal>(),
            _fixture.Create<string>())
        {
            PersonSubscriptions = new()
            {
                new PersonSubscription(_fixture.Create<SubscriptionCosmeticServiceSnapshot>())
                {
                    Status = PersonSubscriptionCosmeticServiceStatus.Cancelled
                }
            }
        };

        // Act, Assert
        Assert.Throws<OrderAlreadyCancelledException>(() => entity.Cancel(comment));
    }
}
