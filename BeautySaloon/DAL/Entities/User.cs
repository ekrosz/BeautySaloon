using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.DAL.Entities;

public class User : IEntity, ISoftDeletable
{
    public User(
        Role role,
        string login,
        string password,
        string phoneNumber,
        string email,
        string firstName,
        string lastName,
        string? middleName)
    {
        Role = role;
        Login = login;
        Password = password;
        PhoneNumber = phoneNumber;
        Email = email;
        Name = new FullName(firstName, lastName, middleName);
    }

    public Guid Id { get; set; }

    public Role Role { get; set; }

    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public FullName Name { get; set; } = FullName.Empty;

    public bool IsDeleted { get; set; }
}
