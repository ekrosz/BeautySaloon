using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.Core.Exceptions;
public class UniquePropertyException : Exception
{
    public UniquePropertyException(Type entityType, string propertyName)
        : base($"Запись типа {entityType.Name} не удовлетворяет ограничению уникальности по полю {propertyName}")
    {
    }
}
