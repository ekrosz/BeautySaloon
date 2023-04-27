namespace BeautySaloon.Core.Utils.Contracts;

public interface IDocumentGenerator<T> where T : class
{
    Task<byte[]> GenerateDocumentAsync(T data);
}
