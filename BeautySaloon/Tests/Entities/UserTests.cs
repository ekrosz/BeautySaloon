using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.DAL.Entities;
using AutoFixture;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.Common.Utils;

namespace Tests.Entities;

public class UserTests
{
    private readonly IFixture _fixture;

    public UserTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Update_ShouldBePropertiesUpdated()
    {
        // Arrange
        var role = _fixture.Create<Role>();
        var login = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var fullName = _fixture.Create<FullName>();
        var phoneNumber = _fixture.Create<string>();
        var email = _fixture.Create<string?>();

        var entity = new User(
            _fixture.Create<Role>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<FullName>(),
            _fixture.Create<string>(),
            _fixture.Create<string?>());

        // Act
        entity.Update(role, login, password, fullName, phoneNumber, email);

        // Assert
        Assert.Equal(role, entity.Role);
        Assert.Equal(login, entity.Login);
        Assert.Equal(CryptoUtilities.GetEncryptedPassword(password), entity.Password);
        Assert.Equal(fullName, entity.Name);
        Assert.Equal(phoneNumber, entity.PhoneNumber);
        Assert.Equal(email, entity.Email);
    }
}
