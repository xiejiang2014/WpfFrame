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

        #region Ok

        public object   OkButtonContent { get; set; } = "确定";
        public ICommand OkCommand       { get; set; }

        #endregion

        #region Yes

        public object   YesButtonContent { get; set; } = "是";
        public ICommand YesCommand       { get; set; }

        #endregion

        #region No

        public Action NoAction        { get; set; }
        public object NoButtonContent { get; set; } = "否";

        #endregion

        #region Cancel

        public Action CancelAction        { get; set; }
        public object CancelButtonContent { get; set; } = "取消";

        #endregion

        public Action CloseAction { get; set; }

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