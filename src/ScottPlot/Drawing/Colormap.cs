using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ScottPlot.Ticks;

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
                                .Where(p => p.ToString().StartsWith("ScottPlot.Drawing.Colormaps."))
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

        public static Bitmap Colorbar(Colormap cmap, int width, int height, bool vertical = false)
        {
            if (width < 1 || height < 1)
                return null;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (Pen pen = new Pen(Color.Magenta))
            {
                if (vertical)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double fraction = (double)y / (height);
                        pen.Color = cmap.GetColor(fraction);
                        gfx.DrawLine(pen, 0, height - y - 1, width - 1, height - y - 1);
                    }
                }
                else
                {
                    for (int x = 0; x < width; x++)
                    {
                        double fraction = (double)x / width;
                        pen.Color = cmap.GetColor(fraction);
                        gfx.DrawLine(pen, x, 0, x, height - 1);
                    }
                }
            }
            return bmp;
        }
    }
}
