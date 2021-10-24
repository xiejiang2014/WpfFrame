using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfFrame
{
    public class FilterComboBox : ComboBox
    {
        #region 事件

        public static readonly RoutedEvent FilterTextChangedEvent = EventManager.RegisterRoutedEvent("FilterTextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilterComboBox));

        public event RoutedEventHandler FilterTextChanged
        {
            add => AddHandler(FilterTextChangedEvent, value);
            remove => RemoveHandler(FilterTextChangedEvent, value);
        }

        #endregion 事件

        #region 是否使用延时

        public static readonly DependencyProperty UseDelayProperty = DependencyProperty.Register(
            "UseDelay", typeof(bool), typeof(FilterComboBox), new PropertyMetadata(true));

        public bool UseDelay
        {
            get => (bool)GetValue(UseDelayProperty);
            set => SetValue(UseDelayProperty, value);
        }

        #endregion 是否使用延时

        public bool CanDropDown { get; set; } = true;

        #region 延时长度

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            "Interval", typeof(double), typeof(FilterComboBox), new PropertyMetadata((double)400));

        public double Interval
        {
            get => (double)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        #endregion 延时长度

        #region 过滤文本

        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register(
            "FilterText", typeof(string), typeof(FilterComboBox), new PropertyMetadata(default(string)));

        /// <summary>
        /// 过滤内容.本属性会延时更改
        /// </summary>
        public string FilterText
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }

        #endregion 过滤文本

        #region 构造

        static FilterComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterComboBox), new FrameworkPropertyMetadata(typeof(FilterComboBox)));
        }

        public FilterComboBox()
        {
            IsEditable = true;

            AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(FilterComboBox_TextChanged));

            _delayer.Interval = Interval;
            _delayer.Elapsed += Delayer_Elapsed;
        }

        #endregion 构造

        /// <summary>
        /// 延时器
        /// </summary>
        private readonly System.Timers.Timer _delayer = new();

        private string _oldString = "";

        private void FilterComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_oldString.Trim() == Text.Trim()) return;//避免字符串没有实际变化时的无用搜索

            _oldString = Text;

            if (UseDelay)
            {
                //延时
                _delayer.Enabled = false;
                _delayer.Enabled = true;
            }
            else
            {
                //不延时
                UpdateFilter();
            }
        }

        private void Delayer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _delayer.Enabled = false;

            Dispatcher.Invoke(UpdateFilter);
        }

        private void UpdateFilter()
        {
            FilterText = Text;
            RaiseEvent(new TextChangedEventArgs(FilterTextChangedEvent, UndoAction.None));
        }
    }
}