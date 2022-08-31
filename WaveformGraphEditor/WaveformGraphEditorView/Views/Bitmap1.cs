using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WaveformGraphEditorView.Views
{
    public class Bitmap1: FrameworkElement
    {
        WriteableBitmap _writeableBitmap;

        System.Drawing.Graphics graphics;

        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.White, 1);

        public Bitmap1(){
            _writeableBitmap = new WriteableBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Bgr32, null);
            using (Bitmap backBufferBitmap = new((int)Width, (int)Height, _writeableBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, _writeableBitmap.BackBuffer))
            {
                graphics = Graphics.FromImage(backBufferBitmap);
                
            }
        }

        public void Draw(List<PointF> PointFs)
        {
            _writeableBitmap.Lock();
            graphics.Clear(System.Drawing.Color.Black);

            graphics.DrawLines(pen, PointFs.ToArray());


            _writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)Width, (int)Height));
            _writeableBitmap.Unlock();
        }

    }
}
