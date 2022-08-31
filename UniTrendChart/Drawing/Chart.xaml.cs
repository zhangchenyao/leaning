using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniTrendChart.Data;

namespace UniTrendChart.Drawing
{
    /// <summary>
    /// Chart.xaml 的交互逻辑
    /// </summary>
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();
            RenderDataSeries.CollectionChanged += RenderDataSeries_CollectionChanged;
        }

        private void RenderDataSeries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ((XyDataSeries)e.NewItems[0]).OnStyleChanged += Chart_OnStyleChanged ;
                ((XyDataSeries)(e.NewItems[0])).OnDataChanged += Chart_OnDataChanged;
            }
            
        }

        private void Chart_OnStyleChanged(XyDataSeriesStyle seriesStyle, string name)
        {

            Application.Current?.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (seriesStyle.IsSelect)
                    {
                        XUnit = mouseNavigation.CalculateCursorX();
                        YUnit = mouseNavigation.CalculateCursorY();
                        if (bitmap.isData)
                        {
                            bitmap.Draw();
                        }
                        else
                        {
                            bitmap.DrawImage(bitmap.colorTable, bitmap.pxiTable);
                        }

                    }
                }));
        }

        private void Chart_OnDataChanged(IList<double> x, IList<double> y)
        {
            
        }

        public bool IsZoomX
        {
            get { return (bool)GetValue(IsZoomXProperty); }
            set { SetValue(IsZoomXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsZoomX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsZoomXProperty =
            DependencyProperty.Register("IsZoomX", typeof(bool), typeof(Chart), new PropertyMetadata(true));

        public bool IsZoomY
        {
            get { return (bool)GetValue(IsZoomYProperty); }
            set { SetValue(IsZoomYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsZoomY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsZoomYProperty =
            DependencyProperty.Register("IsZoomY", typeof(bool), typeof(Chart), new PropertyMetadata(true));


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
            DependencyProperty.Register("IsAutoScal", typeof(bool), typeof(Chart), new PropertyMetadata(true));
        #endregion

        #region 网格数
        public int XGridCount
        {
            get { return (int)GetValue(XGridCountProperty); }
            set { SetValue(XGridCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XGridCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XGridCountProperty =
            DependencyProperty.Register("XGridCount", typeof(int), typeof(Chart), new PropertyMetadata(14));

        public int YGridCount
        {
            get { return (int)GetValue(YGridCountProperty); }
            set { SetValue(YGridCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YGridCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YGridCountProperty =
            DependencyProperty.Register("YGridCount", typeof(int), typeof(Chart), new PropertyMetadata(8));
        #endregion

        #region 是否显示标尺
        public bool IsCursorTimeVisible
        {
            get { return (bool)GetValue(IsCursorTimeVisibleProperty); }
            set { SetValue(IsCursorTimeVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCursorXVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCursorTimeVisibleProperty =
            DependencyProperty.Register("IsCursorTimeVisible", typeof(bool), typeof(Chart), new PropertyMetadata(false));



        public bool IsCursorVoltageVisible
        {
            get { return (bool)GetValue(IsCursorVoltageVisibleProperty); }
            set { SetValue(IsCursorVoltageVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCursorYVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCursorVoltageVisibleProperty =
            DependencyProperty.Register("IsCursorVoltageVisible", typeof(bool), typeof(Chart), new PropertyMetadata(false));
        #endregion

        #region 绘图质量
        public DrawQualityEnum DrawQuality
        {
            get { return (DrawQualityEnum)GetValue(DrawQualityProperty); }
            set { SetValue(DrawQualityProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrawQualityProperty =
            DependencyProperty.Register("DrawQuality", typeof(DrawQualityEnum), typeof(Chart), new PropertyMetadata(DrawQualityEnum.HighQuality));
        #endregion

        #region 标尺坐标
        public double SliceX0
        {
            get { return (double)GetValue(SliceX0Property); }
            set { SetValue(SliceX0Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceX0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceX0Property =
            DependencyProperty.Register("SliceX0", typeof(double), typeof(Chart), new PropertyMetadata(0.0));

        public double SliceX1
        {
            get { return (double)GetValue(SliceX1Property); }
            set { SetValue(SliceX1Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceX1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceX1Property =
            DependencyProperty.Register("SliceX1", typeof(double), typeof(Chart), new PropertyMetadata(0.0));

        public double SliceY0
        {
            get { return (double)GetValue(SliceY0Property); }
            set { SetValue(SliceY0Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceY0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceY0Property =
            DependencyProperty.Register("SliceY0", typeof(double), typeof(Chart), new PropertyMetadata(0.0));

        public double SliceY1
        {
            get { return (double)GetValue(SliceY1Property); }
            set { SetValue(SliceY1Property, value); }
        }

        // Using a DependencyProperty as the backing store for SliceY1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliceY1Property =
            DependencyProperty.Register("SliceY1", typeof(double), typeof(Chart), new PropertyMetadata(0.0));
        #endregion

        public bool OnlyDisplaySelect
        {
            get { return (bool)GetValue(OnlyDisplaySelectProperty); }
            set { SetValue(OnlyDisplaySelectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnlyDisplaySelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnlyDisplaySelectProperty =
            DependencyProperty.Register("OnlyDisplaySelect", typeof(bool), typeof(Chart), new PropertyMetadata(false));

        #region 迹线集合
        public ObservableCollection<XyDataSeries> RenderDataSeries
        {
            get { return (ObservableCollection<XyDataSeries>)GetValue(RenderDataSeriesProperty); }
            set { SetValue(RenderDataSeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenderDataSeries.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderDataSeriesProperty =
            DependencyProperty.Register("RenderDataSeries", typeof(ObservableCollection<XyDataSeries>), typeof(Chart), new PropertyMetadata(new ObservableCollection<XyDataSeries>()));
        #endregion

        #region 单位
        /// <summary>
        /// X轴单位
        /// </summary>
        public string XUnit
        {
            get { return (string)GetValue(XUnitProperty); }
            set { SetValue(XUnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XUnitProperty =
            DependencyProperty.Register("XUnit", typeof(string), typeof(Chart), new PropertyMetadata(""));


        /// <summary>
        /// Y轴单位
        /// </summary>
        public string YUnit
        {
            get { return (string)GetValue(YUnitProperty); }
            set { SetValue(YUnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YUnit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YUnitProperty =
            DependencyProperty.Register("YUnit", typeof(string), typeof(Chart), new PropertyMetadata(""));


        #endregion

        #region  是否可以平移缩放
        public bool ZoomEnable
        {
            get { return (bool)GetValue(ZoomEnableProperty); }
            set { SetValue(ZoomEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoomEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomEnableProperty =
            DependencyProperty.Register("ZoomEnable", typeof(bool), typeof(Chart), new PropertyMetadata(true));
        #endregion

        public void Clear()
        {
            bitmap.ClearAll();
        }
        

        public void Draw()
        {
            XUnit = mouseNavigation.CalculateCursorX();
            YUnit = mouseNavigation.CalculateCursorY();
            bitmap.Draw();
        }
        public void DrawImage(byte[] colorTable, byte[] pxiTable)
        {
            XUnit = mouseNavigation.CalculateCursorX();
            YUnit = mouseNavigation.CalculateCursorY();
            bitmap.DrawImage(colorTable, pxiTable);
        }
        public void AutoScale()
        {
            bitmap.AutoScale();
            XUnit = mouseNavigation.CalculateCursorX();
            YUnit = mouseNavigation.CalculateCursorY();
        }

        private void chart_Loaded(object sender, RoutedEventArgs e)
        {
            mouseNavigation.RenderDataSeries = this.RenderDataSeries;
            bitmap.RenderDataSeries = this.RenderDataSeries;
        }
    }
}
