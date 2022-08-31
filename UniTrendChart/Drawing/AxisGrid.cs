using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using UniTrendChart.Data;
using Media = System.Windows.Media;
using GDI = System.Drawing;
using UniTrendChart.Helper;

namespace UniTrendChart.Drawing
{
    public class AxisGrid : Grid
    {
        private Canvas canvas;
        private Path path; // 网格线
        private Path centerPath; //中心线
        private Path BorderPath; //边框线
        private Path TickPath; //刻度
        private Path MinorTickPath; //小刻度
        private const double tickLength = 10; // 刻度长度
        private const double minorTickLength = 5;//小刻度长度
        private const int minorGridCount = 5; // 每格分割格数

        public AxisGrid()
        {
            
            canvas = new Canvas();
            path = new Path();
            centerPath = new Path();
            BorderPath = new Path();
            TickPath = new Path();
            MinorTickPath = new Path();

            path.Stroke = Media.Brushes.DarkGray;//new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ededed"));
            path.StrokeThickness = 1.0;
            path.StrokeDashArray = new DoubleCollection() { 2, 2, 0, 2 };
            path.Opacity = 0.5;

            BorderPath.Stroke = Media.Brushes.DarkGray;
            BorderPath.StrokeThickness = 1.0;

            centerPath.Stroke = Media.Brushes.DarkGray;
            centerPath.StrokeThickness = 1.0;
            centerPath.Opacity = 0.5;

            TickPath.Stroke = Media.Brushes.DarkGray;
            TickPath.StrokeThickness = 1.0;
            TickPath.Opacity = 1;

            MinorTickPath.Stroke = Media.Brushes.DarkGray;
            MinorTickPath.StrokeThickness = 1.0;
            MinorTickPath.Opacity = 1;

            this.IsHitTestVisible = false;

            canvas.Children.Add(path);
            canvas.Children.Add(centerPath);
            canvas.Children.Add(BorderPath);
            canvas.Children.Add(TickPath);
            canvas.Children.Add(MinorTickPath);
            Children.Add(canvas);
        }

        #region 网格数
        public int XGridCount
        {
            get { return (int)GetValue(XGridCountProperty); }
            set { SetValue(XGridCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XGridCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XGridCountProperty =
            DependencyProperty.Register("XGridCount", typeof(int), typeof(AxisGrid), new PropertyMetadata(10, new PropertyChangedCallback(XGridCountChanged)));

        private static void XGridCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisGrid axisGrid = d as AxisGrid;
            axisGrid.RenderGrid(axisGrid.ActualHeight, axisGrid.ActualWidth, axisGrid.XGridCount, axisGrid.YGridCount);
        }

        public int YGridCount
        {
            get { return (int)GetValue(YGridCountProperty); }
            set { SetValue(YGridCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YGridCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YGridCountProperty =
            DependencyProperty.Register("YGridCount", typeof(int), typeof(AxisGrid), new PropertyMetadata(10, new PropertyChangedCallback(YGridCountChanged)));

        private static void YGridCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisGrid axisGrid = d as AxisGrid;
            axisGrid.RenderGrid(axisGrid.ActualHeight, axisGrid.ActualWidth, axisGrid.XGridCount, axisGrid.YGridCount);
        }
        #endregion

        protected override void OnRender(Media.DrawingContext dc)
        {
            RenderGrid(ActualHeight, ActualWidth, XGridCount, YGridCount);
        }

        #region private
        private void RenderGrid(double height, double width, int xGridCount, int yGridCount)
        {
            if (height != 0 && width != 0)
            {
                GeometryGroup group = new GeometryGroup();
                GeometryGroup centerGroup = new GeometryGroup();
                GeometryGroup borderGroup = new GeometryGroup();
                GeometryGroup tickGroup = new GeometryGroup();
                GeometryGroup minorTicGroup = new GeometryGroup();
                //double[] xTicks = new double[xGridCount + 1];
                //double[] yTicks = new double[yGridCount + 1];

                for (int i = 0; i < xGridCount * minorGridCount; i++)
                {
                    if (i % minorGridCount != 0)
                    {
                        LineGeometry minorTickLine = new LineGeometry();
                        //var m = ((xRange.Max - xRange.Min) / (xGridCount * minorGridCount)) * i;
                        //var t =  CoordinateFromTickHelper.GetCoordinateFromTick(m, xRange, width);
                        var t = i * width / (xGridCount * minorGridCount);
                        minorTickLine.StartPoint = new Point(t, height - minorTickLength);
                        minorTickLine.EndPoint = new Point(t, height);
                        minorTicGroup.Children.Add(minorTickLine);
                    }              
                }
                for (int i = 0; i < yGridCount * minorGridCount; i++)
                {
                    if (i % minorGridCount != 0)
                    {
                        LineGeometry minorTickLine = new LineGeometry();
                        //var m = (yRange.Max - yRange.Min) / (yGridCount * minorGridCount) * i;
                        //var t = CoordinateFromTickHelper.GetCoordinateFromTick(m, yRange, height);
                        var t = i * height / (yGridCount * minorGridCount);
                        minorTickLine.StartPoint = new Point(0, t);
                        minorTickLine.EndPoint = new Point(minorTickLength, t);
                        minorTicGroup.Children.Add(minorTickLine);
                    }
                }
                for (int i = 0; i <= xGridCount; i++)
                {
                    LineGeometry line = new LineGeometry();
                    LineGeometry tickLine = new LineGeometry();

                    //xTicks[i] = (xRange.Max - xRange.Min) / xGridCount * i;
                    //var xTick = CoordinateFromTickHelper.GetCoordinateFromTick(xTicks[i], xRange, width);
                    var xTick = i * width / xGridCount;
                    line.StartPoint = new Point(xTick, 0);
                    line.EndPoint = new Point(xTick, height);
                    if (i == 0 || i == xGridCount)
                    {
                        borderGroup.Children.Add(line);
                    }
                    else if (i == xGridCount / 2)
                    {
                        centerGroup.Children.Add(line);
                    }
                    else
                    {
                        group.Children.Add(line);
                        tickLine.StartPoint = new Point(xTick, height - tickLength);
                        tickLine.EndPoint = new Point(xTick, height);
                        tickGroup.Children.Add(tickLine);
                    }            
                }
                for (int i = 0; i <= yGridCount; i++)
                {
                    LineGeometry line = new LineGeometry();
                    LineGeometry tickLine = new LineGeometry();

                    //yTicks[i] = (yRange.Max - yRange.Min) / yGridCount * i;
                    //var yTick = CoordinateFromTickHelper.GetCoordinateFromTick(yTicks[i], yRange, height);
                    var yTick = i * height / yGridCount;
                    line.StartPoint = new Point(0, yTick);
                    line.EndPoint = new Point(width, yTick);
                    if (i == 0 || i == yGridCount)
                    {
                        borderGroup.Children.Add(line);

                    }
                    else if (i == yGridCount / 2)
                    {
                        centerGroup.Children.Add(line);
                    }
                    else
                    {
                        group.Children.Add(line);
                        tickLine.StartPoint = new Point(0, yTick);
                        tickLine.EndPoint = new Point(tickLength, yTick);
                        tickGroup.Children.Add(tickLine);
                    }
                }
                path.Data = group;
                centerPath.Data = centerGroup;
                BorderPath.Data = borderGroup;
                TickPath.Data = tickGroup;
                MinorTickPath.Data = minorTicGroup;
            }
        }

        
        #endregion
    }
}
