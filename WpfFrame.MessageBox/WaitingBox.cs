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
        private          ButtonBase               _buttonCancel;
        private          MessageBoxViewModel      _messageBoxViewModel;
        private readonly NullToCollapsedConverter _nullToCollapsedConverter = new();

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