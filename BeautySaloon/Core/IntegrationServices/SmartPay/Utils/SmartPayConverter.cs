using BeautySaloon.DAL.Entities;

namespace BeautySaloon.Core.IntegrationServices.SmartPay.Utils;

public static class SmartPayConverter
{
    public static string ToRfc3999Format(DateTime dateTime)
        => dateTime.ToString(SmartPayConstants.DateTimeRFC3339Format);

    public static int ToCentsAmount(decimal amount)
        => Convert.ToInt32(amount * 100);

    public static string ToSmartPayPhoneFormat(string phone)
        => phone[2..];
}
