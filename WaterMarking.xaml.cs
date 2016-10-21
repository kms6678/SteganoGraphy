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

using steganography;
using System.Drawing;
using System.IO;

namespace WpfSteganography
{
    /// <summary>
    /// WaterMark.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WaterMark : Window
    {
        public bool select_image = false;
        string path = string.Empty;
        Bitmap ImageFile = null;
        System.Windows.Forms.PictureBox NewMarkImage = new System.Windows.Forms.PictureBox();

        public WaterMark()
        {
            InitializeComponent();
        }

        private void WaterMarking_exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void WaterMarking_preview_MouseEnter(object sender, MouseEventArgs e)
        {
            WaterMarking_preview.Visibility = Visibility.Hidden;
            WaterMarking_preview_aft.Visibility = Visibility.Visible;
        }

        private void WaterMarking_preview_aft_MouseLeave(object sender, MouseEventArgs e)
        {
            WaterMarking_preview_aft.Visibility = Visibility.Hidden;
            WaterMarking_preview.Visibility = Visibility.Visible;
        }

        private void WaterMarking_save_MouseEnter(object sender, MouseEventArgs e)
        {
            WaterMarking_save.Visibility = Visibility.Hidden;
            WaterMarking_save_aft.Visibility = Visibility.Visible;
        }

        private void WaterMarking_save_aft_MouseLeave(object sender, MouseEventArgs e)
        {
            WaterMarking_save_aft.Visibility = Visibility.Hidden;
            WaterMarking_save.Visibility = Visibility.Visible;
        }

        private void WaterMarking_image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openDialog.ShowDialog();

            if (!openDialog.FileName.Equals(string.Empty))
            {
                // Create image element to set as icon on the menu element
                BitmapImage bmImage = new BitmapImage();
                bmImage.BeginInit();
                bmImage.UriSource = new Uri(openDialog.FileName, UriKind.Absolute);
                bmImage.EndInit();

                path = openDialog.FileName;
                select_image = true;

                ImageBrush ib = new ImageBrush();
                ib.ImageSource = bmImage;

                WaterMarking_image.Background = ib;
            }
        }

        private void WaterMarking_preview_aft_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (select_image == false)
            {
                System.Windows.Forms.MessageBox.Show("배경 이미지 파일을 선택하지 않았습니다.", "알림", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            if (WaterMarking_text.Text.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("마킹할 텍스트를 입력하지 않았습니다.", "알림", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }
            //체크~

            Mark.Pre_ImagePath = path;
            Mark.Mark_ImageText = WaterMarking_text.Text;
            Mark.MarkOpacity = int.Parse(WaterMarking_opacity.Text);
            Bitmap temp = Mark.NewImage();

            NewMarkImage.Image = temp;

            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = bi;

            WaterMarking_image.Background = ib;
        }

        private void WaterMarking_save_aft_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            if (!path.Equals(string.Empty))
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    ImageFile = new Bitmap(Mark.ImageSize.Width, Mark.ImageSize.Height);
                    ImageFile = (Bitmap)NewMarkImage.Image;
                    this.ImageFile.Save(saveFileDialog.FileName);
                }
            }
        }
    }
}
