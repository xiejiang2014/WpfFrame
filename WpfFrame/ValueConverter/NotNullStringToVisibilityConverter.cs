using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfFrame.ValueConverter
{
    public class NotNullStringToVisibilityConverter : IValueConverter
    {
        public bool UseHidden { get; set; }

        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var falseValue = UseHidden ? Visibility.Hidden : Visibility.Collapsed;

            if (value == null ||
                value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
            {
                //传入空值
                return IsReversed ? Visibility.Visible : falseValue;
            }

            //传入非空值
            return IsReversed ? falseValue : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("不支持反向转换");
        }
    }
}