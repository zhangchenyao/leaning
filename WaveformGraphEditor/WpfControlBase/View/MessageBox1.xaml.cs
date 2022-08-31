using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfControlBase.View
{
    /// <summary>
    /// MessageBox1.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox1 : MessageBoxEx
    {
        public MessageBox1(string str)
        {
            InitializeComponent();
            textBlock1.Text = str;
        }
    }
}
