using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    internal class ScopedTransform : IDisposable
    {
        private readonly SKCanvas canvas;
        private readonly int restoreIndex;
        private bool _disposed;

        public ScopedTransform(SKCanvas canvas)
        {
            this.canvas = canvas;
            restoreIndex = canvas.Save();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    canvas.RestoreToCount(restoreIndex);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ScopedTransform was not disposed before finalizer ran. This will likely cause incorrect drawing.");
                    // We shouldn't RestoreToCount here, as a) it's probably even harder to debug, and b) this.canvas has likely been disposed already
                }

                _disposed = true;
            }
        }

        ~ScopedTransform()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
