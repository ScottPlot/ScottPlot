using System;

namespace ScottPlot;

public interface IOHLC
{
    double Open { get; }
    double High { get; }
    double Low { get; }
    double Close { get; }
    DateTime DateTime { get; }
    TimeSpan TimeSpan { get; }
}