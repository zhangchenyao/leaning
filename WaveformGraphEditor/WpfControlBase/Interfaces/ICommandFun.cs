using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfControlBase.Commands;

namespace WpfControlBase.Interfaces
{
    public interface ICommandFun
    {
        /// <summary>
        /// 命令要执行的方法
        /// </summary>
        void UpdateNameExecute(object sender);


        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <returns></returns>
        bool CanUpdateNameExecute1();

        /// <summary>
        /// 创建新命令
        /// </summary>
        public ICommand ClickAction { get; }
        
    }
}
