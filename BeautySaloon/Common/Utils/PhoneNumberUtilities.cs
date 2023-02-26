using PhoneNumbers;

namespace BeautySaloon.Common.Utils;

public static class PhoneNumberUtilities
{
    public const string DefaultRegion = "RU";

    private static readonly PhoneNumberUtil PhoneNumberUtil = PhoneNumberUtil.GetInstance();

    public static string Normilize(string phoneNumber)
    {
       var parsedPhone = PhoneNumberUtil.Parse(phoneNumber, DefaultRegion);

        return PhoneNumberUtil.Format(parsedPhone, PhoneNumberFormat.E164);
    }

    public static bool IsValid(string phoneNumber)
    {
        try
        {
            _ = PhoneNumberUtil.Parse(phoneNumber, DefaultRegion);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
