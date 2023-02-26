using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Dto.Responses.Person;

public record GetPersonListItemResponseDto
{
    public Guid Id { get; init; }

    public FullName Name { get; init; } = FullName.Empty;

    public DateTime BirthDate { get; init; }

    public string? Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string? Comment { get; init; }
}
