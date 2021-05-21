using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfFrame
{
    public static class FrameworkElementExt
    {
        /// <summary>
        /// 对控件截图并返回BitmapSource
        /// </summary>
        /// <param name="element"></param>
        /// <param name="width">默认控件宽度</param>
        /// <param name="height">默认控件高度</param>
        /// <param name="x">默认0</param>
        /// <param name="y">默认0</param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(this FrameworkElement element, int width = -1, int height = -1, int x = 0, int y = 0)
        {
            if (width == -1) width = (int)element.ActualWidth;
            if (height == -1) height = (int)element.ActualHeight;

            var renderTargetBitmap = new RenderTargetBitmap(width, height, x, y, System.Windows.Media.PixelFormats.Default);
            renderTargetBitmap.Render(element);

            return renderTargetBitmap;
        }
    }
}