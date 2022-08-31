using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTrendChart.Data
{
    public class Range
    {
        public Range()
        {
            Min = 0;
            Max = 1;
        }
        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public Range(double min, double max, double k)
        {
            Min = min - (max - min) * (1 - k) / 2;
            Max = max + (max - min) * (1 - k) / 2;
        }
        /// <summary>
        /// 范围最小值
        /// </summary>
        public double Min { get;}

        /// <summary>
        /// 范围最大值
        /// </summary>
        public double Max { get;}

        /// <summary>
        /// 范围长度
        /// </summary>
        public double Length
        {
            get { return Max - Min; }
        }

        /// <summary>
        /// 范围是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (Min == Max)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return "(" + Min + "," + Max + ")";
        }
    }
}
