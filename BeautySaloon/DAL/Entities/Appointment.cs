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

    public Appointment(DateTime appointmentDate, int durationInMinutes)
    {
        AppointmentDate = appointmentDate;
        DurationInMinutes = durationInMinutes;
    }

    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public int DurationInMinutes { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public Person Person { get; set; } = default!;

    public List<PersonSubscription> PersonSubscriptions { get; set; } = new List<PersonSubscription>();

    [JsonIgnore]
    public AppointmentStatus Status => CalcStatus();

    public void AddPersonSubscription(IEnumerable<PersonSubscription> entities)
    {
        PersonSubscriptions.Clear();
        PersonSubscriptions.AddRange(entities);
    }

    public void Complete(string? comment)
    {
        if (Status == AppointmentStatus.Cancelled)
        {
            throw new Exception($"Запись {Id} уже была отменена.");
        }

        if (Status == AppointmentStatus.Completed)
        {
            throw new Exception($"Запись {Id} уже выполнена.");
        }

        Comment = comment;
        PersonSubscriptions.ForEach(x => x.Status = PersonSubscriptionStatus.Completed);
    }

    public void Cancel(string? comment)
    {
        if (Status == AppointmentStatus.Cancelled)
        {
            throw new Exception($"Запись {Id} уже была отменена.");
        }

        if (Status == AppointmentStatus.Completed)
        {
            throw new Exception($"Запись {Id} уже выполнена.");
        }

        Comment= comment;
        PersonSubscriptions.Clear();
    }

    private AppointmentStatus CalcStatus()
    {
        if (!PersonSubscriptions.Any())
        {
            return AppointmentStatus.Cancelled;
        }

        if (PersonSubscriptions.All(x => x.Status == PersonSubscriptionStatus.Paid))
        {
            return AppointmentStatus.NotImplemented;
        }

        return AppointmentStatus.Completed;
    }
}
