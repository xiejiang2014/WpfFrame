using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfFrame.ValueConverter
{
    /// <summary>
    /// 将bool转换为Visibility
    /// </summary>
    public class BoolsToVisibilityConverter : IMultiValueConverter
    {
        public BoolsToVisibilityConverter()
        {
        }

        public BoolsToVisibilityConverter(bool useHidden, bool isReversed)
        {
            UseHidden = useHidden;
            IsReversed = isReversed;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //遍历所有值,如果其中有任何一项不符合要求,那么返回不可见值
            foreach (var value in values)
            {
                var val = System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                if (IsReversed) val = !val;
                if (!val) return UseHidden ? Visibility.Hidden : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 是否转换为Hidden,否则转换为Collapsed
        /// </summary>
        public bool UseHidden { get; set; }

        /// <summary>
        /// 是否反转,默认为否.
        /// 默认true转换为Visible,false转换为Hidden或Collapsed(由UseHidden决定);
        /// 反转后false转换为Visible,true转换为Hidden或Collapsed(由UseHidden决定)
        /// </summary>
        public bool IsReversed { get; set; }
    }
}