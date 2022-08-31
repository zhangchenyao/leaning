using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UniTrendChart
{
    public enum DrawQualityEnum
    {
        [Description("高速度绘图")]
        HighSpeed = 0,
        [Description("高质量绘图")]
        HighQuality = 1
    }
}
