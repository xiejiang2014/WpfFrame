using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfFrame
{
    public static class BitmapSourceExt
    {
        /// <summary>
        /// 将指定BitmapSource的透明通道反转
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static WriteableBitmap InvertBitmapSourceAlpha(this BitmapSource bitmapSource)
        {
            var writeableBitmap = new WriteableBitmap(bitmapSource);

            var stride = writeableBitmap.PixelWidth * writeableBitmap.Format.BitsPerPixel * 8;

            var pixels = new byte[writeableBitmap.PixelHeight * stride];
            writeableBitmap.CopyPixels(pixels, stride, 0);

            if (writeableBitmap.Format == System.Windows.Media.PixelFormats.Pbgra32)
            {
                //注意这里alpha通道是放在第4通道的,所以是从3开始
                for (var i = 3; i < pixels.Length; i += 4)
                {
                    pixels[i] = (byte)(255 - pixels[i]);
                }
            }
            else
            {
                throw new FormatException("不支持的图像数据格式");
            }

            writeableBitmap.WritePixels(
                new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight),
                pixels,
                stride,
                0
            );

            return writeableBitmap;
        }

        /// <summary>
        /// 从资源获取图片
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="decodeWidth"></param>
        /// <returns></returns>
        public static BitmapImage GetBitmapFromResources(string uriString, int decodeWidth = 0)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            if (decodeWidth > 0)
            {
                bitmap.DecodePixelWidth = decodeWidth;
            }

            var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);

            var sriStream = Application.GetResourceStream(uri);
            if (sriStream == null)
            {
                throw new ArgumentException(@"无效的uri,无法找到uri指定的资源", nameof(uriString));
            }

            using var stream = sriStream.Stream;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        /// <summary>
        /// 从资源或uri获取图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="decodeWidth"></param>
        /// <returns></returns>
        public static BitmapImage GetBitmapFromFile(string fileName, int decodeWidth = 0)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            if (decodeWidth > 0)
            {
                bitmap.DecodePixelWidth = decodeWidth;
            }

            var uri = new Uri(fileName, UriKind.RelativeOrAbsolute);
            using Stream stream = new MemoryStream(File.ReadAllBytes(uri.LocalPath));
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        #region 水印

        /// <summary>
        /// 为指定的图片添加水印
        /// </summary>
        /// <param name="watermark"></param>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static void AddToPictureFileAsWatermark(this ImageSource watermark, string inputFile, string outputFile)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(inputFile);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            var renderTargetBitmap = bitmap.AddWatermark(watermark);

            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            if (File.Exists(outputFile))
                File.Delete(outputFile);

            var outputDirName = Path.GetDirectoryName(outputFile);
            if (!Directory.Exists(outputDirName))
            {
                Directory.CreateDirectory(outputDirName);
            }

            using var file = new FileStream(outputFile, FileMode.Create);
            bitmapEncoder.Save(file);
        }

        public static RenderTargetBitmap AddWatermark(this BitmapImage bitmap, ImageSource watermark)
        {
            var result = new RenderTargetBitmap(
                bitmap.PixelWidth,
                bitmap.PixelHeight,
                bitmap.DpiX,
                bitmap.DpiY,
                PixelFormats.Default);

            var drawingVisual = new DrawingVisual();
            using var drawingContext = drawingVisual.RenderOpen();

            //用原图打底
            drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.Width, bitmap.Height));

            var scale = Math.Min(
                    bitmap.Width / watermark.Width,
                    bitmap.Height / watermark.Height);

            if (scale > 1) scale = 1;

            var scaledWidth = watermark.Width * scale;
            var scaledHeight = watermark.Height * scale;

            for (var x = 0; x < bitmap.Width / watermark.Width + 1; x++)
            {
                for (var y = 0; y < bitmap.Height / watermark.Height + 1; y++)
                {
                    var targetRest = new Rect(
                        x * scaledWidth,
                        y * scaledHeight,
                        scaledWidth,
                        scaledHeight);

                    drawingContext.DrawImage(
                        watermark,
                        targetRest);
                }
            }

            drawingContext.Close();
            result.Render(drawingVisual);
            return result;
        }

        #endregion 水印
    }
}