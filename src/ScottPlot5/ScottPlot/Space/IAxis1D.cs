using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Space
{
    public interface IAxis1D
    {
        float FigureSizePx { get; }
        float DataSizePx { get; }
        float DataOffsetPx { get; }
        double Min { get; }
        double Max { get; }
        double Span { get; }
        double UnitsPerPx { get; }
        double PxPerUnit { get; }

        void Resize(float figureSize, float dataSize, float dataOffset);
        AxisLimits1D GetLimits();
        void SetLimits(AxisLimits1D limits);
        void SetLimits(double min, double max);

        bool IsValid { get; }
        bool IsLocked { get; set; }

        void PanPx(float deltaPx);
        void Zoom(double frac);
        void ZoomTo(double frac, double target);
        void ZoomPx(float deltaPx);

        void RememberLimits();
        void RecallLimits();

        float GetPixel(double position);
        double GetPosition(float pixel);
    }
}
