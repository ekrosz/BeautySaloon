using Refit;
using System.Reflection;

namespace BeautySaloon.Api.Dto.Common;

public class CustomUrlParameterFormatter : IUrlParameterFormatter
{
    private readonly DefaultUrlParameterFormatter _defaultUrlParameterFormatter = new();

    public string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormats.DateTimeWithTimeZoneFormat);
        }

        return _defaultUrlParameterFormatter.Format(value, attributeProvider, type);
    }
}

