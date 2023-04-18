namespace BeautySaloon.Core.IntegrationServices.SmartPay.Dto;

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

    public record ContactType
    {
        public const string PhoneType = "phone";

        public const string EmailType = "email";
    }
}
