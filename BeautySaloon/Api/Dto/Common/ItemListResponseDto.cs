namespace BeautySaloon.Api.Dto.Common;

public record ItemListResponseDto<T>(IReadOnlyCollection<T> Items);
