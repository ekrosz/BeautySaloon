using AutoFixture;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace Tests.Entities;

public class PersonTests
{
    private readonly IFixture _fixture;

    public PersonTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Update_ShouldBePropertiesUpdated()
    {
        // Arrange
        var fullName = _fixture.Create<FullName>();
        var birthDate = _fixture.Create<DateTime>();
        var phoneNumber = _fixture.Create<string>();
        var email = _fixture.Create<string?>();

        var entity = new Person(
            _fixture.Create<FullName>(),
            _fixture.Create<DateTime>(),
            _fixture.Create<string>(),
            _fixture.Create<string?>());

        // Act
        entity.Update(fullName, birthDate, phoneNumber, email);

        // Assert
        Assert.Equal(fullName, entity.Name);
        Assert.Equal(birthDate, entity.BirthDate);
        Assert.Equal(phoneNumber, entity.PhoneNumber);
        Assert.Equal(email, entity.Email);
    }
}
