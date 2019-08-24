using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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

        public static string GetVersionString()
        {
            Version scottPlotVersion = typeof(ScottPlot.Plot).Assembly.GetName().Version;
            return scottPlotVersion.ToString();
        }

        public static string GetFrameworkVersionString()
        {
            return $".NET {Environment.Version.ToString()}";
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
            }
            else
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

        public static void DesignerModeDemoPlot(ScottPlot.Plot plt)
        {
            int pointCount = 101;
            double pointSpacing = .01;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount, pointSpacing);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto(0);
            plt.Title("ScottPlot User Control");
            plt.YLabel("Sample Data");
        }

        public static BitmapImage bmpImageFromBmp(System.Drawing.Bitmap bmp)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            ((System.Drawing.Bitmap)bmp).Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            return bmpImage;
        }

        public static void SaveImageDialog(Plot plt)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "ScottPlot.png";
            savefile.Filter = "PNG Files (*.png)|*.png;*.png";
            savefile.Filter += "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            savefile.Filter += "|BMP Files (*.bmp)|*.bmp;*.bmp";
            savefile.Filter += "|TIF files (*.tif, *.tiff)|*.tif;*.tiff";
            savefile.Filter += "|All files (*.*)|*.*";
            if (savefile.ShowDialog() == DialogResult.OK)
                plt.SaveFig(savefile.FileName);
        }

        private static double[] DoubleArray<T>(T[] dataIn)
        {
            double[] dataOut = new double[dataIn.Length];
            for (int i = 0; i < dataIn.Length; i++)
                dataOut[i] = Convert.ToDouble(dataIn[i]);
            return dataOut;
        }

        public static double[] DoubleArray(byte[] dataIn)
        {
            return DoubleArray<byte>(dataIn);
        }

        public static double[] DoubleArray(int[] dataIn)
        {
            return DoubleArray<int>(dataIn);
        }

        public static double[] DoubleArray(float[] dataIn)
        {
            return DoubleArray<float>(dataIn);
        }

        public static void ApplyBaselineSubtraction(double[] data, int index1, int index2)
        {
            double baselineSum = 0;
            for (int i = index1; i < index2; i++)
                baselineSum += data[i];
            double baselineAverage = baselineSum / (index2 - index1);
            for (int i = 0; i < data.Length; i++)
                data[i] -= baselineAverage;
        }
    }
}
