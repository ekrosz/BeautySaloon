using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Dto.Responses.Common;

public record ModifierResponseDto
{
    public Role Role { get; init; }

    public FullName Name { get; init; } = FullName.Empty;
}
