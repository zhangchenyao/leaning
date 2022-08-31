using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfControlBase.View
{
    public class MessageBoxEx : Window
    {
        public MessageBoxEx()
        {
            InitializeStyle();
            this.Loaded += delegate
            {
                InitializeEvent();
            };
        }

        private void InitializeEvent()
        {
            if (this.TryFindResource("BaseWindowControlTemplate") is not ControlTemplate baseWindowTemplate)
                return;
            Button Btn1 = (Button)baseWindowTemplate.FindName("cancel", this);
            Btn1.Click += delegate
            {
                DialogResult = false;
                this.Close();
            };

           
            Border borderTitle = (Border)baseWindowTemplate.FindName("borderTitle", this);
            borderTitle.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
           
        }


        private void InitializeStyle()
        {
            this.Style = (Style)Resources["BaseWindowStyle"];
        }
    }
}
