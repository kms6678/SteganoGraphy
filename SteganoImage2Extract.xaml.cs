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
    /// SteganoImage2Extract.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoImage2Extract : Window
    {
        System.Drawing.Bitmap image = null;
        SteganographyConvert stegano = new SteganographyConvert();

        public bool select_original = false;
        public bool select_extract = false;
        public SteganoImage2Extract()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void SteganoImage2Extract_original_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

                select_original = true;
                SteganoImage2Extract_original.Source = bmImage;
            }
        }

        private void SteganoImage2Extract_extract_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

                select_extract = true;
                SteganoImage2Extract_extract.Source = bmImage;
            }
        }

        private void SteganoImage2Extract_extractbutton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (select_original == false)
            {
                System.Windows.Forms.MessageBox.Show("원본 이미지를 선택해주세요.", "원본이미지 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                BitmapImage temp_image = new BitmapImage();
                temp_image = SteganoImage2Extract_original.Source as BitmapImage;
                image = BitmapImage2Bitmap(temp_image);
            }

            if(SteganoImage2Extract_password.Password.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("비밀번호 8자리를 입력해주세요.", "비밀번호 입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string result = string.Empty;

            string ExtractedText = stegano.ExtractText(image);

            try
            {
                result = Crypto.DecryptStringAES(ExtractedText, SteganoImage2Extract_password.Password);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("비밀번호가 틀렸습니다. \r\n 다시확인해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SteganoImage2Extract_password.Focus();
                return;
            }

            Stream fout = new FileStream("결과.pgm", FileMode.Create);
            BinaryWriter file_out = new BinaryWriter(fout);

            for (int i = 1; i < result.Length; i++)
            {
                file_out.Write(result.ElementAt(i));
            }

            file_out.Close();
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
