using ScottPlot;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotSkia
{
    public class SettingsSkia : Settings
    {
        public SKSurface surface;
        public SKCanvas canvas;
        public bool useSkiaRender;
        public GRContext context = null;
    }
}
