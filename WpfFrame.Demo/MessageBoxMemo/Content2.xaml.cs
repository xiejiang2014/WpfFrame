using System;
using System.Windows;

namespace WpfFrame.Demo
{
    /// <summary>
    /// UserControl2.xaml 的交互逻辑
    /// </summary>
    public partial class Content2
    {
        public Action CloseAction { get; set; }

        public Content2()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CloseAction?.Invoke();
        }
    }
}