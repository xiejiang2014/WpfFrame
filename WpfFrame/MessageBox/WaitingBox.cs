using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using WpfFrame.ValueConverter;

namespace WpfFrame.MessageBox
{
    [TemplatePart(Name = "PART_ButtonCancel")]
    public class WaitingBox : Control
    {
        private ButtonBase _buttonCancel;
        private MessageBoxViewModel _messageBoxViewModel;
        private readonly NullToCollapsedConverter _nullToCollapsedConverter = new NullToCollapsedConverter();

        static WaitingBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitingBox), new FrameworkPropertyMetadata(typeof(WaitingBox)));
        }

        public WaitingBox()
        {
            DataContextChanged += WaitingBox_DataContextChanged;
        }

        private void WaitingBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MessageBoxViewModel messageBoxViewModel)
            {
                _messageBoxViewModel = messageBoxViewModel;
            }
        }

        public override void OnApplyTemplate()
        {
            _buttonCancel = GetTemplateChild("PART_ButtonCancel") as ButtonBase;

            if (_buttonCancel != null)
            {
                _buttonCancel.Click += _buttonCancel_Click;

                var binding = new Binding()
                {
                    Path = new PropertyPath(nameof(MessageBoxViewModel.CancelAction)),
                    Converter = _nullToCollapsedConverter
                };

                _buttonCancel.SetBinding(VisibilityProperty, binding);
            }

            base.OnApplyTemplate();
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxViewModel?.CancelAction?.Invoke();
        }
    }
}