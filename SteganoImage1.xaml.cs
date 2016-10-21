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
    /// SteganoImage1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoImage1 : Window
    {
        public bool select_image = false;

        System.Drawing.Bitmap ImgBmp = null;
        SteganographyConvert stegano = new SteganographyConvert();

        public SteganoImage1()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void SteganoImage1_image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                SteganoImage1_image.Source = bmImage;
            }
        }

        private void SteganoImage1_crypto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfdFile = new Microsoft.Win32.SaveFileDialog();
            sfdFile.Filter = "Png Image|*.png|Bitmap Image|*.bmp";

            string Hiddentext = string.Empty;

            if (select_image == false)
            {
                System.Windows.Forms.MessageBox.Show("이미지를 선택해주세요.", "이미지 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(SteganoImage1_password.Password.Length != 8)
            {
                System.Windows.Forms.MessageBox.Show("비밀번호 8자리를 입력해주세요.", "비밀번호 입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SteganoImage1_password.Focus();
                return;
            }

            if (SteganoImage1_Text.Text.Equals("텍스트를 입력해 주세요."))
            {
                System.Windows.Forms.MessageBox.Show("숨기길 원하는 문자를 입력해주세요.", "텍스트 입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Hiddentext = SteganoImage1_Text.Text;
                Hiddentext = Crypto.EncryptStringAES(SteganoImage1_Text.Text, SteganoImage1_password.Password);
            }

            BitmapImage temp_image = new BitmapImage();

            temp_image = SteganoImage1_image.Source as BitmapImage;

            ImgBmp = BitmapImage2Bitmap(temp_image);
            ImgBmp = stegano.embedText(Hiddentext, ImgBmp);

            if (sfdFile.ShowDialog() == true)
            {
                switch (sfdFile.FilterIndex)
                {
                    case 0:
                        {
                            ImgBmp.Save(sfdFile.FileName, ImageFormat.Png);
                        }
                        break;
                    case 1:
                        {
                            ImgBmp.Save(sfdFile.FileName, ImageFormat.Bmp);
                        }
                        break;
                }
            }
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
