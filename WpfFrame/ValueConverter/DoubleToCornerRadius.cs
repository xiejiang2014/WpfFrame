using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfFrame.ValueConverter
{
    public class DoubleToCornerRadius : IValueConverter
    {
        public int Division { get; set; } = 2;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                if (double.IsNaN(doubleValue) || double.IsInfinity(doubleValue))
                {
                    throw new ArgumentException("无效的 double 值,无法转换为 CornerRadius 对象");
                }


                return new CornerRadius(doubleValue / Division);
            }

            throw new ArgumentException("无效的 double 值,无法转换为 CornerRadius 对象");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}