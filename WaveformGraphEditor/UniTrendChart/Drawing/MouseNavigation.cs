using System;
using System.Collections.Generic;
using GDI = System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using UniTrendChart.Data;
using System.Collections.ObjectModel;

namespace UniTrendChart.Drawing
{
    public class MouseNavigation : Grid
    {
        
        private Path pathx;
        private Path pathy;
        private GeometryGroup groupx;
        private GeometryGroup groupy;
        private LineGeometry lineX0;
        private LineGeometry lineX1;
        private LineGeometry lineY0;
        private LineGeometry lineY1;
        private TextBox x0Lable;
        private TextBox x1Lable;
        private TextBox y0Lable;
        private TextBox y1Lable;
        //private PlotBase masterPlot = null;

        private LineGeometry curLine;
        private bool isSliceDrag = false;
        private bool isZoom = false;
        private bool cursorSlice = false;//鼠标是否在slice上
        private int cursorOffset = 5;
        private Point startPoint;
        private Point prePoint;
        private Rectangle rect;
        private Canvas canvas = new Canvas
        {
            Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255))
        };

        #region 显示范围

        ///// <summary>
        ///// X 轴数据范围
        ///// </summary>
        //public Range XRange
        //{
        //    get { return (Range)GetValue(XRangeProperty); }
        //    set { SetValue(XRangeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for XRange.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty XRangeProperty =
        //    DependencyProperty.Register("XRange", typeof(Range), typeof(MouseNavigation), new PropertyMetadata(new Range(), new PropertyChangedCallback(XRangePropertyChanged)));

        //private static void XRangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var c = d as MouseNavigation;
        //    var picWidth = c.ActualWidth - 1;
        //    var xlength = c.XRange.Max - c.XRange.Min;
        //    var xmin = c.XRange.Min;
        //    c.SliceX0 = c.lineX0.StartPoint.X / picWidth * xlength + xmin;
        //    c.SliceX1 = c.lineX1.StartPoint.X / picWidth * xlength + xmin;
        //}

        ///// <summary>
        ///// Y 轴数据范围
        ///// </summary>
        //public Range YRange
        //{
        //    get { return (Range)GetValue(YRangeProperty); }
        //    set { SetValue(YRangeProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for YRange.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty YRangeProperty =
        //    DependencyProperty.Register("YRange", typeof(Range), typeof(MouseNavigation), new PropertyMetadata(new Range(),new PropertyChangedCallback(YRangePropertyChanged)));

        //private static void YRangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var c = d as MouseNavigation;
        //    var picHeight = c.ActualHeight - 1;
        //    var ylength = c.YRange.Max - c.YRange.Min;
        //    var ymin = c.YRange.Min;
        //    c.SliceY0 = (1 - c.lineY0.StartPoint.Y / picHeight) * ylength + ymin;
        //    c.SliceY1 = (1 - c.lineY1.StartPoint.Y / picHeight) * ylength + ymin;
        //}
        #endregion

        #region 绘图flag
        public object RenderFlag
        {
            get { return (object)GetValue(RenderFlagProperty); }
            set { SetValue(RenderFlagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenderFlag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderFlagProperty =
            DependencyProperty.Register("RenderFlag", typeof(object), typeof(MouseNavigation), new PropertyMetadata(new object()));
        #endregion

        #region x轴缩放平移
        public bool IsZoomX
        {
            get { return (bool)GetValue(IsZoomXProperty); }
            set { SetValue(IsZoomXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsZoomX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsZoomXProperty =
            DependencyProperty.Register("IsZoomX", typeof(bool), typeof(MouseNavigation), new PropertyMetadata(true));
        #endregion


        #region y轴缩放平移
        public bool IsZoomY
        {
            get { return (bool)GetValue(IsZoomYProperty); }
            set { SetValue(IsZoomYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsZoomY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsZoomYProperty =
            DependencyProperty.Register("IsZoomY", typeof(bool), typeof(MouseNavigation), new PropertyMetadata(true));
        #endregion

        #region 标尺坐标


        public double SliceX0
        {
            get { return (double)GetValue(SliceX0Property); }
            set { SetValue(SliceX0Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceX0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceX0Property =
            DependencyProperty.Register("SliceX0", typeof(double), typeof(MouseNavigation), new PropertyMetadata(0.0));

        public double SliceX1
        {
            get { return (double)GetValue(SliceX1Property); }
            set { SetValue(SliceX1Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceX1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceX1Property =
            DependencyProperty.Register("SliceX1", typeof(double), typeof(MouseNavigation), new PropertyMetadata(0.0));

        public double SliceY0
        {
            get { return (double)GetValue(SliceY0Property); }
            set { SetValue(SliceY0Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceY0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceY0Property =
            DependencyProperty.Register("SliceY0", typeof(double), typeof(MouseNavigation), new PropertyMetadata(0.0));

        public double SliceY1
        {
            get { return (double)GetValue(SliceY1Property); }
            set { SetValue(SliceY1Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceY1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceY1Property =
            DependencyProperty.Register("SliceY1", typeof(double), typeof(MouseNavigation), new PropertyMetadata(0.0));
        #endregion

        #region 迹线集合
        public ObservableCollection<XyDataSeries> RenderDataSeries
        {
            get { return (ObservableCollection<XyDataSeries>)GetValue(RenderDataSeriesProperty); }
            set { SetValue(RenderDataSeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenderDataSeries.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderDataSeriesProperty =
            DependencyProperty.Register("RenderDataSeries", typeof(ObservableCollection<XyDataSeries>), typeof(MouseNavigation), new PropertyMetadata(new ObservableCollection<XyDataSeries>()));
        #endregion

        #region 是否显示标尺
        public bool IsCursorXVisible
        {
            get { return (bool)GetValue(IsCursorXVisibleProperty); }
            set { SetValue(IsCursorXVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCursorXVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCursorXVisibleProperty =
            DependencyProperty.Register("IsCursorXVisible", typeof(bool), typeof(MouseNavigation), new PropertyMetadata(true, new PropertyChangedCallback(IsCursorXVisibleChanged)));

        private static void IsCursorXVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MouseNavigation;
            
            if (c.IsCursorXVisible)
            {
                c.pathx.Visibility = Visibility.Visible;
                c.pathx.IsHitTestVisible = true;
                c.x0Lable.Visibility = Visibility.Visible;
                c.x1Lable.Visibility = Visibility.Visible;
                c.CalculateCursorX();
            }
            else
            {
                c.pathx.Visibility = Visibility.Collapsed;
                c.pathx.IsHitTestVisible = false;
                c.x0Lable.Visibility = Visibility.Collapsed;
                c.x1Lable.Visibility = Visibility.Collapsed;
            }
        }

        public bool IsCursorYVisible
        {
            get { return (bool)GetValue(IsCursorYVisibleProperty); }
            set { SetValue(IsCursorYVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCursorYVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCursorYVisibleProperty =
            DependencyProperty.Register("IsCursorYVisible", typeof(bool), typeof(MouseNavigation), new PropertyMetadata(true, new PropertyChangedCallback(IsCursorYVisibleChanged)));

        private static void IsCursorYVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MouseNavigation;
            
            if (c.IsCursorYVisible)
            {
                c.pathy.Visibility = Visibility.Visible;
                c.pathy.IsHitTestVisible = true;
                c.y0Lable.Visibility = Visibility.Visible;
                c.y1Lable.Visibility = Visibility.Visible;
                c.CalculateCursorY();
            }
            else
            {
                c.pathy.Visibility = Visibility.Collapsed;
                c.pathy.IsHitTestVisible = false ;
                c.y0Lable.Visibility = Visibility.Collapsed;
                c.y1Lable.Visibility = Visibility.Collapsed;
            }
        }


        #endregion


        #region  是否可以平移缩放
        public bool ZoomEnable
        {
            get { return (bool)GetValue(ZoomEnableProperty); }
            set { SetValue(ZoomEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoomEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomEnableProperty =
            DependencyProperty.Register("ZoomEnable", typeof(bool), typeof(MouseNavigation), new PropertyMetadata(true));
        #endregion

        public MouseNavigation()
        {
            Children.Add(canvas);
            pathx = new Path();
            pathx.Stroke = Brushes.Red;
            pathx.StrokeThickness = 1.0;
            pathy = new Path();
            pathy.Stroke = Brushes.Red;
            pathy.StrokeThickness = 1.0;

            rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Color.FromArgb(255, 66, 182, 73));
            rect.StrokeThickness = 2;
            rect.Stroke = new SolidColorBrush(Color.FromArgb(255, 66, 182, 73));
            rect.Opacity = 0.6;

            lineX0 = new LineGeometry();
            lineX1 = new LineGeometry();
            lineY0 = new LineGeometry();
            lineY1 = new LineGeometry();
            x0Lable = initLable("X0", new Thickness(-15, 0, 0, 0));
            x1Lable = initLable("X1", new Thickness(-15, 0, 0, 0));
            y0Lable = initLable("Y0", new Thickness(-15, 0, 0, 0));
            y1Lable = initLable("Y1", new Thickness(-15, 0, 0, 0));

            Loaded += MouseNavigation_Loaded;
            Unloaded += MouseNavigation_Unloaded;
            if (IsCursorXVisible)
            {
                pathx.Visibility = Visibility.Visible;
                pathx.IsHitTestVisible = true;
                x0Lable.Visibility = Visibility.Visible;
                x1Lable.Visibility = Visibility.Visible;
            }
            else
            {
                pathx.Visibility = Visibility.Collapsed;
                pathx.IsHitTestVisible = false;
                x0Lable.Visibility = Visibility.Collapsed;
                x1Lable.Visibility = Visibility.Collapsed;
            }
            if (IsCursorYVisible)
            {
                pathy.Visibility = Visibility.Visible;
                pathy.IsHitTestVisible = true;
                y0Lable.Visibility = Visibility.Visible;
                y1Lable.Visibility = Visibility.Visible;
            }
            else
            {
                pathy.Visibility = Visibility.Collapsed;
                pathy.IsHitTestVisible = false;
                y0Lable.Visibility = Visibility.Collapsed;
                y1Lable.Visibility = Visibility.Collapsed;
            }
        }

        private void MouseNavigation_Unloaded(object sender, RoutedEventArgs e)
        {
            MouseLeftButtonDown -= MouseNavigation_MouseLeftButtonDown;
            MouseLeftButtonUp -= MouseNavigation_MouseLeftButtonUp;
            MouseWheel -= MouseNavigation_MouseWheel;
            MouseMove -= MouseNavigation_MouseMove;
        }

        private void MouseNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is EditorMode mode)
            {
                if(true)
                {
                    //masterPlot = PlotBase.FindMaster(this);
                    MouseWheel += MouseNavigation_MouseWheel;
                    MouseMove += MouseNavigation_MouseMove;
                    this.PreviewMouseMove += MouseNavigation_PreviewMouseMove;
                }
                else
                {
                    MouseLeftButtonUp += new MouseButtonEventHandler(MouseNavigation_MouseLeftButtonUp);
                    MouseLeftButtonDown += new MouseButtonEventHandler(MouseNavigation_MouseLeftButtonDown);

                    LostMouseCapture += MouseNavigation_LostMouseCapture;
                }
               
            }
           

        }

        private void MouseNavigation_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(this.DataContext is EditorMode mode)
            {
                mode.NowPoint = e.GetPosition(this);
            }
        }

        private void MouseNavigation_LostMouseCapture(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            isSliceDrag = false;
            isZoom = false;
            if (canvas.Children.Contains(rect))
            {
                canvas.Children.Remove(rect);
            }
        }


        private void MouseNavigation_MouseMove(object sender, MouseEventArgs e)
        {
            Point cursorPosition = e.GetPosition(this);
            if (cursorPosition.X == this.ActualWidth)
            {
                cursorPosition.X = this.ActualWidth - 1;
            }
            if (cursorPosition.Y == this.ActualHeight)
            {
                cursorPosition.Y = this.ActualHeight - 1;
            }
            if (!CheckCursor(cursorPosition))
            {
                return;
            }
            //判断鼠标是否在slice区域 
            if (!isSliceDrag && !isZoom)
            {
                cursorSlice = false;
                if (IsCursorYVisible)
                {
                    foreach (var line in (pathy.Data as GeometryGroup).Children)
                    {
                        var lineGeometry = line as LineGeometry;
                        if (lineGeometry.StartPoint.Y == lineGeometry.EndPoint.Y
                           && (lineGeometry.EndPoint.Y >= cursorPosition.Y - cursorOffset
                           && lineGeometry.EndPoint.Y <= cursorPosition.Y + cursorOffset))
                        {
                            cursorSlice = true;
                            Cursor = Cursors.SizeNS;
                            curLine = lineGeometry;
                        }
                    }
                }
                if (IsCursorXVisible)
                {
                    foreach (var line in (pathx.Data as GeometryGroup).Children)
                    {
                        var lineGeometry = line as LineGeometry;
                        if (lineGeometry.StartPoint.X == lineGeometry.EndPoint.X
                            && (lineGeometry.EndPoint.X >= cursorPosition.X - cursorOffset
                            && lineGeometry.EndPoint.X <= cursorPosition.X + cursorOffset))
                        {
                            cursorSlice = true;
                            Cursor = Cursors.SizeWE;
                            curLine = lineGeometry;
                        }
                    }
                }
                if (!cursorSlice)
                {
                    Cursor = Cursors.Arrow;
                    curLine = null;
                }
            }
            if (IsCursorXVisible && isSliceDrag)
            {
                if (curLine.StartPoint.X == curLine.EndPoint.X) //垂直
                {
                    var s = getSelectSeries();
                    if (s == null) return;
                    var XRange = s.XVisibleRange;
                    if (XRange == null) return;
                    var picWidth = ActualWidth - 1;
                    //var picWidth = ActualWidth;
                    var xlength = XRange.Length;
                    var xmin = XRange.Min;
                    var x = cursorPosition.X / picWidth * xlength + xmin;
                    if (s.XValues != null)
                    {
                        var sataSeries = s;
                        int minIndex = 0;
                        int maxIndex = sataSeries.Count - 1;
                        while (minIndex < maxIndex)
                        {
                            int mid = (maxIndex + minIndex) / 2;
                            if (sataSeries.XValues[mid] < x)
                            {
                                minIndex = mid + 1;

                            }
                            else
                            {
                                maxIndex = mid;
                            }
                        }
                        if (maxIndex > 0)
                        {
                            maxIndex -= 1;
                        }
                        if (x >= sataSeries.XValues[sataSeries.Count - 1])
                        {
                            maxIndex = sataSeries.Count - 1;
                        }
                        float px = (float)((sataSeries.XValues[maxIndex] - xmin) / xlength * picWidth);
                        if (px < 0)
                        {
                            return;
                        }
                        curLine.StartPoint = new Point(px, curLine.StartPoint.Y);
                        curLine.EndPoint = new Point(px, curLine.EndPoint.Y);
                        if (curLine == lineX0)
                        {
                            x0Lable.Margin = new Thickness(px - 10, this.ActualHeight, 0, 0);
                            SliceX0 = sataSeries.XValues[maxIndex];
                        }
                        else
                        {
                            x1Lable.Margin = new Thickness(px - 10, this.ActualHeight, 0, 0);
                            SliceX1 = sataSeries.XValues[maxIndex];
                        }
                    }
                    else
                    {
                        curLine.StartPoint = new Point(cursorPosition.X, curLine.StartPoint.Y);
                        curLine.EndPoint = new Point(cursorPosition.X, curLine.EndPoint.Y);
                        if (curLine == lineX0)
                        {
                            x0Lable.Margin = new Thickness(cursorPosition.X - 10, this.ActualHeight, 0, 0);
                            SliceX0 = x;
                        }
                        else
                        {
                            x1Lable.Margin = new Thickness(cursorPosition.X - 10, this.ActualHeight, 0, 0);
                            SliceX1 = x;
                        }
                    }
                }
                
            }
            if (IsCursorYVisible && isSliceDrag)
            {
                var s = getSelectSeries();
                if (s == null) return;
                var YRange = s.YVisibleRange;
                if (YRange == null) return;
                var picHeight = ActualHeight - 1;
                var ylength = YRange.Max - YRange.Min;
                var ymin = YRange.Min;
                if (curLine.StartPoint.Y == curLine.EndPoint.Y)  //水平
                {
                    curLine.StartPoint = new Point(curLine.StartPoint.X, cursorPosition.Y);
                    curLine.EndPoint = new Point(curLine.EndPoint.X, cursorPosition.Y);
                    var y = (1 - cursorPosition.Y / picHeight) * ylength + ymin;
                    if (curLine == lineY0)
                    {
                        y0Lable.Margin = new Thickness(-20, cursorPosition.Y - 8, 0, 0);
                        SliceY0 = y;

                    }
                    else
                    {
                        y1Lable.Margin = new Thickness(-20, cursorPosition.Y - 8, 0, 0);
                        SliceY1 = y;
                    }
                }
            }
            if (isZoom && rect != null)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    var series = getSelectSeries();
                    var XRange = series?.XVisibleRange;
                    var YRange = series?.YVisibleRange;
                    double width = cursorPosition.X - prePoint.X;
                    double height = cursorPosition.Y - prePoint.Y;
                    double axisWidth = (float)(width / this.ActualWidth * (XRange.Max - XRange.Min));
                    double axisHeight = (float)(height / this.ActualHeight * (YRange.Max - YRange.Min));
                    if (IsZoomX)
                    {
                        series.XVisibleRange = new Range(XRange.Min - axisWidth, XRange.Max - axisWidth);
                    }
                    if (IsZoomY)
                    {
                        series.YVisibleRange = new Range(YRange.Min + axisHeight, YRange.Max + axisHeight);
                    }
                    RenderFlag = new object();
                    CalculateCursorX();
                    CalculateCursorY();
                    prePoint = new Point(cursorPosition.X, cursorPosition.Y);
                }
                else
                {
                    rect.SetValue(Canvas.LeftProperty, cursorPosition.X < startPoint.X ? cursorPosition.X : startPoint.X);
                    rect.SetValue(Canvas.TopProperty, cursorPosition.Y < startPoint.Y ? cursorPosition.Y : startPoint.Y);

                    rect.Width = Math.Abs(cursorPosition.X - startPoint.X);
                    rect.Height = Math.Abs(cursorPosition.Y - startPoint.Y);
                }
                
            }
        }

        private void MouseNavigation_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {          
            if (canvas.Children.Contains(rect))
            {
                canvas.Children.Remove(rect);
            }
            if (isZoom)
            {
                if (rect.Width > 5 && rect.Height > 5)
                {
                    var series = getSelectSeries();
                    var XRange = series?.XVisibleRange;
                    var YRange = series?.YVisibleRange;
                    double startX = (double)rect.GetValue(Canvas.LeftProperty);
                    double startY = (double)rect.GetValue(Canvas.TopProperty);
                    double width = rect.Width;
                    double height = rect.Height;
                    double xmin = startX / (this.ActualWidth - 1) * (XRange.Max - XRange.Min) + XRange.Min;
                    double ymin = (1 - (startY + height) / (this.ActualHeight - 1)) * (YRange.Max - YRange.Min) + YRange.Min;
                    double axisWidth = width / (this.ActualWidth - 1) * (XRange.Max - XRange.Min);
                    double axisHeight = height / (this.ActualHeight - 1) * (YRange.Max - YRange.Min);
                    if (IsZoomX)
                    {
                        series.XVisibleRange = new Range((float)xmin, (float)(xmin + axisWidth));
                    }
                    if (IsZoomY)
                    {
                        series.YVisibleRange = new Range((float)ymin, (float)(ymin + axisHeight));
                    }

                    RenderFlag = new object();
                    CalculateCursorX();
                    CalculateCursorY();
                }
            }
            isSliceDrag = false;
            isZoom = false;
            this.ReleaseMouseCapture();
        }

        private void MouseNavigation_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Point cursorPosition = e.GetPosition(this);
            this.CaptureMouse();
            if (cursorSlice)
            {
                isSliceDrag = true;
            }
            else if(ZoomEnable)
            {
                isZoom = true;
                startPoint = cursorPosition;
                prePoint = new Point(cursorPosition.X, cursorPosition.Y);
                rect.Width = 0;
                rect.Height = 0;
                if (rect.Parent != null) rect.Parent.SetValue(ContentPresenter.ContentProperty, null);
                canvas.Children.Add(rect);
            }
            e.Handled = true;
        }

        private void MouseNavigation_MouseWheel(object sender, MouseEventArgs e)
        {
            Point cursorPosition = e.GetPosition(this);
            if (cursorPosition.X == this.ActualWidth)
            {
                cursorPosition.X = this.ActualWidth - 1;
            }
            if (cursorPosition.Y == this.ActualHeight)
            {
                cursorPosition.Y = this.ActualHeight - 1;
            }
            if (!CheckCursor(cursorPosition))
            {
                return;
            }
            if (ZoomEnable)
            {
                var series = getSelectSeries();
                var XRange = series?.XVisibleRange;
                var YRange = series?.YVisibleRange;
                if(XRange == null || YRange == null)
                {
                    return;
                }
                //double width = cursorPosition.X - prePoint.X;
                //double height = cursorPosition.Y - prePoint.Y;
                double axisWidth = cursorPosition.X / this.ActualWidth * XRange.Length * 0.05;
                double axisWidth1 = (1- cursorPosition.X / this.ActualWidth) * XRange.Length * 0.05;

                double axisHeight = cursorPosition.Y / this.ActualHeight * YRange.Length * 0.05;
                double axisHeight1 = (1 - cursorPosition.Y / this.ActualHeight) * YRange.Length * 0.05;
                if (IsZoomX)
                {
                    if (((MouseWheelEventArgs)e).Delta > 0)
                    {
                        series.XVisibleRange = new Range(XRange.Min + axisWidth, XRange.Max - axisWidth1);
                    }
                    else
                    {
                        series.XVisibleRange = new Range(XRange.Min - axisWidth, XRange.Max + axisWidth1);
                    }
                }
                if (IsZoomY)
                {
                    if (((MouseWheelEventArgs)e).Delta > 0)
                    {
                        series.YVisibleRange = new Range(YRange.Min + axisHeight1, YRange.Max - axisHeight);
                        
                    }
                    else
                    {
                        series.YVisibleRange = new Range(YRange.Min - axisHeight1, YRange.Max + axisHeight);
                    }
                }
                RenderFlag = new object();
                CalculateCursorX();
                CalculateCursorY();
            }
            //Console.WriteLine($"{cursorPosition.X},{cursorPosition.Y}");
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Double.IsInfinity(availableSize.Width))
                availableSize.Width = 1024;
            if (Double.IsInfinity(availableSize.Height))
                availableSize.Height = 1024;
            double maxX = availableSize.Width;
            double maxY = availableSize.Height;
            if (groupx == null || groupy == null)
            {
                groupx = new GeometryGroup();
                groupy = new GeometryGroup();
                lineX0.StartPoint = new Point(0, maxY);
                lineX0.EndPoint = new Point(0, 0);
                lineX1.StartPoint = new Point(0, maxY);
                lineX1.EndPoint = new Point(0, 0);

                lineY0.StartPoint = new Point(0, 0);
                lineY0.EndPoint = new Point(maxX, 0);
                lineY1.StartPoint = new Point(0, 0);
                lineY1.EndPoint = new Point(maxX, 0);
          
                groupx.Children.Add(lineX0);
                groupx.Children.Add(lineX1);
                groupy.Children.Add(lineY0);
                groupy.Children.Add(lineY1);
                pathx.Data = groupx;
                pathy.Data = groupy;
                canvas.Children.Add(pathx);
                canvas.Children.Add(pathy);
                canvas.Children.Add(x0Lable);
                canvas.Children.Add(x1Lable);
                canvas.Children.Add(y0Lable);
                canvas.Children.Add(y1Lable);
            }
            else
            {
                lineX0.StartPoint = new Point(0, maxY);
                lineX0.EndPoint = new Point(0, 0);
                lineX1.StartPoint = new Point(0, maxY);
                lineX1.EndPoint = new Point(0, 0);
                x0Lable.Margin = new Thickness(-10, maxY, 0, 0);
                x1Lable.Margin = new Thickness(-10, maxY, 0, 0);
                y0Lable.Margin = new Thickness(-20, -8, 0, 0);
                y1Lable.Margin = new Thickness(-20, -8, 0, 0);

                lineY0.StartPoint = new Point(0, 0);
                lineY0.EndPoint = new Point(maxX, 0);
                lineY1.StartPoint = new Point(0, 0);
                lineY1.EndPoint = new Point(maxX, 0);
            }

            canvas.Measure(availableSize);
            return canvas.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            canvas.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }

        private bool CheckCursor(Point cursorPosition)
        {
            return !(cursorPosition.X < 0 || cursorPosition.Y < 0 || cursorPosition.X > this.ActualWidth || cursorPosition.Y > this.ActualHeight);
        }

        private TextBox initLable(string text, Thickness thickness)
        {
            TextBox lable = new TextBox();
            lable.Margin = thickness;
            lable.Text = text;
            lable.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#911213"));
            lable.IsReadOnly = true;
            lable.IsHitTestVisible = false;
            lable.BorderThickness = new Thickness(0);
            lable.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c1c1c1"));
            lable.Width = 20;       
            lable.TextAlignment = TextAlignment.Center;
            return lable;
        }

        private XyDataSeries getSelectSeries()
        {
            foreach (var item in RenderDataSeries)
            {
                if (item.IsSelect)
                {
                    return item;
                }
            }
            return null;
        }
        public string CalculateCursorY()
        {
            var s = getSelectSeries();
            if (s == null) return "";
            if (!IsCursorYVisible) return s.YUnit;
            
            var yrange = s.YVisibleRange;
            if (yrange == null) return s.YUnit;
            var picHeight = ActualHeight - 1;
            var ylength = yrange.Max - yrange.Min;
            var ymin = yrange.Min;
            SliceY0 = (1 - lineY0.StartPoint.Y / picHeight) * ylength + ymin;
            SliceY1 = (1 - lineY1.StartPoint.Y / picHeight) * ylength + ymin;
            return s.YUnit;
        }
        public string CalculateCursorX()
        {
            var s = getSelectSeries();
            if (s == null) return "";

            if (!IsCursorXVisible) return s.XUnit;
            
            var xrange = s.XVisibleRange;
            if (xrange == null) return s.XUnit;
            var picWidth = ActualWidth - 1;
            var xlength = xrange.Max - xrange.Min;
            var xmin = xrange.Min;
            SliceX0 = lineX0.StartPoint.X / picWidth * xlength + xmin;
            SliceX1 = lineX1.StartPoint.X / picWidth * xlength + xmin;
            return s.XUnit;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            var maxX = this.ActualWidth;
            var maxY = this.ActualHeight;
            var s = getSelectSeries();
            if (s == null) return ;
            var XRange = s.XVisibleRange;
            if (XRange == null) return;
            var picWidth = ActualWidth - 1;
            var xlength = XRange.Length;
            var xmin = XRange.Min;

            var x0 = (SliceX0 - xmin) / xlength * picWidth;
            var x1 = (SliceX1 - xmin) / xlength * picWidth;
            lineX0.StartPoint = new Point(x0, maxY);
            lineX0.EndPoint = new Point(x0, 0);
            lineX1.StartPoint = new Point(x1, maxY);
            lineX1.EndPoint = new Point(x1, 0);
            x0Lable.Margin = new Thickness(-10 + x0, maxY, 0, 0);
            x1Lable.Margin = new Thickness(-10 + x1, maxY, 0, 0);

            var YRange = s.YVisibleRange;
            if (YRange == null) return;
            var picHeight = ActualHeight - 1;
            var ylength = YRange.Max - YRange.Min;
            var ymin = YRange.Min;

            //var y = (1 - cursorPosition.Y / picHeight) * ylength + ymin;
            var y0 = (1 - (SliceY0 - ymin) / ylength) * picHeight;
            var y1 = (1 - (SliceY1 - ymin) / ylength) * picHeight;
            lineY0.StartPoint = new Point(0, y0);
            lineY0.EndPoint = new Point(maxX, y0);
            lineY1.StartPoint = new Point(0, y1);
            lineY1.EndPoint = new Point(maxX, y1);
            y0Lable.Margin = new Thickness(-20, -8 + y0, 0, 0);
            y1Lable.Margin = new Thickness(-20, -8 + y1, 0, 0);
        }
    }
}
