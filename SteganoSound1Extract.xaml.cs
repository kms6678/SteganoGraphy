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
using System.Drawing.Text; 

namespace WpfSteganography
{
    /// <summary>
    /// SteganoSound1Extract.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoSound1Extract : Window
    {
        string path = string.Empty;
        public SoundPlayer player = null;
        SteganographyConvert stegano = new SteganographyConvert();

        public SteganoSound1Extract()
        {
            InitializeComponent();
        }

        private void SteganoSound1Extract_path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openDialog.ShowDialog();

            if (!openDialog.FileName.Equals(string.Empty))
            {
                // Create image element to set as icon on the menu element
                path = openDialog.FileName;
                SteganoSound1Extract_path.Content = path;
            }
        }

        private void SteganoSound1Extract_play_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
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

        private void SteganoSound1Extract_stop_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (player != null)
            {
                player.Stop();
                return;
            }
            else
                return;
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string extract_text = string.Empty;

            if (path.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("대상 음악파일을 선택해주세요.", "음악파일 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SteganoSound1Extract_password.Password.Equals(string.Empty) && SteganoSound1Extract_password.Password.Length != 8)
            {
                System.Windows.Forms.MessageBox.Show("비밀번호를 8자리로 맞추어주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] temp = path.Split('\\');

            Wave base_wave = new Wave(new FileStream(temp.Last(), FileMode.Open));

            SteganoSound1Extract_text.Text = string.Empty; //텍스트박스 초기화

            //텍스트 추출 시작

            extract_text = base_wave.Extracted_LSB_bit(base_wave.sampleData);

            try
            {
                extract_text = Crypto.DecryptStringAES(extract_text, SteganoSound1Extract_password.Password);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("비밀번호가 틀렸습니다. \r\n 다시확인해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SteganoSound1Extract_password.Focus();
                return;
            }
            //텍스트 추출 완료

            SteganoSound1Extract_text.Text = extract_text;

            //추출된 텍스트 표시
        }

        private void SteganoSound1Extract_exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
