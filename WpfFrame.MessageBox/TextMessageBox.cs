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
    public class TextMessageBox : Control
    {
        private ButtonBase          _buttonOk;
        private ButtonBase          _buttonYes;
        private ButtonBase          _buttonNo;
        private ButtonBase          _buttonCancel;
        private MessageBoxViewModel _messageBoxViewModel;

        private readonly NullToCollapsedConverter _nullToCollapsedConverter = new();

        static TextMessageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextMessageBox), new FrameworkPropertyMetadata(typeof(TextMessageBox)));
        }

        public TextMessageBox()
        {
            DataContextChanged += TextMessageBox_DataContextChanged;
        }

        private void TextMessageBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                //_buttonOk.Click += _buttonOk_Click;

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
                //_buttonYes.Click += _buttonYes_Click;

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
    }
}