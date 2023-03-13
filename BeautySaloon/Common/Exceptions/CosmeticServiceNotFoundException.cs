namespace BeautySaloon.Common.Exceptions;

public class CosmeticServiceNotFoundException : EntityNotFoundException
{
    public CosmeticServiceNotFoundException(Guid cosmeticServiceId)
        : base("Запрашиваемая косметическая услуга не найдена.", cosmeticServiceId)
    {
    }
}
