using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDI = System.Drawing;
using Media = System.Windows.Media;

namespace UniTrendChart.Helper
{
    public static class ColorConverterHelper
    {
        /// <summary>
        /// System.Drawing.Color to System.Windows.Media.Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Media.Color GDIColor2MediaColor(GDI.Color color)
        {
            return Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// System.Windows.Media.Color to System.Drawing.Color 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static GDI.Color MediaColor2GDIColor(Media.Color color)
        {
            return GDI.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// System.Windows.Media.Color to System.Windows.Media.Brush
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Media.Brush MediaColor2Brush(Media.Color color)
        {
            return new Media.SolidColorBrush(color);
        }

        /// <summary>
        /// System.Windows.Media.Brush to System.Windows.Media.Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Media.Color Brush2MediaColor(Media.Brush color)
        {
            return (Media.Color)Media.ColorConverter.ConvertFromString(color.ToString());
        }

        /// <summary>
        /// System.Windows.Media.Brush to System.Drawing.Color
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static GDI.Color Brush2GDIColor(Media.Brush brush)
        {
            Media.Color color = Brush2MediaColor(brush);
            return GDI.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// System.Drawing.Color to System.Windows.Media.Brush
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Media.Brush GDIColor2Brush(GDI.Color color)
        {
            Media.Color mColor = GDIColor2MediaColor(color);
            return MediaColor2Brush(mColor);
        }

        public static GDI.Color GDIColrFromHex(string Hex)
        {
            return MediaColor2GDIColor((Media.Color)Media.ColorConverter.ConvertFromString(Hex));
        }

        public static Media.Brush MediaBrushFromHex(string Hex)
        {
            return MediaColor2Brush((Media.Color)Media.ColorConverter.ConvertFromString(Hex));
        }
    }
}
