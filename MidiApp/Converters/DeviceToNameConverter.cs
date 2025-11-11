using System;
using System.Globalization;
using System.Windows.Data;
using MidiApp.Models;

namespace MidiApp.Converters;

public class DeviceToNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Device device)
        {
            return device.Name;
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
