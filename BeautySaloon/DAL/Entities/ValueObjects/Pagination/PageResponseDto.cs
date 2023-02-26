namespace BeautySaloon.DAL.Entities.ValueObjects.Pagination;

public record PageResponseDto<TEnity>(IReadOnlyCollection<TEnity> Items, int TotalCount);
