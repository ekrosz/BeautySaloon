using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.DAL.Entities;

public class Person : IEntity, ISoftDeletable, IAuditable, IHasPhoneNumber
{
    [Obsolete("For EF")]
    private Person()
    {
    }

    public Person(
        FullName name,
        DateTime birthDate,
        string phoneNumber,
        string? email,
        string? comment)
    {
        Name = name;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Email = email;
        Comment = comment;
    }

    public Guid Id { get; set; }

    public FullName Name { get; set; } = FullName.Empty;

    public DateTime BirthDate { get; set; }

    public string? Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Comment { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public List<Order> Orders { get; set; } = new List<Order>();

    public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    public void AddOrder(Order entity) => Orders.Add(entity);

    public void AddAppointment(Appointment entity) => Appointments.Add(entity);

    public void Update(
        FullName name,
        DateTime birthDate,
        string phoneNumber,
        string? email,
        string? comment)
    {
        Name = name;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Email = email;
        Comment = comment;
    }
}
