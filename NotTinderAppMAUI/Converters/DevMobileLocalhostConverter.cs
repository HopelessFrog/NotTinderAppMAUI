using System.Globalization;

namespace NotTinderAppMAUI.Converters;

public class DevMobileLocalhostConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return " ";
        var url = value as string;
        var index = url.IndexOf('?');
        url = url.Substring(0, index);
        return url.Replace("localhost", "10.0.2.2");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}