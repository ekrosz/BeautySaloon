namespace BeautySaloon.Common.Exceptions;

public class InvoiceNotFoundException : EntityNotFoundException
{
    public InvoiceNotFoundException(Guid invoiceId)
        : base("Запрашиваемое движение не найдено.", invoiceId)
    {
    }
}
