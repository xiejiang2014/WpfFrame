using System.Windows;
using System.Windows.Media;

namespace WpfFrame
{
    /// <summary>
    /// HelpMaskLayer.xaml 的交互逻辑
    /// </summary>
    public partial class HelpMaskLayer
    {
        public static readonly DependencyProperty TargetElementProperty = DependencyProperty.Register(
            "TargetElement", typeof(FrameworkElement), typeof(HelpMaskLayer), new PropertyMetadata(default(FrameworkElement), TargetElementPropertyChanged));

        private static void TargetElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HelpMaskLayer helpMaskLayer)
            {
                if (e.NewValue is FrameworkElement frameworkElement)
                {
                    var bitmapSource = frameworkElement
                        .ToBitmapSource((int)helpMaskLayer.ActualWidth, (int)helpMaskLayer.ActualHeight)
                        .InvertBitmapSourceAlpha();
                    helpMaskLayer.OpacityMask = new ImageBrush(bitmapSource);
                }
            }
        }

        public FrameworkElement TargetElement
        {
            get => (FrameworkElement)GetValue(TargetElementProperty);
            set => SetValue(TargetElementProperty, value);
        }

        public HelpMaskLayer()
        {
            InitializeComponent();
        }
    }
}