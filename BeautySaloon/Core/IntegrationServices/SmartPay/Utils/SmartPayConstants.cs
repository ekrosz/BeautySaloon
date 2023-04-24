namespace BeautySaloon.Core.IntegrationServices.SmartPay.Utils;

public record SmartPayConstants
{
    public const int Type = 0;

    public const string Currency = "RUB";

    public const string Purpose = "Покупка абонементов в салоне красоты Beauty Studio";

    public const string DescriptionPrefix = "Покупка абонементов в салоне красоты Beauty Studio: ";

    public const string Language = "ru-RU";

    public const int TaxSystem = 1;

    public const int TaxType = 6;

    public const string Measure = "шт.";

    public const string OperationType = "payment";

    public const string OperationCode = "QR";

    public const string DateTimeRFC3339Format = "yyyy'-'MM'-'dd'T'HH':'mm':'ss''K";

    public const int ExpireTime = 20;

    public const int NdsRate = 20;

    public record ContactType
    {
        public const string PhoneType = "phone";

        public const string EmailType = "email";
    }

    public record Status
    {
        public const string Created = "created";

        public const string Executed = "executed";

        public const string Confirmed = "confirmed";
    }
}
