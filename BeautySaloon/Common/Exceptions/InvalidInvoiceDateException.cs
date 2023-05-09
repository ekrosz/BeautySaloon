using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class InvalidInvoiceDateException : BusinessExceptions
{
    public InvalidInvoiceDateException(DateTime invoiceDate)
        : base(HttpStatusCode.Conflict, $"Движения позднее {invoiceDate:d} уже существуют")
    {
    }
}

