using System;
using System.Windows;
using System.Windows.Controls;

namespace Xiejiang.DataGridEx.WPF
{
    /// <inheritdoc />
    /// <summary>
    /// 带有延时功能的搜索框
    /// </summary>
    public class FilterBox : TextBox
    {
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            "Interval", typeof(double), typeof(FilterBox), new PropertyMetadata((double)400));

        public double Interval
        {
            get => (double)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register(
            "FilterText", typeof(string), typeof(FilterBox), new PropertyMetadata(default(string)));

        /// <summary>
        /// 过滤内容.本属性会延时更改
        /// </summary>
        public string FilterText
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }

        public static readonly DependencyProperty UseDelayProperty = DependencyProperty.Register(
            "UseDelay", typeof(bool), typeof(FilterBox), new PropertyMetadata(true));

        public bool UseDelay
        {
            get => (bool)GetValue(UseDelayProperty);
            set => SetValue(UseDelayProperty, value);
        }

        static FilterBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterBox), new FrameworkPropertyMetadata(typeof(FilterBox)));
        }

        public event EventHandler FilterTextChanged;

        /// <summary>
        /// 延时器
        /// </summary>
        private readonly System.Timers.Timer _delayer = new();

        public FilterBox()
        {
            TextChanged += FilterBox_TextChanged;

            _delayer.Interval = Interval;
            _delayer.Elapsed += Delayer_Elapsed;
        }

        private string _oldString = "";

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_oldString.Trim() == Text.Trim()) return;//避免字符串没有实际变化时的无用搜索

            _oldString = Text;

            if (UseDelay)
            {
                //延时搜索
                _delayer.Enabled = false;
                _delayer.Enabled = true;
            }
            else
            {
                UpdateFilter();
            }
        }

        private void Delayer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _delayer.Enabled = false;

            Dispatcher?.Invoke(UpdateFilter);
        }

        private void UpdateFilter()
        {
            FilterText = Text;
            FilterTextChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}