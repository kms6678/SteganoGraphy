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
    /// SteganoImage2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoImage2 : Window
    {
        public bool select_original = false;
        public bool select_insert = false;
        System.Drawing.Bitmap image = null;
        SteganographyConvert stegano = new SteganographyConvert();

        string path = string.Empty;

        public SteganoImage2()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void SteganoImage2_original_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                SteganoImage2_original.Source = bmImage;
            }
        }

        private void SteganoImage2_insert_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openDialog.ShowDialog();

            if (!openDialog.FileName.Equals(string.Empty))
            {
                // Create image element to set as icon on the menu element
                /*BitmapImage bmImage = new BitmapImage();
                bmImage.BeginInit();
                bmImage.UriSource = new Uri(openDialog.FileName, UriKind.Absolute);
                bmImage.EndInit();*/

                path = openDialog.FileName;
                select_insert = true;
                //SteganoImage2_insert.Source = bmImage;
            }
        }

        private void SteganoImage2_crypto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] pgm_Value = { };
            string result = string.Empty;
            string Hidden_image = string.Empty;

            if (select_original == false)
            {
                System.Windows.Forms.MessageBox.Show("원본 이미지를 선택해주세요.", "원본이미지 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (select_insert == false)
            {
                System.Windows.Forms.MessageBox.Show("삽입할 이미지를 선택해주세요.", "삽입이미지 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SteganoImage2_password.Password.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("비밀번호 8자리를 입력해주세요.", "비밀번호 입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BitmapImage temp_image = new BitmapImage();

            temp_image = SteganoImage2_original.Source as BitmapImage;

            image = BitmapImage2Bitmap(temp_image);

            pgm_Value = File.ReadAllLines(path);

            string[] temp = pgm_Value[2].Split(' '); // 파일형식, width, height, 밝기를 지난 그다음줄부터 실제 이미지 크기

            int image_raw = temp.Length; //열 길이
            int image_column = pgm_Value.Length; //행 길이

            for (int i = 0; i < image_column; i++)
            {
                temp = pgm_Value[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                {
                    result = result + ' ' + temp[j];
                }
            }

            Hidden_image = Crypto.EncryptStringAES(result, SteganoImage2_password.Password);

            image = stegano.embedText(Hidden_image, image);

            Microsoft.Win32.SaveFileDialog sfdFile = new Microsoft.Win32.SaveFileDialog();

            sfdFile.Filter = "Png Image|*.png|Bitmap Image|*.bmp";

            if (sfdFile.ShowDialog() == true)
            {
                image.Save(sfdFile.FileName);
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
