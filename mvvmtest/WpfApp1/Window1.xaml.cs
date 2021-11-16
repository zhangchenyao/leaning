using mvvmtest;
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

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        public Class1 DataContext
        {
            get {
                return base.DataContext as Class1;
            }
            set {
                base.DataContext = value;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var lis = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                lis.Add(i);
            }
            try {

                for (int i = lis.Count-1; i > 0; i--)
                {
                    if (lis[i] % 3 == 0)
                    {
                        lis.Remove(lis[i]);
                    }
                }
                //foreach (var item in lis)
                //{
                //    if (item % 3 == 0)
                //    {
                //        lis.Remove(item);
                //    }
                //}
                MessageBox.Show(lis.Count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
