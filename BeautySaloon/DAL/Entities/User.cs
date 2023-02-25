using BeautySaloon.Common.Utils;
using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.DAL.Entities;

public class User : IEntity, ISoftDeletable
{
    [Obsolete("For EF")]
    private User()
    {
    }

    public User(
        Role role,
        string login,
        string password,
        FullName name,
        string phoneNumber,
        string? email)
    {
        Role = role;
        Login = login;
        Password = CryptoUtility.GetEncryptedPassword(password);
        PhoneNumber = phoneNumber;
        Email = email;
        Name = name;
    }

    public Guid Id { get; set; }

    public Role Role { get; set; }

    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public FullName Name { get; set; } = FullName.Empty;

    public Guid? RefreshSecretKey { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsValidPassword(string password)
        => Password.Equals(CryptoUtility.GetEncryptedPassword(password));

    public bool IsValidRefreshSecret(Guid refreshSecret)
        => RefreshSecretKey == refreshSecret;

    public Guid GenerateNewRefreshSecret()
    {
        RefreshSecretKey = Guid.NewGuid();

        return RefreshSecretKey.Value;
    }

    public void Update(
        Role role,
        string login,
        string password,
        FullName name,
        string phoneNumber,
        string? email)
    {
        Role = role;
        Login = login;
        Password = CryptoUtility.GetEncryptedPassword(password);
        PhoneNumber = phoneNumber;
        Email = email;
        Name = name;
    }

    public void Delete() => IsDeleted = true;
}
