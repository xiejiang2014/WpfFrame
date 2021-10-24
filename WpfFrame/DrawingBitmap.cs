using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace WpfFrame
{
    /// <summary>
    /// 为一个WriteableBitmap创建关联的Bitmap,以便通gdi+操作wpf图像
    /// </summary>
    public class DrawingBitmap : NotificationObject, IDisposable
    {
        public object          LockObject      { get; } = new();
        public WriteableBitmap WriteableBitmap { get; private set; }
        public Bitmap          Bitmap          { get; private set; }
        public Graphics        Graphics        { get; private set; }

        public DrawingBitmap(string fileName) : this(new Uri(fileName, UriKind.RelativeOrAbsolute))
        {
        }

        public DrawingBitmap(Uri uri) : this(new BitmapImage(uri))
        {
        }

        public DrawingBitmap(BitmapSource bitmapSource) : this(new WriteableBitmap(bitmapSource))
        {
        }

        public DrawingBitmap(int width, int height) : this(
            new WriteableBitmap(
                width,
                height,
                96,
                96,
                PixelFormats.Bgra32,
                null)
        )
        {
        }

        public DrawingBitmap(WriteableBitmap writeableBitmap)
        {
            Load(writeableBitmap);
        }

        private void Load(WriteableBitmap writeableBitmap)
        {
            lock (LockObject)
            {
                WriteableBitmap = writeableBitmap;

                WriteableBitmap.Lock();

                Bitmap = new Bitmap(
                    WriteableBitmap.PixelWidth,
                    WriteableBitmap.PixelHeight,
                    WriteableBitmap.BackBufferStride,
                    PixelFormat.Format32bppArgb,
                    WriteableBitmap.BackBuffer);

                Graphics = Graphics.FromImage(Bitmap);

                WriteableBitmap.Unlock();
            }
        }

        public void Refresh(int x = 0, int y = 0, int width = -1, int height = -1)
        {
            lock (LockObject)
            {
                WriteableBitmap?.Dispatcher?.BeginInvoke(new Action(() =>
                {
                    if (width == -1)
                    {
                        width = WriteableBitmap.PixelWidth;
                    }

                    if (height == -1)
                    {
                        height = WriteableBitmap.PixelHeight;
                    }

                    if (x > WriteableBitmap.PixelWidth) return;
                    if (y > WriteableBitmap.PixelHeight) return;
                    WriteableBitmap.Lock();

                    WriteableBitmap.AddDirtyRect(
                        new Int32Rect(
                            x,
                            y,
                            width,
                            height)
                    );

                    WriteableBitmap.Unlock();
                }));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<挂起>")]
        public void Dispose()
        {
            lock (LockObject)
            {
                Graphics?.Dispose();
                Bitmap?.Dispose();
            }
        }
    }
}