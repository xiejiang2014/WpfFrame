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

        private void TextMessageBox_DataContextChanged(object                             sender,
                                                       DependencyPropertyChangedEventArgs e
        )
        {
            if (e.NewValue is MessageBoxViewModel messageBoxViewModel)
            {
                _messageBoxViewModel = messageBoxViewModel;
            }
        }

        public override void OnApplyTemplate()
        {
            _buttonOk = GetTemplateChild("PART_ButtonOK") as ButtonBase;
            BindCommandToButton(
                                _buttonOk,
                                new PropertyPath(nameof(MessageBoxViewModel.OkCommand))
                               );

            _buttonYes = GetTemplateChild("PART_ButtonYes") as ButtonBase;
            BindCommandToButton(
                                _buttonYes,
                                new PropertyPath(nameof(MessageBoxViewModel.YesCommand))
                               );

            

            _buttonNo = GetTemplateChild("PART_ButtonNo") as ButtonBase;
            BindCommandToButton(
                                _buttonNo,
                                new PropertyPath(nameof(MessageBoxViewModel.NoCommand))
                               );
            
            _buttonCancel = GetTemplateChild("PART_ButtonCancel") as ButtonBase;

            BindCommandToButton(
                                _buttonCancel,
                                new PropertyPath(nameof(MessageBoxViewModel.CancelCommand))
                               );

            base.OnApplyTemplate();
        }

        private static void BindCommandToButton(ButtonBase   button,
                                                PropertyPath commandPath
        )
        {
            if (button != null)
            {
                var binding = new Binding()
                              {
                                  Path      = commandPath,
                                  Converter = NullToCollapsedConverter.Default
                              };

                button.SetBinding(VisibilityProperty, binding);

                binding = new Binding()
                          {
                              Path = commandPath
                          };

                button.SetBinding(ButtonBase.CommandProperty, binding);
            }
        }
    }
}