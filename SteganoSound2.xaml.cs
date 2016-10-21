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

using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Drawing.Imaging;
using steganography;

namespace WpfSteganography
{
    /// <summary>
    /// SteganoSound2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoSound2 : Window
    {
        string path = string.Empty;
        string path_image = string.Empty;
        public SoundPlayer player = null;
        SteganographyConvert stegano = new SteganographyConvert();

        public bool select_original = false;

        public SteganoSound2()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void SteganoSound2_path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openDialog.ShowDialog();

            if (!openDialog.FileName.Equals(string.Empty))
            {
                // Create image element to set as icon on the menu element
                path = openDialog.FileName;
                SteganoSound2_path.Content = path;
            }
        }

        private void SteganoSound2_play_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (!path.Equals(string.Empty))
            {
                player = new SoundPlayer(path);
                player.Play();
                return;
            }
            else
                return;
        }

        private void SteganoSound2_stop_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (player != null)
            {
                player.Stop();
                return;
            }
            else
                return;
        }

        private void SteganoSound2_image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

                path_image = openDialog.FileName;
                select_original = true;
                SteganoSound2_image.Source = bmImage;
            }
        }

        private void SteganoSound2_crypto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string HiddenText = string.Empty;
            string PrivateKey = string.Empty;

            if (path.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("대상 음악파일을 선택해주세요.", "음악파일 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (select_original == false)
            {
                System.Windows.Forms.MessageBox.Show("숨기기 원하는 이미지를 선택해주세요.", "이미지 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SteganoSound2_password.Password.Equals(string.Empty) && SteganoSound2_password.Password.Length != 8)
            {
                System.Windows.Forms.MessageBox.Show("비밀번호를 8자리로 맞추어주세요.", "비밀번호 알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] temp = path.Split('\\');

            Wave base_wave = new Wave(new FileStream(temp.Last(), FileMode.Open));

            byte[] data = base_wave.sampleData;

            byte[] image_value = { };

            image_value = File.ReadAllBytes(path_image);

            string value = string.Empty;

            /*for (int i = 0; i < image_value.Length;i++)
            {
                value = value + image_value[i];
            }*/

            PrivateKey = SteganoSound2_password.Password;

            //HiddenText = Crypto.EncryptStringAES(value, this.textBox1.Text);

            string insert_text = string.Empty;

            for (int i = 0; i < image_value.Length; i++)
            {
                insert_text = insert_text + stegano.toBinary(image_value[i]);
            }

            for (int i = 0; i < insert_text.Length; i++)
            {
                int temp_val = int.Parse(insert_text.ElementAt(i).ToString());
                base_wave.Write_LSB_bit(data, temp_val); //데이터 하이딩 핵심 함수
            }

            base_wave.Write_End(data);

            //데이터 하이딩 완료

            base_wave.Save_wavfile(data);
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
