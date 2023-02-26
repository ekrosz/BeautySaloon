namespace BeautySaloon.DAL.Entities.ValueObjects.Pagination;

public record PageRequestDto(int PageNumber = 1, int PageSize = 10);
