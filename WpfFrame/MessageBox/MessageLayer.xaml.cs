using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace WpfFrame.MessageBox
{
    /// <summary>
    /// MessageLayer.xaml 的交互逻辑
    /// </summary>
    public partial class MessageLayer:INotifyPropertyChanged
    {
        public MessageLayer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前正显示的对话框view的viewModel
        /// </summary>
        public MessageBoxViewModel MessageBoxViewModel { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}