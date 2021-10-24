//////using System;
//////using System.Drawing;
//////using System.Globalization;
//////using System.Windows;
//////using System.Windows.Data;

//////namespace WpfFrame.MessageBox
//////{
//////    internal class MessageLayerBackgroundConverter : IValueConverter
//////    {
//////        public static MessageLayerBackgroundConverter Default { get; }=new MessageLayerBackgroundConverter();

//////        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//////        {
//////            if (value is MessageBoxViewModel messageBoxViewModel)
//////            {
//////                return messageBoxViewModel.MessageBoxType switch
//////                       {
//////                           MessageBoxTypes.Waiting             => (Application.Current.FindResource("WaitingMessageBackground") as Brush),
//////                           MessageBoxTypes.TextMessage         => (Application.Current.FindResource("TextMessageBackground") as Brush),
//////                           MessageBoxTypes.Customize           => (Application.Current.FindResource("CustomizeBackground") as Brush),
//////                           MessageBoxTypes.CustomizeWithButton => (Application.Current.FindResource("CustomizeWithButtonBackground") as Brush),
//////                           _                                   => throw new ApplicationException("不支持的 MessageBoxTypes.")
//////                       };
//////            }

//////            throw new ApplicationException("不支持的转换类型.");
//////        }

//////        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//////        {
//////            throw new NotImplementedException();
//////        }
//////    }
//////}