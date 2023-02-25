namespace BeautySaloon.Core.Dto.Common;

public record ItemListResponseDto<T>(IReadOnlyCollection<T> Items);
