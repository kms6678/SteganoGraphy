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
    /// SteganoSound2Extract.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoSound2Extract : Window
    {
        string path = string.Empty;
        string path_image = string.Empty;
        public SoundPlayer player = null;
        SteganographyConvert stegano = new SteganographyConvert();

        public bool select_image = false;

        public SteganoSound2Extract()
        {
            InitializeComponent();
        }

        private void SteganoSound2Extract_play_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void SteganoSound2Extract_stop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (player != null)
            {
                player.Stop();
                return;
            }
            else
                return;
        }

        private void SteganoSound2Extract_path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openDialog.ShowDialog();

            if (!openDialog.FileName.Equals(string.Empty))
            {
                // Create image element to set as icon on the menu element
                path = openDialog.FileName;
                SteganoSound2Extract_path.Content = path;
            }
        }

        private void SteganoSound2Extract_image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                select_image = true;
                SteganoSound2Extract_image.Source = bmImage;
            }
        }

        private void SteganoSound2Extract_extract_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string extract_text = string.Empty;

            if (path.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("대상 음악파일을 선택해주세요.", "음악파일 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SteganoSound2Extract_password.Password.Equals(string.Empty) && SteganoSound2Extract_password.Password.Length != 8)
            {
                System.Windows.Forms.MessageBox.Show("비밀번호를 8자리로 맞추어주세요.", "비밀번호 알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] temp = path.Split('\\');

            Wave base_wave = new Wave(new FileStream(temp.Last(), FileMode.Open));

            extract_text = base_wave.Extracted_LSB_bit(base_wave.sampleData);

            /*try
            {
                
                extract_text = Crypto.DecryptStringAES(extract_text, this.textBox1.Text);
            }
            catch
            {
                MessageBox.Show("비밀번호가 틀렸습니다. \r\n 다시확인해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.textBox1.Focus();
                return;
            }*/

            //StreamWriter을 사용해서 Write를 하려고했으나 StreamWriter의 Write함수는 UTF-8으로 쓰기를 진행하는데, UTF-8형식은 숫자와 문자가 다른 데이터형으로 간주되어 처리되서 문제가 발생해서... 저런식으로 코드작성

            byte[] final_value = new byte[extract_text.Length];

            for (int i = 0; i < final_value.Length; i++)
            {
                final_value[i] = (byte)extract_text.ElementAt(i);
            }

            Microsoft.Win32.SaveFileDialog sfdDialog = new Microsoft.Win32.SaveFileDialog();

            if(sfdDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(sfdDialog.FileName, final_value);
            }
        }

        private void SteganoSound2Extract_exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
