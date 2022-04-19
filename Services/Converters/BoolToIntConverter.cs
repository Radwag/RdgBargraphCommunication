using System;
using System.Globalization;
using System.Windows.Data;

namespace Services.Converters
{
    public class BoolToIntConverter: IValueConverter
    {
        public object Convert(object value ,Type targetType ,object parameter ,CultureInfo culture)
        {
            int? valueIntiger = value as int?;

            return valueIntiger == 1;
        }

        public object ConvertBack(object value ,Type targetType ,object parameter ,CultureInfo culture)
        {
            bool? valueBool = value as bool?;

            return valueBool == true ? 1 : 0;
        }
    }
}