using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ScottPlot
{
    public class Tools
    {
        static Random rand = new Random();

        public static Color GetRandomColor()
        {
            Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            return randomColor;
        }

        public static Brush GetRandomBrush()
        {
            return new SolidBrush(GetRandomColor());
        }

        public static string BitmapHash(Bitmap bmp)
        {
            byte[] bmpBytes = BitmapToBytes(bmp);
            var md5 = System.Security.Cryptography.MD5.Create();
            StringBuilder hashString = new StringBuilder();
            byte[] hashBytes = md5.ComputeHash(bmpBytes);
            for (int i = 0; i < hashBytes.Length; i++)
                hashString.Append(hashBytes[i].ToString("X2"));
            return hashString.ToString();
        }

        public static Bitmap BitmapFromBytes(byte[] bytes, Size size, PixelFormat format = PixelFormat.Format8bppIndexed)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height, format);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public static byte[] BitmapToBytes(Bitmap bmp)
        {
            int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            byte[] bytes = new byte[bmp.Width * bmp.Height * bytesPerPixel];
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            //byte[] bytes = new byte[bmpData.Stride * bmp.Height * bytesPerPixel];
            Marshal.Copy(bmpData.Scan0, bytes, 0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bytes;
        }

        public static string VerifyFont(string fontName)
        {
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                if (fontName.ToUpper() == font.Name.ToUpper())
                    return font.Name;
            }
            throw new Exception($"Font not found: {fontName}");
        }

        public static string ScientificNotation(double value, int decimalPlaces = 2, bool preceedWithPlus = true)
        {
            string output;

            if ((Math.Abs(value) > .0001) && (Math.Abs(value) < 10000))
            {
                value = Math.Round(value, decimalPlaces);
                output = value.ToString();
            } else
            {
                int exponent = (int)Math.Log10(value);
                double multiplier = Math.Pow(10, exponent);
                double mantissa = value / multiplier;
                mantissa = Math.Round(mantissa, decimalPlaces);
                output = $"{mantissa}e{exponent}";
            }

            if (preceedWithPlus && !output.StartsWith("-"))
                output = "+" + output;

            return output;
        }
    }
}
