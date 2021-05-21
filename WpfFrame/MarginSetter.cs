using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace WpfFrame
{
    /// <summary>
    /// 容器子项目边距设置器,来源:https://gist.github.com/angularsen/90040fb174f71c5ab3ad
    /// </summary>
    public class MarginSetter
    {
        private static Thickness GetLastItemMargin(Panel obj)
        {
            return (Thickness)obj.GetValue(LastItemMarginProperty);
        }

        [UsedImplicitly]
        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        private static void MarginChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Make sure this is put on a panel
            var panel = sender as Panel;
            if (panel == null) return;

            // Avoid duplicate registrations
            panel.Loaded -= OnPanelLoaded;
            panel.Loaded += OnPanelLoaded;

            if (panel.IsLoaded)
            {
                OnPanelLoaded(panel, null);
            }
        }

        private static void OnPanelLoaded(object sender, RoutedEventArgs e)
        {
            var panel = (Panel)sender;

            // Go over the children and set margin for them:
            for (var i = 0; i < panel.Children.Count; i++)
            {
                UIElement child = panel.Children[i];
                var fe = child as FrameworkElement;
                if (fe == null) continue;

                bool isLastItem = i == panel.Children.Count - 1;
                fe.Margin = isLastItem ? GetLastItemMargin(panel) : GetMargin(panel);
            }
        }

        [UsedImplicitly]
        public static void SetLastItemMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(LastItemMarginProperty, value);
        }

        [UsedImplicitly]
        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for Margin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(MarginSetter),
                new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

        public static readonly DependencyProperty LastItemMarginProperty =
            DependencyProperty.RegisterAttached("LastItemMargin", typeof(Thickness), typeof(MarginSetter),
                new UIPropertyMetadata(new Thickness(), MarginChangedCallback));
    }
}