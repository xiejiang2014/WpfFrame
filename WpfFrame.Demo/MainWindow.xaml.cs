using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfFrame.MessageBox;

namespace WpfFrame.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var uc1 = new UserControl2();

            var messageBoxViewModel = MessageBoxManager.ShowCustomizeMessageBox(
                    uc1);

            //显示后立即播放开启动画
            MessageBoxManager.RunDefaultShownAnimation(uc1);

            uc1.CloseAction = () =>
            {
                //播放关闭动画
                MessageBoxManager.RunDefaultCloseAnimation(
                    uc1,
                    () => MessageBoxManager.CloseMessageBox(messageBoxViewModel)//动画完成时关闭对话框
                );
            };

            await messageBoxViewModel.WaitMessageBoxClose();
        }
    }
}