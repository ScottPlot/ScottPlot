using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

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

        public static string GetVersionString(bool justThreeDigits = true)
        {
            Version ver = typeof(Plot).Assembly.GetName().Version;
            if (justThreeDigits)
                return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            else
                return ver.ToString();
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

        [Obsolete("use ScottPlot.Config.Fonts.GetValidFontName()", error: true)]
        public static string VerifyFont(string fontName)
        {
            return null;
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

        public static double[] DateTimesToDoubles(DateTime[] dateTimeArray)
        {
            double[] positions = new double[dateTimeArray.Length];
            for (int i = 0; i < positions.Length; i++)
                positions[i] = dateTimeArray[i].ToOADate();
            return positions;
        }

        [Obsolete]
        public static Bitmap DesignerModeBitmap(Size size, bool drawArrows = false)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);

            Graphics gfx = Graphics.FromImage(bmp);
            gfx.Clear(ColorTranslator.FromHtml("#003366"));
            Brush brushLogo = new SolidBrush(ColorTranslator.FromHtml("#FFFFFF"));
            Brush brushMeasurements = new SolidBrush(ColorTranslator.FromHtml("#006699"));
            Pen pen = new Pen(ColorTranslator.FromHtml("#006699"), 3);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            float arrowSize = 7;
            float padding = 3;

            // logo
            FontFamily ff = new FontFamily(Config.Fonts.GetDefaultFontName());
            gfx.DrawString("ScottPlot", new Font(ff, 24, FontStyle.Bold), brushLogo, 10, 10);
            var titleSize = gfx.MeasureString("ScottPlot", new Font(ff, 24, FontStyle.Bold));
            gfx.DrawString($"version {GetVersionString()}", new Font(ff, 12, FontStyle.Italic), brushLogo, 12, (int)(10 + titleSize.Height * .7));

            if (drawArrows)
            {
                // horizontal arrow
                PointF left = new PointF(padding, size.Height / 2);
                PointF leftA = new PointF(left.X + arrowSize, left.Y + arrowSize);
                PointF leftB = new PointF(left.X + arrowSize, left.Y - arrowSize);
                PointF right = new PointF(size.Width - padding, size.Height / 2);
                PointF rightA = new PointF(right.X - arrowSize, right.Y + arrowSize);
                PointF rightB = new PointF(right.X - arrowSize, right.Y - arrowSize);
                gfx.DrawLine(pen, left, right);
                gfx.DrawLine(pen, left, leftA);
                gfx.DrawLine(pen, left, leftB);
                gfx.DrawLine(pen, right, rightA);
                gfx.DrawLine(pen, right, rightB);
                gfx.DrawString($"{size.Width}px",
                    new Font(ff, 12, FontStyle.Bold), brushMeasurements,
                    (float)(size.Width * .2), (float)(size.Height * .5));

                // vertical arrow
                PointF top = new PointF(size.Width / 2, padding);
                PointF topA = new PointF(top.X - arrowSize, top.Y + arrowSize);
                PointF topB = new PointF(top.X + arrowSize, top.Y + arrowSize);
                PointF bot = new PointF(size.Width / 2, size.Height - padding);
                PointF botA = new PointF(bot.X - arrowSize, bot.Y - arrowSize);
                PointF botB = new PointF(bot.X + arrowSize, bot.Y - arrowSize);
                gfx.DrawLine(pen, top, bot);
                gfx.DrawLine(pen, bot, botA);
                gfx.DrawLine(pen, bot, botB);
                gfx.DrawLine(pen, top, topA);
                gfx.DrawLine(pen, top, topB);
                gfx.RotateTransform(-90);
                gfx.DrawString($"{size.Height}px",
                    new Font(ff, 12, FontStyle.Bold), brushMeasurements,
                    (float)(-size.Height * .4), (float)(size.Width * .5));
            }

            return bmp;
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

        public static double[] Log10(double[] dataIn)
        {
            double[] dataOut = new double[dataIn.Length];
            for (int i = 0; i < dataOut.Length; i++)
                dataOut[i] = Math.Log10(dataIn[i]);
            return dataOut;
        }

        public static void LaunchBrowser(string url)
        {
            // A cross-platform .NET-Core-safe function to launch a URL in the browser
            Debug.WriteLine($"Launching URL: {url}");
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    Process.Start("xdg-open", url);
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    Process.Start("open", url);
                else
                    throw;
            }
        }
    }
}
