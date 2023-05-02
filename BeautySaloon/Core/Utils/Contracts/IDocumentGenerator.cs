using BeautySaloon.Api.Dto.Common;

namespace BeautySaloon.Core.Utils.Contracts;

public interface IDocumentGenerator<T> where T : class
{
    Task<FileResponseDto> GenerateDocumentAsync(string fileName, T data);
}
