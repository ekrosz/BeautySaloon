namespace BeautySaloon.Api.Dto.Common;

public record ByIdWithDataRequestDto<T>(Guid Id, T Data);