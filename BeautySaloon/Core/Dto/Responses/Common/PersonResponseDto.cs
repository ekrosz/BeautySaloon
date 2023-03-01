using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Dto.Responses.Common;

public record PersonResponseDto
{
    public Guid Id { get; init; }

    public FullName Name { get; init; } = FullName.Empty;

    public string PhoneNumber { get; init; } = string.Empty;
}
