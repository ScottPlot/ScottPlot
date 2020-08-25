﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Space
{
    public interface IAxis
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
        void SetLimits(double min, double max);
        void Pan(float deltaPx);
        float GetPixel(double position);
        double GetPosition(float pixel);

        void Remember();
        void Recall();
    }
}
