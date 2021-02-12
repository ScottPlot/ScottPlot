using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Drawing
{
    public class ColormapFactory
    {
        private Dictionary<string, Func<IColormap>> avaialbeColormaps;
        public ColormapFactory()
        {
            avaialbeColormaps = new Dictionary<string, Func<IColormap>>()
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
        }

        public Colormap Create(string Name)
        {
            Func<IColormap> cmap;
            if (avaialbeColormaps.TryGetValue(Name, out cmap))
            {
                return new Colormap(cmap());
            }
            return new Colormap(new Algae());
        }

        public Colormap CreateUnsafe(string Name)
        {
            Func<IColormap> cmap;
            if (avaialbeColormaps.TryGetValue(Name, out cmap))
            {
                return new Colormap(cmap());
            }
            throw new ArgumentOutOfRangeException($"Can't find colormap with name={Name}");
        }

        public IEnumerable<string> GetAvailableNames()
        {
            return avaialbeColormaps.Keys;
        }

        public IEnumerable<Colormap> GetAvailableColormaps()
        {
            return avaialbeColormaps.Values.Select(f => new Colormap(f()));
        }
    }
}
