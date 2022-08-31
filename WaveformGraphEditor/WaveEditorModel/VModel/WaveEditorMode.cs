using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WaveEditorModel.VModel;
using WpfBase;

namespace WaveformGraphEditor.VModel
{
    public class WaveEditorMode : ModelBase
    {
        #region Field
        private List<string> yList = new() { "1.000v", "750mv", "500mv", "250mv", "0", "-250mv", "-500mv", "-750mv", "-1.000v", "-55" };
        private string _title = string.Empty;


        private double ratio = 1;
        #endregion

        #region Properties

        public Point NowPoint
        {
            get
            {
                return _NowPoint;
            }
            set
            {
                _NowPoint = value;
                if (value.X > LastPoint.X)
                {
                    ChangePoint(LastPoint, value);
                }
                else
                {
                    ChangePoint(value, LastPoint);
                }
                LastPoint = value;
            }
        }

        public Tuple<Point,Point>[] points;
        public Point LastPoint { get; set; }

        public string Title
        {
            get { return _title; }

            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public List<string> YList
        {
            get { return yList; }
            set
            {
                yList = value;
                OnPropertyChanged(nameof(YList));
            }
        }

        public List<XTag> XTags
        { get; set; } = new List<XTag>();
        public WorkType WorkType { get; set; }
        public List< System.Drawing.PointF> PointFs { get; set; }=new List<System.Drawing.PointF>();

        public EventHandler Closed;
        private Point _NowPoint;

        public void Close(object? sender, EventArgs e)
        {
            Closed?.Invoke(this, e);
        }



        #endregion

        #region Private Methods
        /// <summary>
        /// p1.x<p2.x
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        private void ChangePoint(Point point1, Point point2)
        {

            int v = (int)(point2.X - point1.X + 1);

            points = new Tuple<Point, Point>[v];
            for (int i = 0; i < points.Length; i++)
            {
                double x = point1.X + i;
                double y = (x - point1.X) / (point2.X - point1.X) * (point2.Y - point1.Y) + point1.Y;
                points[i] = new Tuple<Point, Point>(new Point(x, y), new Point(-100,-100)); 
            }

            var list = new List<System.Drawing.PointF>();

            if(PointFs.Count==0)
            {
                list.Add( new System.Drawing.PointF((float)NowPoint.X, (float)NowPoint.Y) );
            }
            else
            {

                if (point1.X < PointFs[0].X)
                {
                    list.Add(new System.Drawing.PointF((float)point1.X, (float)point1.Y));
                }

                if (point2.X < PointFs[0].X)
                {
                    list.Add(new System.Drawing.PointF((float)point2.X, (float)point2.Y));
                }
                foreach (var  item in PointFs)
                {
                    if (item.X >= point1.X && item.X <= point2.X)
                    {
                        var y = (item.X - point1.X) / (point2.X - point1.X) * (point2.Y - point1.Y) + point1.Y;
                        list.Add(new System.Drawing.PointF(item.X, (float)y));

                    }
                    else
                    {
                        list.Add(new System.Drawing.PointF(item.X, item.Y));
                    }
                }

                if (list[list.Count - 1].X <= point1.X)
                {
                    list.Add(new System.Drawing.PointF((float)point1.X, (float)point1.Y) );
                    list.Add(new System.Drawing.PointF((float)point2.X, (float)point2.Y));
                }



            }

            PointFs = list;

        }
        #endregion

        public void Inidata(IniEditorParameterModel mode)
        {

        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void Undo()
        {

        }
    }

    public enum WorkType
    {
        Editor = 0,

        View = 1,
    }
    public class XTag : ModelBase
    {
        private string _Count = string.Empty;
        private string _time = string.Empty;

        public string Count
        {
            get { return _Count; }
            set { _Count = value; OnPropertyChanged(nameof(Count)); }
        }

        public string Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged(nameof(Time)); }
        }
    }
}
