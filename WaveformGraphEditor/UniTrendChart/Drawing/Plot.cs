using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Media = System.Windows.Media;
using System.Windows.Media.Imaging;
using GDI = System.Drawing;
using UniTrendChart.Helper;
using System.Collections.Generic;
using UniTrendChart.Data;
using System;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Media;

namespace UniTrendChart.Drawing
{
    public class Plot : FrameworkElement
    {
        private int width = 0;//图片分辨率
        private int height = 0;
        private WriteableBitmap bitmap;
        private GDI.Graphics graphics1;
        internal byte[] colorTable;
        internal byte[] pxiTable;
        internal bool isData = true;
        #region 依赖属性
        #region 背景颜色
        /// <summary>
        /// 图标背景颜色
        /// </summary>
        public Media.Color Background
        {
            get { return (Media.Color)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Media.Color), typeof(Plot), 
                new PropertyMetadata((Media.Color)Media.ColorConverter.ConvertFromString("#232426")));
        #endregion

        #region 迹线信息
        ///// <summary>
        ///// 图表迹线信息
        ///// </summary>
        //public List<Line> Lines
        //{
        //    get { return (List<Line>)GetValue(MyPropertyProperty); }
        //    set { SetValue(MyPropertyProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MyPropertyProperty =
        //    DependencyProperty.Register("MyProperty", typeof(List<Line>), typeof(Plot), new PropertyMetadata(new List<Line>()));
        #endregion

