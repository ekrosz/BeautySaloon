namespace BeautySaloon.Common.Exceptions;

public class PersonNotFoundException : EntityNotFoundException
{
    public PersonNotFoundException(Guid personId)
        : base("Запрашиваемый клиент не найден.", personId)
    {
    }
}
