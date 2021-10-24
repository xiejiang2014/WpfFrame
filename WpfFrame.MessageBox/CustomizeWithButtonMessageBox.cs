using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using WpfFrame.ValueConverter;

namespace WpfFrame.MessageBox
{
    [TemplatePart(Name = "PART_ButtonOK")]
    [TemplatePart(Name = "PART_ButtonYes")]
    [TemplatePart(Name = "PART_ButtonNo")]
    [TemplatePart(Name = "PART_ButtonCancel")]
    [TemplatePart(Name = "PART_ButtonClose")]
    [TemplatePart(Name = "PART_CustomizeContent")]
    public class CustomizeWithButtonMessageBox : Control
    {
        private ButtonBase          _buttonOk;
        private ButtonBase          _buttonYes;
        private ButtonBase          _buttonNo;
        private ButtonBase          _buttonCancel;
        private ButtonBase          _buttonClose;
        private MessageBoxViewModel _messageBoxViewModel;

        private readonly NullToCollapsedConverter _nullToCollapsedConverter = new();

        static CustomizeWithButtonMessageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizeWithButtonMessageBox), new FrameworkPropertyMetadata(typeof(CustomizeWithButtonMessageBox)));
        }

        public CustomizeWithButtonMessageBox()
        {
            DataContextChanged += CustomizeWithButtonMessageBox_DataContextChanged;
        }

        private void CustomizeWithButtonMessageBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MessageBoxViewModel messageBoxViewModel)
            {
                _messageBoxViewModel = messageBoxViewModel;
            }
        }

        public override void OnApplyTemplate()
        {
            _buttonOk = GetTemplateChild("PART_ButtonOK") as ButtonBase;

            if (_buttonOk != null)
            {
                var binding = new Binding()
                              {
                                  Path      = new PropertyPath(nameof(MessageBoxViewModel.OkCommand)),
                                  Converter = _nullToCollapsedConverter
                              };

                _buttonOk.SetBinding(VisibilityProperty, binding);

                binding = new Binding()
                          {
                              Path = new PropertyPath(nameof(MessageBoxViewModel.OkCommand))
                          };

                _buttonOk.SetBinding(ButtonBase.CommandProperty, binding);
            }

            _buttonYes = GetTemplateChild("PART_ButtonYes") as ButtonBase;

            if (_buttonYes != null)
            {
                var binding = new Binding()
                              {
                                  Path      = new PropertyPath(nameof(MessageBoxViewModel.YesCommand)),
                                  Converter = _nullToCollapsedConverter
                              };
                _buttonYes.SetBinding(VisibilityProperty, binding);


                binding = new Binding()
                          {
                              Path = new PropertyPath(nameof(MessageBoxViewModel.YesCommand))
                          };
                _buttonYes.SetBinding(ButtonBase.CommandProperty, binding);
            }

            _buttonNo = GetTemplateChild("PART_ButtonNo") as ButtonBase;

            if (_buttonNo != null)
            {
                _buttonNo.Click += _buttonNo_Click;

                var binding = new Binding()
                              {
                                  Path      = new PropertyPath(nameof(MessageBoxViewModel.NoAction)),
                                  Converter = _nullToCollapsedConverter
                              };

                _buttonNo.SetBinding(VisibilityProperty, binding);
            }

            _buttonCancel = GetTemplateChild("PART_ButtonCancel") as ButtonBase;

            if (_buttonCancel != null)
            {
                _buttonCancel.Click += _buttonCancel_Click;

                var binding = new Binding()
                              {
                                  Path      = new PropertyPath(nameof(MessageBoxViewModel.CancelAction)),
                                  Converter = _nullToCollapsedConverter
                              };

                _buttonCancel.SetBinding(VisibilityProperty, binding);
            }

            _buttonClose = GetTemplateChild("PART_ButtonClose") as ButtonBase;

            if (_buttonClose != null)
            {
                _buttonClose.Click += _buttonClose_Click;

                var binding = new Binding()
                              {
                                  Path      = new PropertyPath(nameof(MessageBoxViewModel.CloseAction)),
                                  Converter = _nullToCollapsedConverter
                              };

                _buttonClose.SetBinding(VisibilityProperty, binding);
            }

            base.OnApplyTemplate();
        }

        //private void _buttonOk_Click(object sender, RoutedEventArgs e)
        //{
        //    _messageBoxViewModel?.OkAction?.Invoke();
        //}

        //private void _buttonYes_Click(object sender, RoutedEventArgs e)
        //{
        //    _messageBoxViewModel?.YesAction?.Invoke();
        //}

        private void _buttonNo_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxViewModel?.NoAction?.Invoke();
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxViewModel?.CancelAction?.Invoke();
        }

        private void _buttonClose_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxViewModel?.CloseAction?.Invoke();
        }
    }
}