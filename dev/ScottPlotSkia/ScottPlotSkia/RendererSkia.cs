using ScottPlot;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotSkia
{
    public class RendererSkia
    {
        public static void DataBackgroundSkia(Settings settings)
        {
            var settingsSkia = (settings as SettingsSkia);
            if (settingsSkia == null)
                Renderer.DataBackgroundImpl(settings);
            else
                settingsSkia.canvas.Clear(settings.dataBackgroundColor.ToSKColor());
        }

        public static void DataGridSkia(Settings settings)
        {

            var settingsSkia = (settings as SettingsSkia);
            if (settingsSkia == null)
                Renderer.DataGridImpl(settings);
            else
            {
                if (settings.displayGrid == false)
                    return;
                
                SKPaint paint = new SKPaint()
                {
                    Color = settings.gridColor.ToSKColor(),
                    IsAntialias = true,
                };

                for (int i = 0; i < settings.tickCollectionX.tickPositionsMajor.Length; i++)
                {
                    double value = settings.tickCollectionX.tickPositionsMajor[i];
                    double unitsFromAxisEdge = value - settings.axis[0];
                    int xPx = (int)(unitsFromAxisEdge * settings.xAxisScale);                    
                    settingsSkia.canvas.DrawLine(xPx, 0, xPx, settings.dataSize.Height, paint);
                }

                for (int i = 0; i < settings.tickCollectionY.tickPositionsMajor.Length; i++)
                {
                    double value = settings.tickCollectionY.tickPositionsMajor[i];
                    double unitsFromAxisEdge = value - settings.axis[2];
                    int yPx = settings.dataSize.Height - (int)(unitsFromAxisEdge * settings.yAxisScale);
                    settingsSkia.canvas.DrawLine(0, yPx, settings.dataSize.Width, yPx, paint);                    
                }
            }
        }


        public static void PlaceDataOntoFigureSkia(Settings settings)
        {
            var settingsSkia = (settings as SettingsSkia);
            if (settingsSkia == null)
                Renderer.PlaceDataOntoFigureImpl(settings);
            else
            {
                var snapshot = settingsSkia.surface.Snapshot();
                var bitmap = snapshot.ToBitmap();
                settings.gfxFigure.DrawImage(bitmap, settings.dataOrigin);
                snapshot.Dispose();
                bitmap.Dispose();
            }
        }
    }
}
