using Avalonia.Data.Converters;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.ValueConverters;

public class IntToFormattedStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value is int) ? $"{value:n0}" : BindingOperations.DoNothing;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            if (int.TryParse(s, out int i))
            {
                return i;
            }

            return BindingOperations.DoNothing;
        }
        else
        {
            return BindingOperations.DoNothing;
        }
    }
}
