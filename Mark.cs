using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;

namespace steganography
{
    class Mark
    {
        public static string Pre_ImagePath = null;
        public static string Mark_ImageText = null;
        public static float MarkOpacity = 100;
        public static Image ImageSize = null;

        public static Bitmap NewImage()
        {
            Image originImage = Image.FromFile(Pre_ImagePath);
            ImageSize = originImage;

            Bitmap tempImage = new Bitmap(originImage.Width, originImage.Height);

            tempImage.SetResolution(72, 72);

            Graphics grPhoto = Graphics.FromImage(tempImage);

            grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grPhoto.DrawImage(originImage, new Rectangle(0, 0, originImage.Width, originImage.Height), 0, 0, originImage.Width, originImage.Height, GraphicsUnit.Pixel);

            int[] sizes = new int[] { 50, 48, 46, 44, 42, 40, 38 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < sizes.Length; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                crSize = grPhoto.MeasureString(Mark_ImageText, crFont);

                if ((ushort)crSize.Width < (ushort)originImage.Width)
                    break;
            }

            float setOpacity = (MarkOpacity / 100);

            int yPixelsFromBottom = (int)(originImage.Height * .5);
            float yPosFromBottom = ((originImage.Height - yPixelsFromBottom) - (crSize.Height / 2));
            float xCenterOfImg = originImage.Width / 2;
            //워터마크가 찍힐 좌표를 정하는 부분

            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;

            SolidBrush semitransBrush = new SolidBrush(System.Drawing.Color.FromArgb((int)(255 * setOpacity), 0, 0, 0));


            for (int i = 80; i < originImage.Width; i = i + (int)crSize.Width)
            {
                for (int j = 0; j < originImage.Height; j = j + (int)crSize.Height)
                {
                    grPhoto.DrawString(Mark_ImageText, crFont, semitransBrush, new PointF(i, j), strFormat);
                }
            }//워터마크 반복적으로 찍음

            return tempImage;
        }
    }
}