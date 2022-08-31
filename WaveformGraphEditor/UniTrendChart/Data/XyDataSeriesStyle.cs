using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace UniTrendChart.Data
{
    public delegate void StyleChangedEventHandler(XyDataSeriesStyle seriesStyle, string name);
    public class XyDataSeriesStyle
    {
        public event StyleChangedEventHandler OnStyleChanged;

        public void NotifyStyleChangedEvent(string name)
        {
            if (OnStyleChanged != null)
            {
                OnStyleChanged(this, name);
            }
        }

        private string m_SeriesName;
        /// <summary>
        /// 获取或设置 DataSeries 名称
        /// </summary>
        public string SeriesName
        {
            get { return m_SeriesName; }
            set 
            {
                if (SeriesName == value) return;
                m_SeriesName = value;
                NotifyStyleChangedEvent("SeriesName");
            }
        }
        
        private object m_Tag;
        /// <summary>
        /// 获取或设置 DataSeries 上的任意标记
        /// </summary>
        public object Tag
        {
            get { return m_Tag; }
            set 
            {
                if (m_Tag == value) return;
                m_Tag = value;
                NotifyStyleChangedEvent("Tag");
            }
        }

        private Color m_Color;
        /// <summary>
        /// 获取或设置 DataSeries 颜色
        /// </summary>
        public Color Color
        {
            get { return m_Color; }
            set 
            {
                if (m_Color == value) return;
                m_Color = value;
                NotifyStyleChangedEvent("Color");
            }
        }

        private float m_Thickness = 1;
        /// <summary>
        /// 获取或设置 DataSeries 粗细
        /// </summary>
        public float Thickness
        {
            get { return m_Thickness; }
            set 
            {
                if (m_Thickness == value) return;
                m_Thickness = value;
                NotifyStyleChangedEvent("Thickness");
            }
        }

        private bool m_IsSelect;
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsSelect
        {
            get { return m_IsSelect; }
            set
            {
                if (m_IsSelect == value) return;
                m_IsSelect = value;
                NotifyStyleChangedEvent("IsSelect");
            }
        }

        private Range m_XVisibleRange;
        /// <summary>
        /// x轴可视范围
        /// </summary>
        public Range XVisibleRange
        {
            get { return m_XVisibleRange; }
            set
            {
                if (m_XVisibleRange == value) return;
                m_XVisibleRange = value;
                //NotifyStyleChangedEvent("XVisibleRange");
            }
        }

        private Range m_YVisibleRange;
        /// <summary>
        /// Y轴范围
        /// </summary>
        public Range YVisibleRange
        {
            get { return m_YVisibleRange; }
            set
            {
                if (m_YVisibleRange == value) return;
                m_YVisibleRange = value;
                //NotifyStyleChangedEvent("YVisibleRange");
            }
        }
    }
}
