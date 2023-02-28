using BeautySaloon.Common.Exceptions;
using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;
using System.Text.Json.Serialization;

namespace BeautySaloon.DAL.Entities;

public class Order : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private Order()
    {
    }

    public Order(decimal cost)
    {
        Cost = cost;
        PaymentMethod = PaymentMethod.None;
    }

    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public decimal Cost { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public Person Person { get; set; } = default!;

    public List<PersonSubscription> PersonSubscriptions { get; set; } = new List<PersonSubscription>();

    [JsonIgnore]
    public PaymentStatus PaymentStatus => CalcStatus();

    public void AddPersonSubscriptions(IEnumerable<PersonSubscription> entities)
    {
        if (PaymentStatus == PaymentStatus.Paid)
        {
            throw new OrderAlreadyPaidException(Id);
        }

        if (PaymentStatus == PaymentStatus.Cancelled)
        {
            throw new OrderAlreadyCancelledException(Id);
        }

        PersonSubscriptions.Clear();
        PersonSubscriptions.AddRange(entities);
    }

    public void Pay(PaymentMethod paymentMethod)
    {
        if (PaymentStatus == PaymentStatus.Paid)
        {
            throw new OrderAlreadyPaidException(Id);
        }

        if (PaymentStatus == PaymentStatus.Cancelled)
        {
            throw new OrderAlreadyCancelledException(Id);
        }

        PaymentMethod = paymentMethod;
        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionStatus.Paid);
    }

    public void Cancel()
    {
        if (PaymentStatus == PaymentStatus.Paid)
        {
            throw new OrderAlreadyPaidException(Id);
        }

        if (PaymentStatus == PaymentStatus.Cancelled)
        {
            throw new OrderAlreadyCancelledException(Id);
        }

        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionStatus.Cancelled);
    }

    private PaymentStatus CalcStatus()
    {
        if (!PersonSubscriptions.Any() || PersonSubscriptions.All(x => x.Status == PersonSubscriptionStatus.NotPaid))
        {
            return PaymentStatus.NotPaid;
        }

        if (PersonSubscriptions.All(x => x.Status == PersonSubscriptionStatus.Cancelled))
        {
            return PaymentStatus.Cancelled;
        }

        return PaymentStatus.Paid;
    }
}
