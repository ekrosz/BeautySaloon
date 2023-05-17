using AutoFixture;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using FluentValidation;

namespace Tests.Entities;

public class CosmeticServiceValidatorTests
{
    private readonly IValidator<CreateCosmeticServiceRequestDto> _validator;

    private readonly IFixture _fixture;

    public CosmeticServiceValidatorTests()
    {
        _validator = new CreateCosmeticServiceRequestValidator();
        _fixture = new Fixture();
    }

    [Fact]
    public void Validate_WhenCorrectlyData_SouldBeIsValid()
    {
        // Arrange
        var data = new CreateCosmeticServiceRequestDto
        {
            Name = string.Join(string.Empty, _fixture.CreateMany<char>(100)),
            Description = string.Join(string.Empty, _fixture.CreateMany<char>(500)),
            ExecuteTimeInMinutes = 1440
        };

        // Act
        var result = _validator.Validate(data);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenNameLengthGreatThenMaximumLength_SouldBeIsNotValid()
    {
        // Arrange
        var data = new CreateCosmeticServiceRequestDto
        {
            Name = string.Join(string.Empty, _fixture.CreateMany<char>(100)),
            Description = string.Join(string.Empty, _fixture.CreateMany<char>(501)),
            ExecuteTimeInMinutes = 1440
        };

        // Act
        var result = _validator.Validate(data);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_SouldBeIsNotValid()
    {
        // Arrange
        var data = new CreateCosmeticServiceRequestDto
        {
            Name = string.Empty,
            Description = string.Join(string.Empty, _fixture.CreateMany<char>(500)),
            ExecuteTimeInMinutes = 1440
        };

        // Act
        var result = _validator.Validate(data);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenDescriptionLengthGreatThenMaximumLength_SouldBeIsNotValid()
    {
        // Arrange
        var data = new CreateCosmeticServiceRequestDto
        {
            Name = string.Join(string.Empty, _fixture.CreateMany<char>(101)),
            Description = string.Join(string.Empty, _fixture.CreateMany<char>(500)),
            ExecuteTimeInMinutes = 1440
        };

        // Act
        var result = _validator.Validate(data);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenDescriptionIsEmpty_SouldBeIsNotValid()
    {
        // Arrange
        var data = new CreateCosmeticServiceRequestDto
        {
            Name = string.Join(string.Empty, _fixture.CreateMany<char>(100)),
            Description = string.Empty,
            ExecuteTimeInMinutes = 1440
        };

        // Act
        var result = _validator.Validate(data);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenExecuteTimeInMinutesGreatThenMaximumValue_SouldBeIsNotValid()
    {
        // Arrange
        var data = new CreateCosmeticServiceRequestDto
        {
            Name = string.Join(string.Empty, _fixture.CreateMany<char>(100)),
            Description = string.Join(string.Empty, _fixture.CreateMany<char>(500)),
            ExecuteTimeInMinutes = 1441
        };

        // Act
        var result = _validator.Validate(data);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenExecuteTimeInMinutesLessThenMinimumValue_SouldBeIsNotValid()
    {
        // Arrange
        var data = new CreateCosmeticServiceRequestDto
        {
            Name = string.Join(string.Empty, _fixture.CreateMany<char>(100)),
            Description = string.Join(string.Empty, _fixture.CreateMany<char>(500)),
            ExecuteTimeInMinutes = 0
        };

        // Act
        var result = _validator.Validate(data);

        // Assert
        Assert.False(result.IsValid);
    }
}
