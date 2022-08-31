using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfControlBase.Commands;
using WpfControlBase.Interfaces;

namespace WaveEditorModel.VModel.Menus
{
    internal class NewFileMenuItem: Meum_Item, ICommandFun
    {

        /// <summary>
        /// 创建新命令
        /// </summary>
        public ICommand ClickAction
        {
            get
            {
                return new RelayCommand<object>(UpdateNameExecute, CanUpdateNameExecute1);
            }
        }

    }
}
