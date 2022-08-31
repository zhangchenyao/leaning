using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GDI = System.Drawing;

namespace UniTrendChart.Data
{
    public delegate void DataSeriesChangedEventHandler(IList<double> x, IList<double>y);
    public class XyDataSeries : XyDataSeriesStyle
    {
        public XyDataSeries(string name, Color color)
        {
            SeriesName = name;
            Color = color;
            Thickness = 1;
        }

        public XyDataSeries()
        {

        }
        public event DataSeriesChangedEventHandler OnDataChanged;
        /// <summary>
        /// 当 AcceptsUnsortedData 为 false 时，如果附加了未排序的数据，
        /// DataSeries 将抛出 InvalidOperationException。 
        /// 无意的未排序数据会导致性能下降
        /// </summary>
        public bool AcceptsUnsortedData { get; set; }
        /// <summary>
        /// 获取此DataSeries的更改计数。 允许识别DataSeries何时更改
        /// </summary>
        public int ChangeCount { get; private set; }

        /// <summary>
        /// 获取此DataSeries中的点数
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中X的最大值
        /// </summary>
        public double XMax { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中X的最小值
        /// </summary>
        public double XMin { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中X的范围
        /// </summary>
        public Range XRange { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中X的数据
        /// </summary>
        public IList<double> XValues { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中Y的最大值
        /// </summary>
        public double YMax { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中Y的最小值
        /// </summary>
        public double YMin { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中Y的范围
        /// </summary>
        public Range YRange { get; private set; }

        /// <summary>
        /// 获取此 DataSeries 中Y的数据
        /// </summary>
        public IList<double> YValues { get; private set; }

        /// <summary>
        /// 获取DataSeries中的像素点
        /// </summary>
        public List<GDI.PointF> PointFs { get; private set; }

        /// <summary>
        /// X单位
        /// </summary>
        public string XUnit { get; set; } = "";

        /// <summary>
        /// Y单位
        /// </summary>
        public string YUnit { get; set; } = "";

        /// <summary>
        /// X初始轴范围
        /// </summary>
        public Range XOrignalVisibleRange { get; set; }

        /// <summary>
        /// Y初始轴范围
        /// </summary>
        public Range YOrignalVisibleRange { get; set; }

        //每个像素对应的迹线最大值最小值集合
        private int[] pointInfo;


        /// <summary>
        /// 将 X、Y 点的列表添加到DataSeries。
        /// </summary>
        /// <param name="x">点的X列表</param>
        /// <param name="y">点的Y列表</param>
        public void UpdateData(IList<double> x, IList<double> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("x");
            }
            if (x.Count() != y.Count())
            {
                throw new ArgumentException("x 和 y 长度不一致");
            }
            if (x.Count() == 0)
            {
                throw new ArgumentException("x 没有数据");
            }
            this.XValues = (IList<double>)x;
            this.YValues = (IList<double>)y;            
            this.ChangeCount = x.Count;
            this.Count = this.XValues.Count();
            if (OnDataChanged != null)
            {
                OnDataChanged(XValues, YValues);
            }
        }

        /// <summary>
        /// 将 X、Y 点添加到DataSeries。
        /// </summary>
        /// <param name="x">点的X值</param>
        /// <param name="y">点的Y值</param>
        public void AppendData(double x, double y)
        {
            this.XValues.Add(x);
            this.YValues.Add(y);
            this.XMax = this.XMax > x ? this.XMax : x;
            this.XMin = this.XMin < x ? this.XMin : x;
            this.YMax = this.YMax > y ? this.YMax : y;
            this.YMin = this.YMin < y ? this.YMin : y;
            this.ChangeCount = 1;
            this.Count += 1;
            this.XRange = new Range(this.XMin, this.XMax);
            this.YRange = new Range(this.YMin, this.YMax);
            if (OnDataChanged != null)
            {
                OnDataChanged(XValues, YValues);
            }
        }

        /// <summary>
        /// 清除数据，将内部列表重置为零大小
        /// </summary>
        public void Clear()
        {
            this.XValues.Clear();
            this.YValues.Clear();
            this.XMax = 0;
            this.XMin = 0;
            this.YMax = 0;
            this.YMin = 0;
            this.ChangeCount = this.Count;
            this.Count = 0;
            this.XRange = new Range(0, 0);
            this.YRange = new Range(0, 0);
            if (OnDataChanged != null)
            {
                OnDataChanged(XValues, YValues);
            }
        }

        /// <summary>
        /// 将迹线数据转换为像素数据
        /// </summary>
        /// <param name="picWidth">像素宽度</param>
        /// <param name="picHeight">像素高度</param>
        /// <param name="startIndex">显示数据起始索引</param>
        /// <param name="endIndex">显示数据结束索引</param>
        /// <param name="minx">显示数据X最小值</param>
        /// <param name="xlength">显示x数据长度</param>
        /// <param name="miny">显示数据y最大值</param>
        /// <param name="ylength">显示y数据长度</param>
        internal void Data2Pxi(int picWidth,int picHeight, int startIndex, int endIndex, double minx, double xlength, double miny, double ylength)
        {
            //Stopwatch stopwatch = Stopwatch.StartNew();
            pointInfo = new int[3 * picWidth];
            Parallel.For(0, picWidth, j =>
            {
                //二分法找到当前像素对应数据第一个点
                bool flag = false;
                int low = startIndex;
                int high = endIndex;
                while (low < high)
                {
                    int mid = (low + high) / 2;
                    var pointx = (XValues[mid] - minx) / xlength * picWidth;
                    if (pointx < j)
                    {
                        low = mid + 1;
                    }
                    else
                    {
                        high = mid;
                    }
                }
                double pointlast = (XValues[high] - minx) / xlength * picWidth;
                if (pointlast >= j)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                //找出当前像素对应的所有数据点的最大值和最小值
                if (flag)
                {
                    double pointyMin = double.MaxValue;
                    double pointyMax = double.MinValue;
                    int minIndex = -1;
                    int maxIndex = -1;
                    var pointx = (int)((XValues[high] - minx) / xlength * picWidth);
                    int count = high;
                    double minxValue = (1 + pointx) * xlength / picWidth + minx;//(int)((x[i][count] - minx) / xlength * width) == pointx  //x[i][count] < minxValue
                    int curLineLength = XValues.Count;
                    while (count < curLineLength && XValues[count] <= minxValue)
                    {
                        var pointy = YValues[count];
                        if (pointyMin > pointy)
                        {
                            pointyMin = pointy;
                            minIndex = count;
                        }
                        if (pointyMax < pointy)
                        {
                            pointyMax = pointy;
                            maxIndex = count;
                        }
                        count++;
                    }
                    if (minIndex >= 0 && maxIndex >= 0)
                    {
                        pointInfo[3 * j] = 1;
                        pointInfo[3 * j + 1] = minIndex;
                        pointInfo[3 * j + 2] = maxIndex;
                    }

                }
            });
            PointFs = new List<GDI.PointF>();
            //缩放情况下添加屏幕外的第一个点
            if (startIndex > 0)
            {
                float px = (float)((XValues[startIndex - 1] - minx) / xlength * picWidth);
                float py = (float)((1 - (YValues[startIndex - 1] - miny) / ylength) * picHeight);
                PointFs.Add(new GDI.PointF(px, py));
            }
            for (int j = 0; j < picWidth; j++)
            {
                if (pointInfo[3 * j] == 1)
                {
                    float px = (float)((XValues[pointInfo[3 * j + 1]] - minx) / xlength * picWidth);
                    float px1 = (float)((XValues[pointInfo[3 * j + 2]] - minx) / xlength * picWidth);
                    float py = (float)((1 - (YValues[pointInfo[3 * j + 1]] - miny) / ylength) * picHeight);
                    float py1 = (float)((1 - (YValues[pointInfo[3 * j + 2]] - miny) / ylength) * picHeight);
                    PointFs.Add(new GDI.PointF(px, py));
                    PointFs.Add(new GDI.PointF(px1, py1));
                }
            }
            //缩放情况下添加屏幕外的最后一个点
            if (endIndex < XValues.Count - 1)
            {
                float px = (float)((XValues[endIndex + 1] - minx) / xlength * picWidth);
                float py = (float)((1 - (YValues[endIndex + 1] - miny) / ylength) * picHeight);
                PointFs.Add(new GDI.PointF(px, py));
            }
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);   
        }

        /// <summary>
        /// 找到最接近给定 X 和 Y 值的点的线。 搜索区域是一个垂直区域，以 X 为中心，向左和向右 [maxXDistance] X 单位
        /// </summary>
        /// <param name="x">点的 X 值 【X 数据单位，而不是像素】</param>
        /// <param name="y">点的 Y 值 【Y 数据单位，而不是像素】</param>
        /// <param name="xyScaleRatio">x每像素单位/y每像素单位 </param>
        /// <param name="hitTestRadius">以 X 单位指定搜索区域</param>
        /// <returns>找到值的索引，如果没有找到返回 -1（当计数为零时）</returns>
        [Obsolete]
        public int FindClosestPoint(IComparable x,IComparable y,double xyScaleRatio, double hitTestRadius)
        {

            return -1;
        }
    }
}
