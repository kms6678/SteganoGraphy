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
    /// SteganoSound1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SteganoSound1 : Window
    {
        string path = string.Empty;
        public SoundPlayer player = null;
        SteganographyConvert stegano = new SteganographyConvert();

        public SteganoSound1()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void SteganoSound1_path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openDialog.ShowDialog();

            if (!openDialog.FileName.Equals(string.Empty))
            {
                // Create image element to set as icon on the menu element
                path = openDialog.FileName;
                string[] temp_path = path.Split('\\');
                SteganoSound1_path.Content = temp_path.Last();
            }
        }

        private void SteganoSound1_play_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
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

        private void SteganoSound1_stop_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (player != null)
            {
                player.Stop();
                return;
            }
            else
                return;
        }

        private void SteganoSound1_crypto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string HiddenText = string.Empty;
            string PrivateKey = string.Empty;

            if (path.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("대상 음악파일을 선택해주세요.", "음악파일 선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SteganoSound1_text.Text.Equals(string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("숨기기 원하는 텍스트를 입력해주세요.", "텍스트 입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SteganoSound1_password.Password.Equals(string.Empty) && SteganoSound1_password.Password.Length != 8)
            {
                System.Windows.Forms.MessageBox.Show("비밀번호를 8자리로 맞추어주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] temp = path.Split('\\');

            Wave base_wave = new Wave(new FileStream(temp.Last(), FileMode.Open));

            byte[] data = base_wave.sampleData;

            //데이터를 수정하기위해서 음원데이터를 바이트배열로 읽어들임. byte[] data안으로

            HiddenText = SteganoSound1_text.Text;
            PrivateKey = SteganoSound1_password.Password;

            HiddenText = Crypto.EncryptStringAES(SteganoSound1_text.Text, SteganoSound1_password.Password);

            string insert_text = string.Empty;

            for (int i = 0; i < HiddenText.Length; i++)
            {
                insert_text = insert_text + stegano.toBinary(HiddenText.ElementAt(i));
            }

            for (int i = 0; i < insert_text.Length; i++)
            {
                int value = int.Parse(insert_text.ElementAt(i).ToString());
                base_wave.Write_LSB_bit(data, value); //데이터 하이딩 핵심 함수
            }

            base_wave.Write_End(data);

            //데이터 하이딩 완료

            base_wave.Save_wavfile(data);
        }
    }
}
