using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public static class Tools
    {
        public static Color GetRandomColor(Random rand = null)
        {
            if (rand is null)
                rand = new Random();
            Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            return randomColor;
        }

        public static Brush GetRandomBrush()
        {
            return new SolidBrush(GetRandomColor());
        }

        public static Color Blend(this Color colorA, Color colorB, double fractionA)
        {
            fractionA = Math.Max(fractionA, 0);
            fractionA = Math.Min(fractionA, 1);
            byte r = (byte)((colorA.R * fractionA) + colorB.R * (1 - fractionA));
            byte g = (byte)((colorA.G * fractionA) + colorB.G * (1 - fractionA));
            byte b = (byte)((colorA.B * fractionA) + colorB.B * (1 - fractionA));
            return Color.FromArgb(r, g, b);
        }

        [Obsolete("use ScottPlot.Plot.Version", true)]
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

            plt.AddScatter(dataXs, dataSin);
            plt.AddScatter(dataXs, dataCos);
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

        private static double[] DoubleArray<T>(T[] dataIn)
        {
            double[] dataOut = new double[dataIn.Length];
            for (int i = 0; i < dataIn.Length; i++)
                dataOut[i] = NumericConversion.GenericToDouble(ref dataIn[i]);
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
                dataOut[i] = dataIn[i] > 0 ? Math.Log10(dataIn[i]) : 0;
            return dataOut;
        }

        public static (double[] xs, double[] ys) ConvertPolarCoordinates(double[] rs, double[] thetas)
        {
            double[] xs = new double[rs.Length];
            double[] ys = new double[rs.Length];

            for (int i = 0; i < rs.Length; i++)
            {
                double x = rs[i];
                double y = thetas[i];

                xs[i] = x * Math.Cos(y);
                ys[i] = x * Math.Sin(y);
            }

            return (xs, ys);
        }

        public static void LaunchBrowser(string url = "https://ScottPlot.NET")
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

        public static double[] Round(double[] data, int decimals = 2)
        {
            double[] rounded = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
                rounded[i] = Math.Round(data[i], decimals);
            return rounded;
        }

        /// <summary>
        /// return a copy of the given array padded with the given value at both sidees
        /// </summary>
        public static double[] Pad(double[] values, int padCount = 1, double padWithLeft = 0, double padWithRight = 0, bool cloneEdges = false)
        {
            double[] padded = new double[values.Length + padCount * 2];

            Array.Copy(values, 0, padded, padCount, values.Length);

            if (cloneEdges)
            {
                padWithLeft = values[0];
                padWithRight = values[values.Length - 1];
            }

            for (int i = 0; i < padCount; i++)
            {
                padded[i] = padWithLeft;
                padded[padded.Length - 1 - i] = padWithRight;
            }

            return padded;
        }

        public static string GetOsName(bool details = true)
        {
            string name = "Unknown";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                name = "Linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                name = "MacOS";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                name = "Windows";

            if (details)
                name += $" ({System.Environment.OSVersion})";

            return name;
        }

        public static int SimpleHash(double[] input)
        {
            byte[] bytes = input.SelectMany(n => { return BitConverter.GetBytes(n); }).ToArray();
            int hash = 0;
            foreach (byte b in bytes)
                hash = (hash * 31) ^ b;
            return hash;
        }

        public static double[,] XYToIntensitiesGaussian(int[] xs, int[] ys, int width, int height, int sigma)
        {
            static double NormPDF(double x, double mu, double sigma) =>
                (1 / (sigma * Math.Sqrt(2 * Math.PI))) * Math.Exp(-0.5 * (x - mu / sigma) * (x - mu / sigma));

            double[,] output = new double[height, width];
            double[,] intermediate = new double[height, width]; // Each cell has the number of hits. This is the array before any blurring

            int radius = 2; // 2 Standard deviations is ~0.95, i.e. close enough
            for (int i = 0; i < xs.Length; i++)
            {
                if (xs[i] >= 0 && xs[i] < width && ys[i] >= 0 && ys[i] < height)
                {
                    intermediate[ys[i], xs[i]] += 1;
                }
            }

            double[] kernel = new double[2 * radius * sigma + 1];
            for (int i = 0; i < kernel.Length; i++)
            {
                kernel[i] = NormPDF(i - kernel.Length / 2, 0, sigma);
            }

            for (int i = 0; i < height; i++) // Blurs up and down, i.e. a vertical kernel. Gaussian Blurs are special in that it can be decomposed this way, saving time
            {
                for (int j = 0; j < width; j++)
                {
                    double sum = 0;
                    double kernelSum = 0; // The kernelSum can be precomputed, but this gives incorrect output at the edges of the image
                    for (int k = -radius * sigma; k <= radius * sigma; k++)
                    {
                        if (i + k >= 0 && i + k < height)
                        {
                            sum += intermediate[i + k, j] * kernel[k + kernel.Length / 2];
                            kernelSum += kernel[k + kernel.Length / 2];
                        }
                    }

                    output[i, j] = sum / kernelSum;
                }
            }

            for (int i = 0; i < height; i++) // Blurs left and right, i.e. a horizontal kernel
            {
                for (int j = 0; j < width; j++)
                {
                    double sum = 0;
                    double kernelSum = 0;
                    for (int k = -radius * sigma; k <= radius * sigma; k++)
                    {
                        if (j + k >= 0 && j + k < width)
                        {
                            sum += output[i, j + k] * kernel[k + kernel.Length / 2];
                            kernelSum += kernel[k + kernel.Length / 2];
                        }
                    }

                    output[i, j] = sum / kernelSum;
                }
            }

            return output;
        }

        public static double[,] XYToIntensitiesDensity(int[] xs, int[] ys, int width, int height, int sampleWidth)
        {
            double[,] output = new double[height, width];
            (int x, int y)[] points = xs.Zip(ys, (x, y) => (x, y)).ToArray();
            points = points.OrderBy(p => p.x).ToArray();
            int[] xs_sorted = points.Select(p => p.x).ToArray();

            for (int i = 0; i < height - height % sampleWidth; i += sampleWidth)
            {
                for (int j = 0; j < width - width % sampleWidth; j += sampleWidth)
                {
                    int count = 0;
                    for (int k = 0; k < sampleWidth; k++)
                    {
                        for (int l = 0; l < sampleWidth; l++)
                        {
                            int index = Array.BinarySearch(xs_sorted, j + l);
                            if (index > 0)
                            {
                                for (int m = index; m < xs.Length; m++)
                                { //Multiple points w/ same x value
                                    if (points[m].x == j + l && points[m].y == i + k)
                                    {
                                        count++; // Increments number of hits in sampleWidth sized square
                                    }
                                }
                            }
                        }
                    }

                    for (int k = 0; k < sampleWidth; k++)
                    {
                        for (int l = 0; l < sampleWidth; l++)
                        {
                            output[i + k, j + l] = count;
                        }
                    }
                }
            }

            return output;
        }

        public static double[,] XYToIntensities(IntensityMode mode, int[] xs, int[] ys, int width, int height, int sampleWidth)
        {
            return mode switch
            {
                IntensityMode.Gaussian => XYToIntensitiesGaussian(xs, ys, width, height, sampleWidth),
                IntensityMode.Density => XYToIntensitiesDensity(xs, ys, width, height, sampleWidth),
                _ => throw new NotImplementedException($"{nameof(mode)} is not a supported {nameof(IntensityMode)}"),
            };
        }

        public static string ToDifferentBase(double number, int radix = 16, int decimalPlaces = 3, int padInteger = 0, bool dropTrailingZeroes = true, char decimalSymbol = '.')
        {
            if (number < 0)
            {
                return "-" + ToDifferentBase(Math.Abs(number), radix, decimalPlaces, padInteger, dropTrailingZeroes, decimalSymbol);
            }
            else if (number == 0)
            {
                return "0";
            }

            char[] symbols = "0123456789ABCDEF".ToCharArray();
            if (radix > symbols.Length || radix <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(radix));
            }

            double epsilon = Math.Pow(radix, -decimalPlaces);

            if (radix > symbols.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(radix));
            }

            int integerLength = (int)Math.Ceiling(Math.Log(number, radix));
            int decimalLength = number % 1 > epsilon ? decimalPlaces : 0;
            double decimalPart = number % 1;
            string output = "";

            for (int i = 0; i < integerLength; i++)
            {
                if (number == radix && padInteger == 0)
                {
                    output = "10" + output;
                }
                else
                {
                    output = symbols[(int)(number % radix)] + output;
                }
                number /= radix;
            }

            while (output.Length < padInteger)
            {
                output = "0" + output;
            }

            if (decimalLength != 0)
            {
                if (output == "")
                {
                    output += "0";
                }
                output += decimalSymbol;
                output += ToDifferentBase(Math.Round(decimalPart * Math.Pow(radix, decimalPlaces)), radix, decimalPlaces, decimalPlaces, dropTrailingZeroes, decimalSymbol);
                if (dropTrailingZeroes)
                {
                    while (output.Last() == '0')
                    {
                        output = output.Substring(0, output.Length - 1);
                    }

                    if (output.Last() == decimalSymbol)
                    {
                        output = output.Substring(0, output.Length - 1);
                    }
                }
            }

            return output;
        }

        public static bool Uses24HourClock(CultureInfo culture) => culture.DateTimeFormat.LongTimePattern.Contains("H");
    }
}
