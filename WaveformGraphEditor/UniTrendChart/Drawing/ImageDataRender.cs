using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace UniTrendChart.Drawing
{
    public class ImageDataRender
    {
        private int m_nCHs = 4; // 通道数
        private int m_nGrayScale = 256; //色阶
        private int colorFormat = 3; //颜色byte数 3-24位
        private int m_CoordSys_Data_Width = 700; // 像素表宽度
        private int m_CoordSys_Data_Height = 200; //像素表高度
        private double m_rScreenToData_X;
        private double m_rScreenToData_Y;
        private Color[][] m_crTable; // 颜色表
        private Bitmap bitmap;

        int CvtToScreenX(int _x)
        {
            return (int)(Math.Round(m_rScreenToData_X * _x));
        }
        int CvtToScreenY(int _y)
        {
            return (int)(Math.Round(m_rScreenToData_Y * _y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_data_cs_width"></param>
        /// <param name="_data_cs_height"></param>
        /// <param name="m_rcScreenWidth"></param>
        /// <param name="m_rcScreenHeight"></param>
        /// <param name="_ch_number">4</param>
        public void Create(int _data_cs_width, int _data_cs_height, System.Drawing.Bitmap bitmap, int _ch_number)
        {
            m_nCHs = _ch_number;
            m_CoordSys_Data_Width = _data_cs_width;
            m_CoordSys_Data_Height = _data_cs_height;
            this.bitmap = bitmap;
            m_rScreenToData_X = bitmap.Width / (double)_data_cs_width;
            m_rScreenToData_Y = bitmap.Height / (double)_data_cs_height;
        }

        /// <summary>
        /// 颜色表
        /// </summary>
        /// <param name="colorFormat">24位 - 3</param>
        /// <param name="_data">颜色表数据</param>
        public void RefreshTable(int colorFormat, byte[] _data)
        {
            m_crTable = new Color[m_nCHs][];
            if (colorFormat == 3)
            {
                for (int c = 0, pc = 0; c < m_nCHs; c++)
                {
                    m_crTable[c] = new Color[m_nGrayScale];
                    for (int g = 0; g < m_nGrayScale && pc < _data.Length; g++)
                    {
                        
                        var B = _data[pc++];
                        var G = _data[pc++];
                        var R = _data[pc++];
                        pc++;
                        m_crTable[c][g] = Color.FromArgb(R, G, B);
                    }
                }
            }
        }

        public void Render(byte[] data)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            Parallel.For(0, data.Length / 2, (i) =>
            {
                int c = i % m_CoordSys_Data_Width;
                int l = i / m_CoordSys_Data_Width;
                int lNext = l >= m_CoordSys_Data_Height ? m_CoordSys_Data_Height : l + 1;
                int cNext = c >= m_CoordSys_Data_Width ? m_CoordSys_Data_Width : c + 1;
                var color = m_crTable[data[i * 2 + 1]][data[i * 2]];
                int yCur = CvtToScreenY(l), yNext = CvtToScreenY(lNext);
                int xCur = CvtToScreenX(c), xNext = CvtToScreenX(cNext);
                if (color.R == 0 && color.G == 0 && color.B == 0)
                {
                    color = Color.FromArgb(35,36,38);
                }
                for (int y = yCur; y < yNext; y++)
                {
                    for (int x = xCur; x < xNext; x++)
                    {
                        int index = (y * bmpData.Stride + x * 4);
                        rgbValues[index] = color.B;
                        rgbValues[index + 1] = color.G;
                        rgbValues[index + 2] = color.R;
                    }
                }
            });
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bitmap.UnlockBits(bmpData);           
        }
    }
}
