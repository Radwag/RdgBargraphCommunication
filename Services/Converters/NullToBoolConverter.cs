using System;
using System.Globalization;
using System.Windows.Data;

namespace Services.Converters
{
    public class NullToBoolConverter: IValueConverter
    {
        public object Convert(object value ,Type targetType ,object parameter ,CultureInfo culture)
        {
            if (value == null)
                return false;
            else
                return true;
        }

        public object ConvertBack(object value ,Type targetType ,object parameter ,CultureInfo culture)
        {
            return null;
        }
    }
}