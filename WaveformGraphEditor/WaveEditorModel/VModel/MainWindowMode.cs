using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WaveEditorModel.VModel.Menus;
using WaveformGraphEditor.VModel;
using WpfBase;


namespace WaveEditorModel.VModel.VModel
{
    public class MainWindowMode : ModelBase
    {
        public ObservableCollection<WaveEditorMode> editorModes { get; set; } = new ObservableCollection<WaveEditorMode>();
        public ObservableCollection<MenuItem> MeumItemss { get; set; } = new ObservableCollection<MenuItem>();

        #region Public Methods
       
        #endregion

        #region Private Methods
        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <returns></returns>
        bool CanUpdateNameExecute()
        {
            return true;
        }

        

        public void Execute(WaveEditorMode mode)
        {
            mode.Title = $"文件{editorModes.Count+1}";
            mode.Closed += Remove;
            editorModes.Add(mode);
           
        }

        private void Remove(object? sender, EventArgs e)
        {
            try
            {
                if(sender is WaveEditorMode mode)
                {
                    editorModes.Remove(mode);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

       

    }
}
