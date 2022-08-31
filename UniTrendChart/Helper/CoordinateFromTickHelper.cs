using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTrendChart.Data;

namespace UniTrendChart.Helper
{
    public static class CoordinateFromTickHelper
    {
        public static double GetCoordinateFromTick(double tick, Range range, double length)
        {
            return tick * length / (range.Max - range.Min);
        }
    }
}
