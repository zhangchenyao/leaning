using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using UniTrendChart.Helper;
using WaveformGraphEditor.VModel;

namespace WaveformGraphEditor.Views
{
    /// <summary>
    /// WaveEditor.xaml 的交互逻辑
    /// </summary>
    public partial class WaveEditor : UserControl
    {
        public EventHandler<int> RemoveModeEventHandel;

        Dictionary<
            WorkType, Action<object, MouseEventArgs>> waveEditors;

        WriteableBitmap _writeableBitmap;

        public Bitmap backBitmap { get; private set; }

        System.Drawing.Graphics graphics;

        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.White, 1);


        private Line[] lins;
        public WaveEditor()
        {
            InitializeComponent();
            waveEditors = new Dictionary<WorkType, Action<object, MouseEventArgs>>
            {
                { WorkType.Editor, Canvas_Editor_PreviewMouseMove }
            };

            
            int width = (int)Canvas1.Width;
            int height = (int)Canvas1.Height;
           
            //using (Bitmap backBufferBitmap = new(width,height, _writeableBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, _writeableBitmap.BackBuffer))
            //{
            //    //graphics = Graphics.FromImage(backBufferBitmap);
            //    //graphics.SmoothingMode =  System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //    //graphics.CompositingQuality =  System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //}

            //lins = new Line[(int)Canvas1.Width];
            //for(int i = 0; i < lins.Length; i++)
            //{
            //    lins[i] = new Line()
            //    {
            //        Stroke= new SolidColorBrush(Colors.White),
            //        StrokeThickness=2
            //    };

            //    Canvas1.Children.Add(lins[i]);
            //}
        }

        public void SetDataContext(object mode)
        {
            DataContext = mode;
            
        }

        internal void Closed(object? sender, EventArgs e)
        {
            RemoveModeEventHandel?.Invoke(this,this.DataContext.GetHashCode());
        }


        public void Draw(List<PointF> PointFs)
        {

            int width = (int)Canvas1.Width;
            int height = (int)Canvas1.Height;
            _writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            image.Source = _writeableBitmap;
            backBitmap = new(width, height, _writeableBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, _writeableBitmap.BackBuffer);
            graphics = Graphics.FromImage(backBitmap);
            _writeableBitmap.Lock();
            graphics.DrawLines(pen, PointFs.ToArray());
            _writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            _writeableBitmap.Unlock();
            
            
        }
        private void Canvas_Editor_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                //if (DataContext is WaveEditorMode mode&&e.LeftButton== MouseButtonState.Pressed)
                //{
                //    mode.NowPoint = e.GetPosition(Canvas1);

                //    Point point = mode.points[0].Item1;

                //    var l1 = lins[(int)point.X-1];
                //    l1.X2 = point.X;
                //    l1.Y2 = point.Y;
                //    foreach(var item in mode.points)
                //    {
                //        if(item.Item2.X>=0)
                //        {

                //            point = item.Item2;
                //        }else
                //        {
                //            Line line = lins[(int)item.Item1.X];
                            
                //            line.X1 =(int)point.X;
                //            line.X2 = (int)item.Item1.X;
                //            line.Y1 =(int)point.Y;
                //            line.Y2 = (int)item.Item1.Y;

                //            point = item.Item1;
                //        }
                        
                //    }

                //  var l=  lins[(int)mode.points[mode.points.Length - 1].Item1.X+1];
                //    if (l.X1 <= 0)
                //        return;
                //    l.X2 = l.X1;
                //    l.Y2= l.Y1;
                //    l.X1 = (int)point.X;
                //    l.Y2=(int)point.Y;
                //}


                if (DataContext is WaveEditorMode mode && e.LeftButton == MouseButtonState.Pressed)
                {
                   var point= e.GetPosition(Canvas1);
                    if (point.X < 0)
                        return;

                    mode.NowPoint = point;

                    Draw(mode.PointFs);
                }
                    
            }catch(Exception)
            {

            }
            
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(DataContext is WaveEditorMode mode)
            {
                if(waveEditors.ContainsKey(mode.WorkType))
                waveEditors[mode.WorkType]?.Invoke(sender, e);
            }
           
        }
    }
}
