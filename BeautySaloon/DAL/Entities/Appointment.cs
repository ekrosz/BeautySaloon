using BeautySaloon.Common.Exceptions;
using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;
using System.Text.Json.Serialization;

namespace BeautySaloon.DAL.Entities;

public class Appointment : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private Appointment()
    {
    }

    public Appointment(DateTime appointmentDate, int durationInMinutes, string? comment)
    {
        AppointmentDate = appointmentDate;
        DurationInMinutes = durationInMinutes;
        Comment = comment;
    }

    public Guid Id { get; set; }

    public DateTime AppointmentDate { get; set; }

    public int DurationInMinutes { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public Person Person { get; set; } = default!;

    public User Modifier { get; set; } = default!;

    public List<PersonSubscription> PersonSubscriptions { get; set; } = new List<PersonSubscription>();

    [JsonIgnore]
    public AppointmentStatus Status => CalcStatus();

    public void Update(
        DateTime appointmentDate,
        int durationInMinutes,
        string? comment)
    {
        ThrowIfFinalStatus();

        AppointmentDate = appointmentDate;
        DurationInMinutes = durationInMinutes;
        Comment = comment;
    }

    public void AddPersonSubscription(IEnumerable<PersonSubscription> entities)
    {
        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionCosmeticServiceStatus.Paid);
        PersonSubscriptions.Clear();

        PersonSubscriptions.AddRange(entities);
        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionCosmeticServiceStatus.InProgress);
    }

    public void Complete(string? comment)
    {
        ThrowIfFinalStatus();

        Comment = comment;
        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionCosmeticServiceStatus.Completed);
    }

    public void Cancel(string? comment)
    {
        ThrowIfFinalStatus();

        Comment = comment;
        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionCosmeticServiceStatus.Paid);
        PersonSubscriptions.Clear();
    }

    private AppointmentStatus CalcStatus()
    {
        if (!PersonSubscriptions.Any())
        {
            return AppointmentStatus.Cancelled;
        }

        if (PersonSubscriptions.All(x => x.Status == PersonSubscriptionCosmeticServiceStatus.Paid))
        {
            return AppointmentStatus.NotImplemented;
        }

        if (PersonSubscriptions.Any(x => x.Status == PersonSubscriptionCosmeticServiceStatus.InProgress))
        {
            return AppointmentStatus.InProgress;
        }

        return AppointmentStatus.Completed;
    }

    private void ThrowIfFinalStatus()
    {
        if (Status == AppointmentStatus.Cancelled)
        {
            throw new AppointmentAlreadyCancelledException();
        }

        if (Status == AppointmentStatus.Completed)
        {
            throw new AppointmentAlreadyCompletedException();
        }
    }
}
