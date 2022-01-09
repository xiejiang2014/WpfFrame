using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfFrame.MessageBox
{
    public class MessageBoxViewModel : NotificationObject
    {
        public MessageBoxTypes MessageBoxType { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public Visual CustomizeContent { get; set; }

        public MessageBoxViewModel()
        {
            OkButtonContent     = OkButtonDefaultContent;
            YesButtonContent    = YesButtonDefaultContent;
            NoButtonContent     = NoButtonDefaultContent;
            CancelButtonContent = CancelButtonDefaultContent;
        }

        #region Ok

        public static object OkButtonDefaultContent { get; set; } = "确定";

        public object   OkButtonContent { get; set; }
        public ICommand OkCommand       { get; set; }

        #endregion

        #region Yes

        public static object   YesButtonDefaultContent { get; set; } = "是";
        public        object   YesButtonContent        { get; set; }
        public        ICommand YesCommand              { get; set; }

        #endregion

        #region No

        public static object   NoButtonDefaultContent { get; set; } = "否";
        public        object   NoButtonContent        { get; set; }
        public        ICommand NoCommand              { get; set; }

        #endregion

        #region Cancel

        public static object   CancelButtonDefaultContent { get; set; } = "取消";
        public        object   CancelButtonContent        { get; set; }
        public        ICommand CancelCommand              { get; set; }

        #endregion

        public static object   CloseButtonDefaultContent { get; set; } = "关闭";
        public        object   CloseButtonContent        { get; set; }
        public        ICommand CloseCommand              { get; set; }


        /// <summary>
        /// 是否已关闭
        /// </summary>
        public bool IsClosed { get; internal set; }

        /// <summary>
        /// 是否已取消.此属性在等待框中有效.
        /// </summary>
        public bool IsCanceled { get; set; }

        public Task WaitMessageBoxClose()
        {
            return Task.Run(() =>
                            {
                                while (!IsClosed)
                                {
                                    Thread.Sleep(1);
                                }
                            });
        }
    }
}