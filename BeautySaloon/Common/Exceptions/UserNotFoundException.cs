namespace BeautySaloon.Common.Exceptions;

public class UserNotFoundException : EntityNotFoundException
{
    public UserNotFoundException(Guid userId)
        : base("Запрашиваемый пользователь не найден.", userId)
    {
    }
}
