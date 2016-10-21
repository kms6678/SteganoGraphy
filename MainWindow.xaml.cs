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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSteganography
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        string temp = AppDomain.CurrentDomain.BaseDirectory;
        int choose = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void button_crypto_before_MouseEnter(object sender, MouseEventArgs e)
        {
            button_crypto_before.Visibility = Visibility.Hidden;
            button_crypto_aft.Visibility = Visibility.Visible;
        }

        private void button_crypto_aft_MouseLeave(object sender, MouseEventArgs e)
        {
            button_crypto_aft.Visibility = Visibility.Hidden;
            button_crypto_before.Visibility = Visibility.Visible;
        }

        private void button_extract_before_MouseEnter(object sender, MouseEventArgs e)
        {
            button_extract_before.Visibility = Visibility.Hidden;
            button_extract_aft.Visibility = Visibility.Visible;
        }

        private void button_extract_aft_MouseLeave(object sender, MouseEventArgs e)
        {
            button_extract_aft.Visibility = Visibility.Hidden;
            button_extract_before.Visibility = Visibility.Visible;
        }

        private void button_watermarking_befoer_MouseEnter(object sender, MouseEventArgs e)
        {
            button_watermarking_befoer.Visibility = Visibility.Hidden;
            button_watermarking_aft.Visibility = Visibility.Visible;
        }

        private void button_watermarking_aft_MouseLeave(object sender, MouseEventArgs e)
        {
            button_watermarking_aft.Visibility = Visibility.Hidden;
            button_watermarking_befoer.Visibility = Visibility.Visible;
        }

        private void button_info_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            informationWindow dlg = new informationWindow();
            dlg.ShowDialog();
        }

        private void button_crypto_aft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Choose dlg = new Choose(1);
            dlg.ShowDialog();
        }

        private void button_extract_aft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Choose dlg = new Choose(2);
            dlg.ShowDialog();
        }

        private void button_watermarking_aft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WaterMark dlg = new WaterMark();
            dlg.ShowDialog();
        }
    }
}
