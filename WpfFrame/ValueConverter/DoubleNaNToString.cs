using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfFrame.ValueConverter
{
    public class DoubleNaNToString : IValueConverter
    {
        public string NaNText { get; set; } = "无效值";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (double.IsNaN((double) value) || double.IsInfinity((double) value)))
            {
                return NaNText;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class NaNToDoubleZero : IValueConverter
    {
        public string NaNText { get; set; } = "无效值";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                if (double.IsNaN(doubleValue))
                {
                    return 0d;
                }

                return doubleValue;
            }


            throw new ArgumentException("无效的输入值,无法转换为 double 对象");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}