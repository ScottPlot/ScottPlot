using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlot.Drawing
{
    public class Colormap
    {
        public static Colormap Algae => new Colormap(new Colormaps.Algae());
        public static Colormap Amp => new Colormap(new Colormaps.Amp());
        public static Colormap Balance => new Colormap(new Colormaps.Balance());
        public static Colormap Blues => new Colormap(new Colormaps.Blues());
        public static Colormap Curl => new Colormap(new Colormaps.Curl());
        public static Colormap Deep => new Colormap(new Colormaps.Deep());
        public static Colormap Delta => new Colormap(new Colormaps.Delta());
        public static Colormap Dense => new Colormap(new Colormaps.Dense());
        public static Colormap Diff => new Colormap(new Colormaps.Diff());
        public static Colormap Grayscale => new Colormap(new Colormaps.Grayscale());
        public static Colormap GrayscaleR => new Colormap(new Colormaps.GrayscaleR());
        public static Colormap Greens => new Colormap(new Colormaps.Greens());
        public static Colormap Haline => new Colormap(new Colormaps.Haline());
        public static Colormap Ice => new Colormap(new Colormaps.Ice());
        public static Colormap Inferno => new Colormap(new Colormaps.Inferno());
        public static Colormap Jet => new Colormap(new Colormaps.Jet());
        public static Colormap Magma => new Colormap(new Colormaps.Magma());
        public static Colormap Matter => new Colormap(new Colormaps.Matter());
        public static Colormap Oxy => new Colormap(new Colormaps.Oxy());
        public static Colormap Phase => new Colormap(new Colormaps.Phase());
        public static Colormap Plasma => new Colormap(new Colormaps.Plasma());
        public static Colormap Rain => new Colormap(new Colormaps.Rain());
        public static Colormap Solar => new Colormap(new Colormaps.Solar());
        public static Colormap Speed => new Colormap(new Colormaps.Speed());
        public static Colormap Tarn => new Colormap(new Colormaps.Tarn());
        public static Colormap Tempo => new Colormap(new Colormaps.Tempo());
        public static Colormap Thermal => new Colormap(new Colormaps.Thermal());
        public static Colormap Topo => new Colormap(new Colormaps.Topo());
        public static Colormap Turbid => new Colormap(new Colormaps.Turbid());
        public static Colormap Turbo => new Colormap(new Colormaps.Turbo());
        public static Colormap Viridis => new Colormap(new Colormaps.Viridis());

        private readonly IColormap ThisColormap;

        /// <summary>
        /// Name of this colormap
        /// </summary>
        public string Name => ThisColormap.Name;

        private static readonly ColormapFactory ColormapFactory = new ColormapFactory();

        public Colormap(IColormap colormap)
        {
            ThisColormap = colormap ?? ColormapFactory.GetDefaultColormap();
        }

        public Colormap(Color[] colors)
        {
            ThisColormap = new Colormaps.Palette(colors);
        }

        public Colormap(IPalette palette)
        {
            Color[] colors = palette.GetColors(palette.Count());
            ThisColormap = new Colormaps.Palette(colors);
        }

        public override string ToString() => $"Colormap {Name}";

        /// <summary>
        /// Create new instances of every colormap and return them as an array.
        /// </summary>
        /// <returns></returns>
        public static Colormap[] GetColormaps() => ColormapFactory.GetAvailableColormaps().ToArray();

        /// <summary>
        /// Return the names of all available colormaps.
        /// </summary>
        /// <returns></returns>
        public static string[] GetColormapNames() => ColormapFactory.GetAvailableNames().ToArray();

        /// <summary>
        /// Create a new colormap by its name.
        /// </summary>
        /// <param name="name">colormap name</param>
        /// <param name="throwIfNotFound">if false the default colormap (Viridis) will be returned</param>
        /// <returns></returns>
        public static Colormap GetColormapByName(string name, bool throwIfNotFound = true) =>
            throwIfNotFound ? ColormapFactory.CreateOrThrow(name) : ColormapFactory.CreateOrDefault(name);

        public (byte r, byte g, byte b) GetRGB(byte value) => ThisColormap.GetRGB(value);

        public (byte r, byte g, byte b) GetRGB(double fraction)
        {
            fraction = Math.Max(fraction, 0);
            fraction = Math.Min(fraction, 1);
            return ThisColormap.GetRGB((byte)(fraction * 255));
        }

        public int GetInt32(byte value, byte alpha = 255)
        {
            var (r, g, b) = GetRGB(value);
            return alpha << 24 | r << 16 | g << 8 | b;
        }

        public int GetInt32(double fraction, byte alpha = 255)
        {
            var (r, g, b) = GetRGB(fraction);
            return alpha << 24 | r << 16 | g << 8 | b;
        }

        public Color GetColor(byte value, double alpha = 1.0)
        {
            byte alphaByte = (byte)(255 * alpha);
            return Color.FromArgb(GetInt32(value, alphaByte));
        }

        public Color GetColor(double fraction, double alpha = 1.0)
        {
            byte alphaByte = (byte)(255 * alpha);
            return Color.FromArgb(GetInt32(fraction, alphaByte));
        }

        public Color RandomColor(Random rand, double alpha = 1.0)
        {
            byte alphaByte = (byte)(255 * alpha);
            return Color.FromArgb(GetInt32(rand.NextDouble(), alphaByte));
        }

        public void Apply(Bitmap bmp)
        {
            System.Drawing.Imaging.ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
                pal.Entries[i] = GetColor((byte)i);
            bmp.Palette = pal;
        }

        public static byte[,] IntenstitiesToRGB(double[] intensities, IColormap cmap)
        {
            byte[,] output = new byte[intensities.Length, 3];
            for (int i = 0; i < intensities.Length; i++)
            {
                double intensity = intensities[i] * 255;
                byte pixelIntensity = (byte)Math.Max(Math.Min(intensity, 255), 0);
                var (r, g, b) = cmap.GetRGB(pixelIntensity);
                output[i, 0] = r;
                output[i, 1] = g;
                output[i, 2] = b;
            }
            return output;
        }

        /// <summary>
        /// Convert intensities to colors using the given colormap and return the results as integer RGBA values.
        /// </summary>
        public static int[] GetRGBAs(double[] intensities, Colormap colorMap, double minimumIntensity = 0)
        {
            int[] rgbas = new int[intensities.Length];
            for (int i = 0; i < intensities.Length; i++)
            {
                byte pixelIntensity = (byte)Math.Max(Math.Min(intensities[i] * 255, 255), 0);
                var (r, g, b) = colorMap.GetRGB(pixelIntensity);
                byte alpha = intensities[i] < minimumIntensity ? (byte)0 : (byte)255;
                byte[] argb = { b, g, r, alpha };
                rgbas[i] = BitConverter.ToInt32(argb, 0);
            }
            return rgbas;
        }

        /// <summary>
        /// Convert intensities to colors using the given colormap and return the results as integer RGBA values.
        /// RGBA alpha value will be set according to the given array of opacities (values from 0 to 1).
        /// </summary>
        public static int[] GetRGBAs(double[] intensities, double[] opacity, Colormap colorMap)
        {
            int[] rgbas = new int[intensities.Length];
            for (int i = 0; i < intensities.Length; i++)
            {
                byte pixelIntensity = (byte)Math.Max(Math.Min(intensities[i] * 255, 255), 0);
                var (r, g, b) = colorMap.GetRGB(pixelIntensity);
                byte alpha = (byte)Math.Max(Math.Min(opacity[i] * 255, 255), 0);
                byte[] argb = { b, g, r, alpha };
                rgbas[i] = BitConverter.ToInt32(argb, 0);
            }
            return rgbas;
        }

        /// <summary>
        /// Return an array of RGBA integer values for a single color where the alpha
        /// channel is defined by an array of values from 0 to 1.
        /// </summary>
        public static int[] GetRGBAs(double[] opacity, Color color)
        {
            int[] rgbas = new int[opacity.Length];
            for (int i = 0; i < opacity.Length; i++)
            {
                byte alpha = (byte)Math.Max(Math.Min(opacity[i] * 255, 255), 0);
                byte[] argb = { color.B, color.G, color.R, alpha };
                rgbas[i] = BitConverter.ToInt32(argb, 0);
            }
            return rgbas;
        }

        /// <summary>
        /// Return an array of RGBA integer values set according to a colormap
        /// where intensities are clamped to a lower limit.
        /// </summary>
        public static int[] GetRGBAs(double?[] intensities, Colormap colorMap, double minimumIntensity = 0)
        {
            int[] rgbas = new int[intensities.Length];
            for (int i = 0; i < intensities.Length; i++)
            {
                if (intensities[i].HasValue)
                {
                    byte pixelIntensity = (byte)Math.Max(Math.Min(intensities[i].Value * 255, 255), 0);
                    var (r, g, b) = colorMap.GetRGB(pixelIntensity);
                    byte alpha = intensities[i] < minimumIntensity ? (byte)0 : (byte)255;
                    byte[] argb = { b, g, r, alpha };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
                else
                {
                    byte[] argb = { 0, 0, 0, 0 };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
            }
            return rgbas;
        }

        /// <summary>
        /// Convert intensities to colors using the given colormap and return the results as integer RGBA values.
        /// RGBA alpha value will be set according to the given array of opacities (values from 0 to 1).
        /// </summary>
        public static int[] GetRGBAs(double?[] intensities, double?[] opacity, Colormap colorMap)
        {
            int[] rgbas = new int[intensities.Length];
            for (int i = 0; i < intensities.Length; i++)
            {
                if (intensities[i].HasValue)
                {
                    byte pixelIntensity = (byte)Math.Max(Math.Min(intensities[i].Value * 255, 255), 0);
                    var (r, g, b) = colorMap.GetRGB(pixelIntensity);
                    byte alpha;
                    if (opacity[i].HasValue) alpha = (byte)Math.Max(Math.Min(opacity[i].Value * 255, 255), 0);
                    else alpha = 0;
                    byte[] argb = { b, g, r, alpha };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
                else
                {
                    byte[] argb = { 0, 0, 0, 0 };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
            }
            return rgbas;
        }

        /// <summary>
        /// Convert intensities to colors using the given colormap and return the results as integer RGBA values.
        /// RGBA alpha value will be set according to the given opacity (value from 0 to 1).
        /// </summary>
        public static int[] GetRGBAs(double?[] intensities, double? opacity, Colormap colorMap)
        {
            int[] rgbas = new int[intensities.Length];
            byte alpha;
            if (opacity.HasValue) alpha = (byte)Math.Max(Math.Min(opacity.Value * 255, 255), 0);
            else alpha = 0;

            for (int i = 0; i < intensities.Length; i++)
            {
                if (intensities[i].HasValue)
                {
                    byte pixelIntensity = (byte)Math.Max(Math.Min(intensities[i].Value * 255, 255), 0);
                    var (r, g, b) = colorMap.GetRGB(pixelIntensity);
                    byte[] argb = { b, g, r, alpha };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
                else
                {
                    byte[] argb = { 0, 0, 0, 0 };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
            }
            return rgbas;
        }

        /// <summary>
        /// Return an array of RGBA integer values for a single color where the alpha
        /// channel is defined by an array of values from 0 to 1.
        /// </summary>
        public static int[] GetRGBAs(double?[] opacity, Color color)
        {
            int[] rgbas = new int[opacity.Length];
            for (int i = 0; i < opacity.Length; i++)
            {
                if (opacity[i].HasValue)
                {
                    byte alpha;
                    if (opacity[i].HasValue) alpha = (byte)Math.Max(Math.Min(opacity[i].Value * 255, 255), 0);
                    else alpha = 0;
                    byte[] argb = { color.B, color.G, color.R, alpha };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
                else
                {
                    byte[] argb = { 0, 0, 0, 0 };
                    rgbas[i] = BitConverter.ToInt32(argb, 0);
                }
            }
            return rgbas;
        }

        /// <summary>
        /// Given an array of intensities (ranging from 0 to 1) return an array of
        /// colors according to the given colormap.
        /// </summary>
        public static Color[] GetColors(double[] intensities, Colormap colorMap)
        {
            Color[] colors = new Color[intensities.Length];
            for (int i = 0; i < intensities.Length; i++)
            {
                byte pixelIntensity = (byte)Math.Max(Math.Min(intensities[i] * 255, 255), 0);
                var (r, g, b) = colorMap.GetRGB(pixelIntensity);
                colors[i] = Color.FromArgb(255, r, g, b);
            }
            return colors;
        }

        /// <summary>
        /// Return a bitmap showing the gradient of colors in a colormap.
        /// Defining min/max will create an image containing only part of the colormap.
        /// </summary>
        public static Bitmap Colorbar(Colormap cmap, int width, int height, bool vertical = false, double min = 0, double max = 1)
        {
            if (width < 1 || height < 1)
                return null;

            if (min < 0)
                throw new ArgumentException($"{nameof(min)} must be >= 0");
            if (max > 1)
                throw new ArgumentException($"{nameof(max)} must be <= 1");
            if (min >= max)
                throw new ArgumentException($"{nameof(min)} must < {nameof(max)}");

            Bitmap bmp = new(width, height);
            using Graphics gfx = Graphics.FromImage(bmp);
            using Pen pen = new(Color.Magenta);

            if (vertical)
            {
                for (int y = 0; y < height; y++)
                {
                    double fraction = (double)y / (height);
                    fraction = fraction * (max - min) + min;
                    pen.Color = cmap.GetColor(fraction);
                    gfx.DrawLine(pen, 0, height - y - 1, width - 1, height - y - 1);
                }
            }
            else
            {
                for (int x = 0; x < width; x++)
                {
                    double fraction = (double)x / width;
                    fraction = fraction * (max - min) + min;
                    pen.Color = cmap.GetColor(fraction);
                    gfx.DrawLine(pen, x, 0, x, height - 1);
                }
            }

            return bmp;
        }

        public Color[] GetColors()
        {
            return Enumerable
                .Range(0, 255)
                .Select(x => GetColor(x / 255.0))
                .ToArray();
        }

        public Colormap Reversed()
        {
            Color[] colors = GetColors().Reverse().ToArray();
            return new Colormap(colors);
        }
    }
}
