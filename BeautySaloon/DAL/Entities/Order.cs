﻿using BeautySaloon.Common.Exceptions;
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

    public Order(decimal cost, string? comment)
    {
        Cost = cost;
        Comment = comment;
        PaymentMethod = PaymentMethod.None;
    }

    public Guid Id { get; set; }

    public decimal Cost { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public Person Person { get; set; } = default!;

    public User Modifier { get; set; } = default!;

    public List<PersonSubscription> PersonSubscriptions { get; set; } = new List<PersonSubscription>();

    [JsonIgnore]
    public PaymentStatus PaymentStatus => CalcStatus();

    public void Update(
        decimal cost,
        string? comment)
    {
        ThrowIfFinalStatus();

        Cost = cost;
        Comment = comment;
    }

    public void AddPersonSubscriptions(IEnumerable<PersonSubscription> entities)
    {
        ThrowIfFinalStatus();

        PersonSubscriptions.Clear();
        PersonSubscriptions.AddRange(entities);
    }

    public void Pay(PaymentMethod paymentMethod, string? comment)
    {
        ThrowIfFinalStatus();

        PaymentMethod = paymentMethod;
        Comment = comment;
        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionStatus.Paid);
    }

    public void Cancel(string? comment)
    {
        ThrowIfFinalStatus();

        Comment = comment;
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

    private void ThrowIfFinalStatus()
    {
        if (PaymentStatus == PaymentStatus.Paid)
        {
            throw new OrderAlreadyPaidException();
        }

        if (PaymentStatus == PaymentStatus.Cancelled)
        {
            throw new OrderAlreadyCancelledException();
        }
    }
}
