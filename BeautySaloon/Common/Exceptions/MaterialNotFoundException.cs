namespace BeautySaloon.Common.Exceptions;

public class MaterialNotFoundException : EntityNotFoundException
{
    public MaterialNotFoundException(Guid materialId)
        : base("Запрашиваемый материал не найден.", materialId)
    {
    }
}
