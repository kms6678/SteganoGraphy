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

using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using steganography;

namespace WpfSteganography
{
    /// <summary>
    /// SteganoImageExtract.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoImage1Extract : Window
    {
        SteganographyConvert stegano = new SteganographyConvert();
        System.Drawing.Bitmap ImgBmp = null;

        public bool select_image = false;

        public SteganoImage1Extract()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

                select_image = true;
                SteganoImage1Extract_image.Source = bmImage;
            }
        }

        private void SteganoImage1Extract_extract_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfdFile = new Microsoft.Win32.SaveFileDialog();

            string ExtractedText = string.Empty;

            if (select_image == false)
            {
                System.Windows.Forms.MessageBox.Show("이미지를 선택해주세요.", "이미지 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SteganoImage1Extract_password.Password.Length != 8)
            {
                System.Windows.Forms.MessageBox.Show("이미지를 선택해주세요.", "비밀번호 입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SteganoImage1Extract_password.Focus();
                return;
            }

            BitmapImage temp_image = new BitmapImage();

            temp_image = SteganoImage1Extract_image.Source as BitmapImage;

            ImgBmp = BitmapImage2Bitmap(temp_image);
            ExtractedText = stegano.ExtractText(ImgBmp);

            try
            {
                ExtractedText = Crypto.DecryptStringAES(ExtractedText, SteganoImage1Extract_password.Password);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("비밀번호가 틀렸습니다. \r\n 다시확인해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SteganoImage1Extract_password.Focus();
                return;
            }

            SteganoImage1Extract_Text.Text = ExtractedText;
        }

        private System.Drawing.Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }
    }
}
