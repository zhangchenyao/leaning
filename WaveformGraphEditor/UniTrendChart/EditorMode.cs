using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UniTrendChart
{
    public class EditorMode
    {
        
       

       public Point NowPoint { get { return _NowPoint; } set { _NowPoint = value;
            if(NowPoint.X>lastpoint.X)
                {
                    GetPointChangedList(lastpoint, NowPoint, PointLists);
                }
            else
                {
                    GetPointChangedList( NowPoint,lastpoint, PointLists);
                }
            } }

       public List<Point> PointLists = new List<Point>();
        private Point _NowPoint;

        public Point lastpoint { get; set; }

        /// <summary>
        /// p1.x<p2.x
        /// </summary>
        /// <param name="Point1"></param>
        /// <param name="point2"></param>
        /// <param name="points"></param>
        private void GetPointChangedList(Point Point1, Point point2, List<Point> points)
        {
            var list = new List<Point>(points.Count);
            foreach (var point in points)
            {
                if (point.X >= Point1.X && point.X <= point2.X)
                {
                    var y = (point.X - Point1.X) / (point2.X - Point1.X) * (point2.Y - Point1.Y) + Point1.Y;
                    list.Add(new Point(point.X, y));

                }
                else
                {
                    list.Add(new Point(point.X, point.Y));
                }
            }
            if (list[list.Count - 1].X <= Point1.X)
            {
                list.Add(Point1);
                list.Add(point2);
            }
            PointLists = list;
        }
    }

    
}
