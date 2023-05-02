using BeautySaloon.Api.Dto.Common;

namespace BeautySaloon.Core.IntegrationServices.MailKit.Dto;

public record SendEmailRequestDto
{
    public string ReceiverName { get; init; } = string.Empty;

    public string ReceiverEmail { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;

    public bool IsHtmlContent { get; init; }

    public IReadOnlyCollection<FileResponseDto> Files { get; init; } = Array.Empty<FileResponseDto>();
}
