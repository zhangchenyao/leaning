using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfBase;
using WpfControlBase.Commands;
using WpfControlBase.Interfaces;

namespace WaveEditorModel.VModel.Menus
{
    public class Meum_Item: ModelBase
    {
        #region Filed
        private bool _isVisible;
        private bool _isEnable;
        private string _content;


        #endregion

        #region Properties
        public ObservableCollection<Meum_Item> MeumItems { get; private set; } = new ObservableCollection<Meum_Item>();

        public bool IsVisible 
        {
            get
            {
                return _isVisible;
            }
            set
            { 
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            } 
        }

        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
            set { _isEnable = value; OnPropertyChanged(nameof(IsEnable)); }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }


        #endregion

        /// <summary>
        /// 命令要执行的方法
        /// </summary>
       public  void UpdateNameExecute(object sender)
        {

        }

        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <returns></returns>
       public bool CanUpdateNameExecute1()
        {
            return true;
        }


    }
}
