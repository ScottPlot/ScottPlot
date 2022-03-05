using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Drawing
{
    public class ColormapFactory
    {
        private readonly Dictionary<string, Func<IColormap>> Colormaps =
            new Dictionary<string, Func<IColormap>>()
            {
                { "Algae", () => new Algae()},
                { "Amp", () => new Amp()},
                { "Balance", () => new Balance()},
                { "Blues", () => new Blues()},
                { "Curl", () => new Curl()},
                { "Deep", () => new Deep()},
                { "Delta", () => new Delta()},
                { "Dense", () => new Dense()},
                { "Diff", () => new Diff()},
                { "Grayscale", () => new Grayscale()},
                { "GrayscaleR", () => new GrayscaleR()},
                { "Greens", () => new Greens()},
                { "Haline", () => new Haline()},
                { "Ice", () => new Ice()},
                { "Inferno", () => new Inferno()},
                { "Jet", () => new Jet()},
                { "Magma", () => new Magma()},
                { "Matter", () => new Matter()},
                { "Oxy", () => new Oxy()},
                { "Phase", () => new Phase()},
                { "Plasma", () => new Plasma()},
                { "Rain", () => new Rain()},
                { "Solar", () => new Solar()},
                { "Speed", () => new Speed()},
                { "Tarn", () => new Tarn()},
                { "Tempo", () => new Tempo()},
                { "Thermal", () => new Thermal()},
                { "Topo", () => new Topo()},
                { "Turbid", () => new Turbid()},
                { "Turbo", () => new Turbo()},
                { "Viridis", () => new Viridis()},
            };

        public IColormap GetDefaultColormap() => new Grayscale();

        public Colormap CreateOrDefault(string Name)
        {
            if (Colormaps.TryGetValue(Name, out Func<IColormap> cmap))
                return new Colormap(cmap());
            else
                return new Colormap(GetDefaultColormap());
        }

        public Colormap CreateOrThrow(string Name)
        {
            if (Colormaps.TryGetValue(Name, out Func<IColormap> cmap))
                return new Colormap(cmap());
            else
                throw new ArgumentOutOfRangeException($"No colormap with name '{Name}'");
        }

        public IEnumerable<string> GetAvailableNames() => Colormaps.Keys;

        public IEnumerable<Colormap> GetAvailableColormaps() =>
            Colormaps.Values.Select(f => new Colormap(f()));
    }
}
