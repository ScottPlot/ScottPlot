using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    internal class ScopedTransform : IDisposable // TODO: I'm a little uneasy on this name?
    {
        private readonly SKCanvas canvas;
        private readonly int restoreIndex;

        public ScopedTransform(SKCanvas canvas)
        {
            this.canvas = canvas;
            restoreIndex = canvas.Save();
        }

        public void Dispose()
        {
            canvas.RestoreToCount(restoreIndex);
        }
    }
}
