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
            var commandPath = new PropertyPath(nameof(MessageBoxViewModel.OkCommand));
            BindCommandToButton(_buttonOk, commandPath);

            _buttonYes = GetTemplateChild("PART_ButtonYes") as ButtonBase;
            commandPath = new PropertyPath(nameof(MessageBoxViewModel.YesCommand));
            BindCommandToButton(_buttonYes, commandPath);

            _buttonNo   = GetTemplateChild("PART_ButtonNo") as ButtonBase;
            commandPath = new PropertyPath(nameof(MessageBoxViewModel.NoCommand));
            BindCommandToButton(_buttonNo, commandPath);

            _buttonCancel = GetTemplateChild("PART_ButtonCancel") as ButtonBase;
            commandPath   = new PropertyPath(nameof(MessageBoxViewModel.CancelCommand));
            BindCommandToButton(_buttonCancel, commandPath);
            
            _buttonClose = GetTemplateChild("PART_ButtonClose") as ButtonBase;
            commandPath  = new PropertyPath(nameof(MessageBoxViewModel.CloseCommand));
            BindCommandToButton(_buttonClose, commandPath);

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