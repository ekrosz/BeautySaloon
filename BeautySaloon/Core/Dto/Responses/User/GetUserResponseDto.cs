using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Dto.Responses.User;

public record GetUserResponseDto
{
    public Guid Id { get; init; }

    public Role Role { get; init; }

    public string Login { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public FullName Name { get; set; } = FullName.Empty;
}
