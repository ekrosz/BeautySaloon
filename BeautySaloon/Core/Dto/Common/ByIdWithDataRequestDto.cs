namespace BeautySaloon.Core.Dto.Common;

public record ByIdWithDataRequestDto<T>(Guid Id, T Data);