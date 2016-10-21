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

namespace WpfSteganography
{
    /// <summary>
    /// Choose.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Choose : Window
    {
        int choose = 0;

        public Choose(int n)
        {
            choose = n;
            InitializeComponent();
        }

        private void button_imagetext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (choose == 1)
            {
                SteganoImage1 dlg = new SteganoImage1();
                dlg.ShowDialog();
                this.Close();
            }
            else
            {
                SteganoImage1Extract dlg = new SteganoImage1Extract();
                dlg.ShowDialog();
                this.Close();
            }
        }

        private void button_imageimage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (choose == 1)
            {
                SteganoImage2 dlg = new SteganoImage2();
                dlg.ShowDialog();
                this.Close();
            }
            else
            {
                SteganoImage2Extract dlg = new SteganoImage2Extract();
                dlg.ShowDialog();
                this.Close();
            }
        }

        private void button_soundtext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (choose == 1)
            {
                SteganoSound1 dlg = new SteganoSound1();
                dlg.ShowDialog();
                this.Close();
            }
            else
            {
                SteganoSound1Extract dlg = new SteganoSound1Extract();
                dlg.ShowDialog();
                this.Close();
            }
        }

        private void button_soundimage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (choose == 1)
            {
                SteganoSound2 dlg = new SteganoSound2();
                dlg.ShowDialog();
                this.Close();
            }
            else
            {
                SteganoSound2Extract dlg = new SteganoSound2Extract();
                dlg.ShowDialog();
                this.Close();
            }
        }
    }
}
