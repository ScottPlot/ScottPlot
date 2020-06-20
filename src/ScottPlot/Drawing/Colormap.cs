using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Drawing
{
    class Colormap
    {
        public static Colormap Blues => new Colormap(new Colormaps.Blues());
        public static Colormap Grayscale => new Colormap(new Colormaps.Grayscale());
        public static Colormap GrayscaleReversed => new Colormap(new Colormaps.Grayscale());
        public static Colormap Greens => new Colormap(new Colormaps.Greens());
        public static Colormap Inferno => new Colormap(new Colormaps.Inferno());
        public static Colormap Magma => new Colormap(new Colormaps.Magma());
        public static Colormap Plasma => new Colormap(new Colormaps.Plasma());
        public static Colormap Turbo => new Colormap(new Colormaps.Turbo());
        public static Colormap Viridis => new Colormap(new Colormaps.Viridis());

        private readonly IColormap cmap;
        public readonly string Name;
        public Colormap(IColormap colormap)
        {
            cmap = colormap ?? new Colormaps.Grayscale();
            Name = cmap.GetType().Name;
        }

        public override string ToString()
        {
            return $"Colormap {Name}";
        }

        public static Colormap[] GetColormaps()
        {
            IColormap[] ics = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => p.IsInterface == false)
                                .Where(p => p.ToString().StartsWith("Spectrogram.Colormaps."))
                                .Select(x => x.ToString())
                                .Select(path => (IColormap)Activator.CreateInstance(Type.GetType(path)))
                                .ToArray();

            return ics.Select(x => new Colormap(x)).ToArray();
        }

        public (byte r, byte g, byte b) GetRGB(byte value)
        {
            return cmap.GetRGB(value);
        }

        public (byte r, byte g, byte b) GetRGB(double fraction)
        {
            fraction = Math.Max(fraction, 0);
            fraction = Math.Min(fraction, 1);
            return cmap.GetRGB((byte)(fraction * 255));
        }

        public int GetInt32(byte value)
        {
            var (r, g, b) = GetRGB(value);
            return 255 << 24 | r << 16 | g << 8 | b;
        }

        public int GetInt32(double fraction)
        {
            var (r, g, b) = GetRGB(fraction);
            return 255 << 24 | r << 16 | g << 8 | b;
        }

        public Color GetColor(byte value)
        {
            return Color.FromArgb(GetInt32(value));
        }

        public Color GetColor(double fraction)
        {
            return Color.FromArgb(GetInt32(fraction));
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

        public static int[] IntensitiesToARGB(double[] intensities, double? transparencyThreshold = null)
        {
            return intensities.AsParallel().AsOrdered().Select(i => i < (transparencyThreshold ?? double.NegativeInfinity) ? unchecked((int)0x00000000) : RGBToARGB(new byte[] { cmap[(int)(i * 255), 0], cmap[(int)(i * 255), 1], cmap[(int)(i * 255), 2] })).ToArray();
        }
    }
}
