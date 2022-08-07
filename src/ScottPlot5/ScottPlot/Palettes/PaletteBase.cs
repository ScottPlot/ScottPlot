using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Palettes
{
    public abstract class PaletteBase : IPalette
    {
        protected abstract Color[] colors { get; }
        public Color this[int index] => colors[index % Count];

        public int Count => colors.Length;

        public IEnumerator<Color> GetEnumerator() => ((IEnumerable<Color>)colors).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => colors.GetEnumerator();
    }
}
