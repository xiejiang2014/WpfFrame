using System.Timers;

namespace WpfFrame
{
    public class Delayer
    {
        /// <summary>
        /// 延时器
        /// </summary>
        private readonly Timer _delayer = new Timer();

        public Delayer()
        {
            _delayer.Interval = 100;
            _delayer.Elapsed += Delayer_Elapsed;
        }

        /// <summary>
        /// 延时毫秒数
        /// </summary>
        public double Millisecond { get => _delayer.Interval; set => _delayer.Interval = value; }

        private void Delayer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _delayer.Enabled = false;
        }
    }
}