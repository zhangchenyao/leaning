
using CommonTools;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SericeTool;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WaveEditorModel.VModel.Menus;
using WaveEditorModel.VModel.VModel;
using WaveformGraphEditor.VModel;

namespace WaveformGraphEditor
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
       
       
        public Window2()
        {
            InitializeComponent();
            DataContext = new winmode();
            us1.DataContext = new WaveEditorMode();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private List<Point> GetPointChangedList(Point Point1 ,Point point2,List<Point> points)
        {
            var list=new List<Point>(points.Count);
            #region 
            //  foreach(Point p in points)
            //{
            //    if(p.X<Point1.X)
            //    {
            //        list.Add(p);
            //        continue;
            //    }

            //    if(p.X==Point1.X)
            //    {
            //        list.Add(Point1);
            //        continue;
            //    }
            //    if(p.X>Point1.X&&p.X<point2.X)
            //    {
            //        continue;
            //    }
            //    if(p.X==point2.X)
            //    {
            //        list.Add(point2);
            //        continue;
            //    }
            //    list.Add(p);

            //}
            #endregion


            foreach (var point in points)
            {
                if(point.X>= Point1.X && point.X<= point2.X)
                {
                    var y=(point.X - Point1.X) / (point2.X - Point1.X) * (point2.Y - Point1.Y) + Point1.Y;
                    list.Add(new Point(point.X, y));

                }else
                {
                    list.Add(new Point(point.X, point.Y));
                }
            }
            if (list[list.Count - 1].X <= Point1.X)
            {
                list.Add(Point1);
                list.Add(point2);
            }
            return  list;
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        //private void mycanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton != MouseButtonState.Pressed)
        //    {
        //        return;
        //    }
        //    Point point = e.GetPosition(mycanvas);
        //    if (PointLists.Count == 0)
        //    {

        //        PointLists.Add(new Point(StaPoint.X, StaPoint.Y));
        //    }
        //    else
        //    {
        //        if (lastpoint.X > point.X)
        //        {
        //            PointLists = GetPointChangedList(point, lastpoint, PointLists);
        //        }
        //        else
        //        {
        //            PointLists = GetPointChangedList(lastpoint, point, PointLists);
        //        }


        //    }
        //    lastpoint = point;

        //    if (path1.Data == null)
        //    {
        //        PathGeometry pathGeometry = new();
        //        PathFigure value1 = new();
        //        pathGeometry.Figures.Add(value1);
        //        value1.StartPoint = StaPoint;
        //        path1.Data = pathGeometry;
        //    }


        //    PathGeometry? pathGeometry1 = (path1.Data as PathGeometry);
        //    PathFigure pathFigure = pathGeometry1.Figures[0];

        //    pathFigure.Segments.Clear();
        //    foreach (var item in PointLists)
        //    {
        //        pathFigure.Segments.Add(new LineSegment(item, true));
        //    }

        //}
    }

    internal class JsonMenus
    {
        
        public string Heard { get; set; }
        public string Name { get; set; }

        public int Id { get; set; }

        public List<JsonMenus> Jsons { get; set; } =new List<JsonMenus> ();
        public int MenuType { get; set; }
        
    }

        class aa
    {
        public int num { get; set; }

        public List<aa> aas { get; set; }=new List<aa>();
    }
}
