using ScottPlot;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotSkia
{
    public class PlotSkia : Plot
    {
        public PlotSkia(int width = 800, int height = 600) : base(new SettingsSkia())
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");
            GetSettings().dataBackend = new GDIbackend(width, height, pixelFormat);
            Resize(width, height);
            TightenLayout();
        }

        protected override void InitializeBitmaps()
        {
            base.InitializeBitmaps();
            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                var settingsSkia = settings as SettingsSkia;
                var info = new SKImageInfo(settings.dataSize.Width, settings.dataSize.Height);
                if (settingsSkia.context == null)
                {
                    try
                    {
                        var glInterface = GRGlInterface.CreateNativeAngleInterface();
                        settingsSkia.context = GRContext.Create(GRBackend.OpenGL, glInterface);
                    }
                    catch
                    {
                        settingsSkia.context = GRContext.Create(GRBackend.OpenGL);
                    }

                }
                settingsSkia.surface?.Dispose();
                settingsSkia.surface = SKSurface.Create(settingsSkia.context, true, info);
                settingsSkia.canvas = settingsSkia.surface.Canvas;
            }
        }

        public override PlottableSignalConst<T> PlotSignalConst<T>(
            T[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableSignalConst<T> signal = new PlottableSignalConstSkia<T>(
                ys: ys,
                sampleRate: sampleRate,
                xOffset: xOffset,
                yOffset: yOffset,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                useParallel: settings.useParallel
                );

            settings.plottables.Add(signal);
            return signal;
        }
    }
}
