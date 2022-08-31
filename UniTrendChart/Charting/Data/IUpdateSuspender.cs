using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTrendChart.Charting.Data
{
    /// <summary>
    /// 允许在目标上嵌套挂起/恢复操作的一次性类
    /// </summary>
    public interface IUpdateSuspender : IDisposable
    {
        /// <summary>
        /// 获取一个值，该值指示此实例的更新当前是否已暂停
        /// </summary>
        bool IsSuspended { get; }

        /// <summary>
        /// 获取或设置一个值，该值指示在处置 IUpdateSuspender 时目标是否将恢复。 默认为真
        /// </summary>
        bool ResumeTargetOnDispose { get; set; }

        /// <summary>
        /// 获取或设置此实例的关联 Tag
        /// </summary>
        object Tag { get; }
    }
}
