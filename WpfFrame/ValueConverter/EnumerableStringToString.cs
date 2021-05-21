using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WpfFrame.ValueConverter
{
    public class EnumerableStringToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IEnumerable strList)) throw new ApplicationException("对象类型错误.");

            var sb = new StringBuilder();
            foreach (var s in strList)
            {
                sb.AppendLine(s.ToString());
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}