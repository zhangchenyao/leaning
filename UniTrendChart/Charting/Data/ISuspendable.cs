using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTrendChart.Charting.Data
{
    public interface ISuspendable
    {
        /// <summary>
        /// 获取一个值，该值指示当前是否暂停了目标的更新
        /// </summary>
        bool IsSuspended { get; }

        /// <summary>
        /// 暂停目标上的绘图更新，直到返回的对象被释放，届时将发出最终的绘图调用
        /// </summary>
        /// <returns>一次性更新暂停器</returns>
        IUpdateSuspender SuspendUpdates();

        /// <summary>
        /// 恢复目标上的更新，旨在由 IUpdateSuspender 调用
        /// </summary>
        void ResumeUpdates(IUpdateSuspender suspender);

        /// <summary>
        /// 每次处理目标时由 IUpdateSuspender 调用。 当最终目标 suspender 被释放后，ResumeUpdates 被调用
        /// </summary>
        void DecrementSuspend();
    }
}
