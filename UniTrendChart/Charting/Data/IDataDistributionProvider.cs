using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTrendChart.Charting.Data
{
    /// <summary>
    /// 定义 DataDistributionProvider 的接口，提供有关 DataSeries 的不同信息
    /// </summary>
    public interface IDataDistributionProvider
    {
        /// <summary>
        /// 获取此 DataSeries 是否包含 X 方向的 Sorted 数据。
        /// 注意：排序的数据将导致更快的索引操作。 如果可能的话，尽量保持数据在 X 方向向上上排序
        /// </summary>
        bool DataIsSortedAscending { get; }

        /// <summary>
        /// 获取X数据是否均匀分布
        /// </summary>
        bool DataIsEvenlySpaced { get; }

        /// <summary>
        /// 获取 Y 数据是否包含 NaN 值
        /// </summary>
        bool DataContainsNaN { get; }
    }
}
