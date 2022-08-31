using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WaveEditorModel.VModel;
using WaveformGraphEditor;
using WpfControlBase.View;

namespace WaveformGraphEditorView
{
    /// <summary>
    /// IniEditorParameter.xaml 的交互逻辑
    /// </summary>
    public partial class IniEditorParameter : MessageBoxEx
    {
        public new object DataContext
        {
            get { return base.DataContext; }
            set
            {
                base.DataContext = value;
                if (DataContext is IniEditorParameterModel mode)
                {
                    mode.ErrorEventHandler += ErrorFun;
                }
            }
        }

        private void ErrorFun(object? sender, string e)
        {
            
        }

        public IniEditorParameter()
        {
            InitializeComponent();
        }

        internal void EnterBtn_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rlimitnumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled=regex.IsMatch(e.Text);
        }
    }
}
