using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfFrame.Demo
{
    /// <summary>
    /// MessageBoxMemo.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxMemo
    {
        public MessageBoxMemo()
        {
            InitializeComponent();
        }


        private async void WaitingButton_OnClick(object          sender,
                                                 RoutedEventArgs e
        )
        {
            var messageBoxViewModel = MessageBoxManager.ShowWaitingMessageBox("内容", "标题");

            await Task.Delay(1000);

            MessageBoxManager.CloseMessageBox(messageBoxViewModel);
        }

        private async void WaitingButton2_OnClick(object          sender,
                                                  RoutedEventArgs e
        )
        {
            var messageBoxResults = await MessageBoxManager.ShowTextMessageBox("消息内容");
        }

        private async void AniButton_OnClick(object          sender,
                                             RoutedEventArgs e
        )
        {
            var content = new Content2();

            var messageBoxViewModel = MessageBoxManager.ShowCustomizeMessageBox(
                                                                                content
                                                                               );

            //显示后立即播放开启动画
            MessageBoxManager.RunDefaultShownAnimation(content);

            content.CloseAction = () =>
                                  {
                                      //播放关闭动画
                                      MessageBoxManager.RunDefaultCloseAnimation(
                                           content,
                                           () => MessageBoxManager.CloseMessageBox(messageBoxViewModel) //动画完成时关闭对话框
                                          );
                                  };

            await messageBoxViewModel.WaitMessageBoxClose();
        }
    }
}