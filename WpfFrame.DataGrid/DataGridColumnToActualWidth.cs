using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Xiejiang.DataGridEx.WPF
{
    public class DataGridColumnToActualWidth : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //要求第一个值是Visibility,第二个值是ActualWidth
            return (Visibility)values[0] == Visibility.Visible ? new GridLength((double)values[1]) : new GridLength(0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}