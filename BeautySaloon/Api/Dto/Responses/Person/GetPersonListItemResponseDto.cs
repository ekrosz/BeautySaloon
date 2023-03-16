using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Api.Dto.Responses.Person;

public record GetPersonListItemResponseDto
{
    public Guid Id { get; init; }

    public FullName Name { get; init; } = FullName.Empty;

    public DateTime BirthDate { get; init; }

    public string? Email { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;
}