        #region 是否自适应
        /// <summary>
        /// 是否自动缩放
        /// </summary>
        public bool IsAutoScal
        {
            get { return (bool)GetValue(IsAutoScalProperty); }
            set { SetValue(IsAutoScalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAutoScal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAutoScalProperty =
            DependencyProperty.Register("IsAutoScal", typeof(bool), typeof(Plot), new PropertyMetadata(true));


        #endregion

        #region 绘图flag
        public object RenderFlag
        {
            get { return (object)GetValue(RenderFlagProperty); }
            set { SetValue(RenderFlagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenderFlag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderFlagProperty =
            DependencyProperty.Register("RenderFlag", typeof(object), typeof(Plot), new PropertyMetadata(new object(), new PropertyChangedCallback(RenderFlagChanged)));

        private static void RenderFlagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Plot c = d as Plot;
            //c.IsCursorTimeVisible = false;
            //c.IsCursorVoltageVisible = false;
            c.Draw();
        }
        #endregion

        #region 是否显示标尺
        public bool IsCursorTimeVisible
        {
            get { return (bool)GetValue(IsCursorTimeVisibleProperty); }
            set { SetValue(IsCursorTimeVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCursorXVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCursorTimeVisibleProperty =
            DependencyProperty.Register("IsCursorTimeVisible", typeof(bool), typeof(Plot), new PropertyMetadata(false));



        public bool IsCursorVoltageVisible
        {
            get { return (bool)GetValue(IsCursorVoltageVisibleProperty); }
            set { SetValue(IsCursorVoltageVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCursorYVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCursorVoltageVisibleProperty =
            DependencyProperty.Register("IsCursorVoltageVisible", typeof(bool), typeof(Plot), new PropertyMetadata(false));
        #endregion

        #region 数据范围
        ///// <summary>
        ///// X 轴数据范围
        ///// </summary>
        //public Range XDataRange
        //{
        //    get { return (Range)GetValue(XDataRangeProperty); }
        //    set { SetValue(XDataRangeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for XDataRange.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty XDataRangeProperty =
        //    DependencyProperty.Register("XDataRange", typeof(Range), typeof(Plot), new PropertyMetadata(new Range()));

        ///// <summary>
        ///// Y 轴数据范围
        ///// </summary>
        //public Range YDataRange
        //{
        //    get { return (Range)GetValue(YDataRangeProperty); }
        //    set { SetValue(YDataRangeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for YDataRange.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty YDataRangeProperty =
        //    DependencyProperty.Register("YDataRange", typeof(Range), typeof(Plot), new PropertyMetadata(new Range()));
        #endregion

        #region 绘图质量
        public DrawQualityEnum DrawQuality
        {
            get { return (DrawQualityEnum)GetValue(DrawQualityProperty); }
            set { SetValue(DrawQualityProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrawQualityProperty =
            DependencyProperty.Register("DrawQuality", typeof(DrawQualityEnum), typeof(Plot), new PropertyMetadata(DrawQualityEnum.HighQuality));


        #endregion

        #region 迹线集合
        public ObservableCollection<XyDataSeries> RenderDataSeries
        {
            get { return (ObservableCollection<XyDataSeries>)GetValue(RenderDataSeriesProperty); }
            set { SetValue(RenderDataSeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenderDataSeries.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderDataSeriesProperty =
            DependencyProperty.Register("RenderDataSeries", typeof(ObservableCollection<XyDataSeries>), typeof(Plot), new PropertyMetadata(new ObservableCollection<XyDataSeries>()));
        #endregion

        #region 标尺坐标
        public double SliceX0
        {
            get { return (double)GetValue(SliceX0Property); }
            set { SetValue(SliceX0Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceX0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceX0Property =
            DependencyProperty.Register("SliceX0", typeof(double), typeof(Plot), new PropertyMetadata(0.0));

        public double SliceX1
        {
            get { return (double)GetValue(SliceX1Property); }
            set { SetValue(SliceX1Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceX1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceX1Property =
            DependencyProperty.Register("SliceX1", typeof(double), typeof(Plot), new PropertyMetadata(0.0));

        public double SliceY0
        {
            get { return (double)GetValue(SliceY0Property); }
            set { SetValue(SliceY0Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceY0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceY0Property =
            DependencyProperty.Register("SliceY0", typeof(double), typeof(Plot), new PropertyMetadata(0.0));

        public double SliceY1
        {
            get { return (double)GetValue(SliceY1Property); }
            set { SetValue(SliceY1Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceY1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceY1Property =
            DependencyProperty.Register("SliceY1", typeof(double), typeof(Plot), new PropertyMetadata(0.0));
        #endregion



        public bool OnlyDisplaySelect
        {
            get { return (bool)GetValue(OnlyDisplaySelectProperty); }
            set { SetValue(OnlyDisplaySelectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnlyDisplaySelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnlyDisplaySelectProperty =
            DependencyProperty.Register("OnlyDisplaySelect", typeof(bool), typeof(Plot), new PropertyMetadata(false, new PropertyChangedCallback(OnlyDisplaySelectChanged)));

        private static void OnlyDisplaySelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Plot c = d as Plot;
            c.Draw();
        }





        #endregion

        #region drawImage
        public void DrawImage(byte[] colorTable, byte[] pxiTable)
        {
            isData = false;
            this.colorTable = colorTable;
            this.pxiTable = pxiTable;
            GDI.Bitmap b = new GDI.Bitmap(700, 200, GDI.Imaging.PixelFormat.Format32bppRgb);
            ImageDataRender imageDataRender = new ImageDataRender();
            imageDataRender.Create(700, 200, b, 4);
            imageDataRender.RefreshTable(3, colorTable);
            imageDataRender.Render(pxiTable);
            bitmap.Lock();
            graphics1.DrawImage(b, -1, 0, width + 1, height);
            b.Dispose();
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            bitmap.Unlock();
        }
        #endregion

        #region DrawLine
        /// <summary>
        /// 自动缩放
        /// </summary>
        internal void AutoScale()
        {
            foreach (var series in RenderDataSeries)
            {
                series.XVisibleRange = new Range(series.XOrignalVisibleRange.Min, series.XOrignalVisibleRange.Max);
                series.YVisibleRange = new Range(series.YOrignalVisibleRange.Min, series.YOrignalVisibleRange.Max);
                //DrawLine(series);
            }
            Draw();
            RenderFlag = new object();
        }

        
       //TOD

        /// <summary>
        /// 计算迹线数据对应的像素数据，数据改变/缩放/窗口大小改变时调用
        /// </summary>
        /// <param name="dataSeries"></param>
        private void CalculatePxiValue(XyDataSeries dataSeries)
        {
            double maxx = dataSeries.XVisibleRange.Max;
            double minx = dataSeries.XVisibleRange.Min;
            double maxy = dataSeries.YVisibleRange.Max;
            double miny = dataSeries.YVisibleRange.Min;
            double xlength = maxx - minx;
            double ylength = maxy - miny;
            if (xlength == 0)
            {
                return;

            }
            if (ylength == 0)
            {
                maxy = dataSeries.YRange.Max;
                miny = dataSeries.YRange.Min;
                ylength = maxy - miny;
            }
            int picWidth = width - 1;
            int picHeight = height - 1;
            int startIndex = findStartIndex(dataSeries, 0, dataSeries.Count - 1);
            int endIndex = findEndIndex(dataSeries, 0, dataSeries.Count - 1);
            
            dataSeries.Data2Pxi(picWidth, picHeight, startIndex, endIndex, minx, xlength, miny, ylength);
        }


        /// <summary>
        /// 将迹线的像素数据渲染到界面
        /// </summary>
        internal void Draw()
        {
            isData = true;
            this.colorTable = null;
            this.pxiTable = null;
            if (RenderDataSeries.Count != 0)
            {
                bitmap.Lock();
                graphics1.Clear(ColorConverterHelper.MediaColor2GDIColor(Background));
                foreach (var series in RenderDataSeries)
                {
                    if ((series.IsSelect || !OnlyDisplaySelect) && series.XValues != null)
                    {
                        //x轴数据超出界限
                        if (series.XValues[0] < series.XVisibleRange.Max && series.XValues[series.XValues.Count - 1] > series.XVisibleRange.Min)
                        {
                            //Stopwatch stopwatch = new Stopwatch();
                            //stopwatch.Start();
                            CalculatePxiValue(series);
                            //stopwatch.Stop();
                            //Console.WriteLine($"抽点耗时：{stopwatch.ElapsedMilliseconds}");
                            GDI.Pen pen = new GDI.Pen(ColorConverterHelper.MediaColor2GDIColor(series.Color), series.Thickness);
                            if (series.PointFs != null && series.PointFs.Count != 0)
                            {
                                //stopwatch.Restart();
                                graphics1.DrawLines(pen, series.PointFs.ToArray());
                                //stopwatch.Stop();
                                //Console.WriteLine($"画线耗时：{stopwatch.ElapsedMilliseconds}");
                            }
                        }
                        
                    }
                }
                //Stopwatch stopwatch1 = new Stopwatch();
                //stopwatch1.Start();
                bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
                //stopwatch1.Stop();
                //Console.WriteLine($"更新脏区耗时：{stopwatch1.ElapsedMilliseconds}");
                bitmap.Unlock();
            }

        }
        /// <summary>
        /// 清除所有迹线
        /// </summary>
        internal void ClearAll()
        {
            if (RenderDataSeries != null)
            {
                RenderDataSeries.Clear();
            }
            bitmap.Lock();
            graphics1.Clear(ColorConverterHelper.MediaColor2GDIColor(Background));
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            bitmap.Unlock();
        }

        /// <summary>
        /// 查找当前可视范围的起始点索引
        /// </summary>
        /// <param name="x"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        private int findStartIndex(XyDataSeries dataSeries, int startIndex, int endIndex)
        {
            var x = dataSeries.XValues;
            if (startIndex == 0 && x[0] >= dataSeries.XVisibleRange.Min)
            {
                return 0;
            }

            if (x[startIndex] >= dataSeries.XVisibleRange.Min && x[startIndex - 1] <= dataSeries.XVisibleRange.Min)
            {
                return startIndex;
            }
            int index = (endIndex - startIndex) / 2 + startIndex;
            if (endIndex - startIndex == 1)
            {
                startIndex = endIndex;
            }
            else if (x[index] < dataSeries.XVisibleRange.Min)
            {
                startIndex = index;
            }
            else
            {
                endIndex = index;
            }
            return findStartIndex(dataSeries, startIndex, endIndex);
        }

        /// <summary>
        /// 查找当前可视范围的终止点索引
        /// </summary>
        /// <param name="x"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        private int findEndIndex(XyDataSeries dataSeries, int startIndex, int endIndex)
        {
            var x = dataSeries.XValues;
            if (endIndex == x.Count - 1 && x[endIndex] <= dataSeries.XVisibleRange.Max)
            {
                return endIndex;
            }

            if (x[endIndex] <= dataSeries.XVisibleRange.Max && x[endIndex + 1] >= dataSeries.XVisibleRange.Max)
            {
                return endIndex;
            }
            int index = (endIndex - startIndex) / 2 + startIndex;

            if (endIndex - startIndex == 1)
            {
                endIndex = startIndex;
            }
            else if (x[index] > dataSeries.XVisibleRange.Max)
            {
                endIndex = index;
            }
            else
            {
                startIndex = index;
            }
            return findEndIndex(dataSeries, startIndex, endIndex);

        }
        #endregion

        Media.DrawingContext dc;
        /// <summary>
        /// 初始化图表
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(Media.DrawingContext dc)
        {           
            this.dc = dc;
            double div = 1;
            width = (int)(RenderSize.Width / div);
            height = (int)(RenderSize.Height / div);
            if (width < 100)
            {
                width = 100;
            }
            if (height < 100)
            {
                height = 100;
            }
            bitmap = new WriteableBitmap(width, height, 96, 96, Media.PixelFormats.Bgr32, null);
            using (GDI.Bitmap backBufferBitmap = new GDI.Bitmap(width, height, bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppRgb, bitmap.BackBuffer))
            {
                graphics1 = GDI.Graphics.FromImage(backBufferBitmap);
                if (DrawQuality == DrawQualityEnum.HighQuality)
                {
                    graphics1.SmoothingMode = GDI.Drawing2D.SmoothingMode.HighQuality;
                    graphics1.CompositingQuality = GDI.Drawing2D.CompositingQuality.HighQuality;
                }
                else
                {
                    graphics1.SmoothingMode = GDI.Drawing2D.SmoothingMode.HighSpeed;
                    graphics1.CompositingQuality = GDI.Drawing2D.CompositingQuality.HighSpeed;
                }
            }
            graphics1.CompositingMode = GDI.Drawing2D.CompositingMode.SourceOver;
            bitmap.Lock();
            graphics1.Clear(ColorConverterHelper.MediaColor2GDIColor(Background));
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            bitmap.Unlock();


            //lineBitmap = new GDI.Bitmap(width, height, GDI.Imaging.PixelFormat.Format32bppRgb);
            if (colorTable != null)
            {
                DrawImage(colorTable, pxiTable);
            }
            else if (RenderDataSeries.Count != 0)
            {
                RenderFlag = new object();
            }

            dc.DrawImage(bitmap, new Rect(0, 0, (int)RenderSize.Width, (int)RenderSize.Height));
            base.OnRender(dc);
        }
    }
}
