namespace BeautySaloon.Api.Dto.Common;

public record FileResponseDto
{
    public byte[] Data { get; init; } = Array.Empty<byte>();
}
