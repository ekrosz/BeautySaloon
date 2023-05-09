using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class NotEnoughMaterialException : BusinessExceptions
{
    public NotEnoughMaterialException(string materialName, double count)
        : base(HttpStatusCode.Conflict, $"Недостаточно материала \"{materialName}.\". Текущий остаток: {count}.")
    {
    }
}
