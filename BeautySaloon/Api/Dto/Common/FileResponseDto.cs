namespace BeautySaloon.Api.Dto.Common;

public record FileResponseDto
{
    public string FileName { get; init; } = string.Empty;

    public byte[] Data { get; init; } = Array.Empty<byte>();
}
