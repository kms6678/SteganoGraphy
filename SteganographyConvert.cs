using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace steganography
{
    class SteganographyConvert
    {
        enum State
        {
            hiding,
            filling_with_zeros
        };

        public Bitmap embedText(string HiddenText, Bitmap Imgbmp)
        {
            State s = State.hiding;

            Imgbmp.SetPixel(Imgbmp.Width - 1, Imgbmp.Height - 1, Color.FromArgb(0, 0, 0));

            int charIndex = 0;
            int charValue = 0;
            long colorUnitIndex = 0;
            int zeros = 0;

            string binaryValue = string.Empty;
            int b_count = 7;

            int R = 0, G = 0, B = 0;

            int RunStatus = 0;

            Application.DoEvents();

            for (int i = 0; i < Imgbmp.Height; i++)
            {
                for (int j = 0; j < Imgbmp.Width; j++)
                {
                    Color pixel = Imgbmp.GetPixel(i, j);

                    pixel = Color.FromArgb(pixel.R - pixel.R % 2, pixel.G - pixel.G % 2, pixel.B - pixel.B % 2); //데이터 믹싱과정에서 255값을 초과할수 있으므로 홀수값들을 모두 짝수값으로 만드는 과정.

                    R = pixel.R;
                    G = pixel.G;
                    B = pixel.B;

                    for (int n = 0; n < 3; n++)
                    {
                        ++RunStatus;
                        if (colorUnitIndex % 8 == 0)
                        {
                            if (zeros == 8)//마지막 바이트 끝에꺼 처리하는 구문
                            {
                                if (colorUnitIndex % 3 != 0)
                                {
                                    Imgbmp.SetPixel(i, j, Color.FromArgb(R, G, B));
                                }
                                return Imgbmp;
                            }

                            if (charIndex >= HiddenText.Length)//숨길데이터를 다 숨겻으면 hiding 상태를 해제시킴
                            {
                                s = State.filling_with_zeros;
                            }
                            else
                            {
                                charValue = HiddenText[charIndex++]; //8비트씩 한문자를 읽어오기 위해서 colorUnitIndex%8 == 0를 조건문으로 넣음.
                                binaryValue = toBinary(charValue); //binaryValue에 이진수로 string 형식에 저장시킨다.
                            }
                        }

                        int case_RGB = (int)colorUnitIndex % 3;

                        switch (case_RGB)
                        {
                            case 0:
                                {
                                    if (s == State.hiding)
                                        R = R + int.Parse(binaryValue.ElementAt(b_count).ToString());
                                    else
                                        zeros++;
                                }
                                break;
                            case 1:
                                {
                                    if (s == State.hiding)
                                        G = G + int.Parse(binaryValue.ElementAt(b_count).ToString());
                                    else
                                        zeros++;
                                }
                                break;
                            case 2:
                                {
                                    if (s == State.hiding)
                                        B = B + int.Parse(binaryValue.ElementAt(b_count).ToString());
                                    else
                                        zeros++;

                                    Imgbmp.SetPixel(i, j, Color.FromArgb(R, G, B)); //R,G,B 한픽셀에 데이터 수정이 끝낫으므로 픽셀의 값을 변경시킴.
                                }
                                break;
                        }

                        b_count = b_count - 1;

                        if (b_count == -1)
                            b_count = 7;

                        colorUnitIndex++;
                    }
                }
            }
            Application.DoEvents();

            return Imgbmp;
        }

        public string ExtractText(Bitmap Imgbmp)//텍스트 추출하는 함수
        {
            int colorUnitIndex = 0;
            int tempValue = 0;
            int charValue = 0;

            string ExtractedText = String.Empty;

            int RunStatus = 0;

            Application.DoEvents(); //버퍼에 쌓여있는 처리 내용을 강제로 뿌려주어서 윈도우 폼의 빠른 업데이트를 하게하는 메서드

            for (int i = 0; i < Imgbmp.Height; i++)
            {
                for (int j = 0; j < Imgbmp.Width; j++)
                {
                    Color pixel = Imgbmp.GetPixel(i, j);

                    for (int n = 0; n < 3; n++)
                    {
                        ++RunStatus;
                        switch (colorUnitIndex % 3) //데이터 숨기는 과정에서 R,G,B값들을 모두 짝수로 만들엇는데 %2로 나누어서 나머지가 1이나온다는 말은 데이터에 숨겨진 텍스트 정보가 있다는 의미이다.
                        {
                            case 0:
                                tempValue = pixel.R % 2;
                                writeBit(ref charValue, tempValue);
                                break;
                            case 1:
                                tempValue = pixel.G % 2;
                                writeBit(ref charValue, tempValue);
                                break;
                            case 2:
                                tempValue = pixel.B % 2;
                                writeBit(ref charValue, tempValue);
                                break;
                        }

                        colorUnitIndex++;


                        if (colorUnitIndex % 8 == 0)
                        {
                            if (charValue == 0)
                            {
                                return ExtractedText;
                            }

                            char c = (char)charValue;

                            ExtractedText = ExtractedText + c.ToString();

                            charValue = 0;
                            location = 0;
                        }
                    }
                }
            }
            Application.DoEvents();
            return ExtractedText;
        }

        int location = 0;

        public void writeBit(ref int buffer, int value) //비트연산
        {
            if (value == 1)
                buffer = buffer | (1 << location++);
            else
                location++;
        }

        public string toBinary(int temp)
        {
            string result = string.Empty;

            while (temp != 0)
            {
                result = (temp % 2) + result;
                temp = temp / 2;
            }

            while (result.Length != 8)
            {
                result = "0" + result;
            }

            return result;
        }
    }
}
