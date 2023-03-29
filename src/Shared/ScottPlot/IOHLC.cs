using System;

namespace ScottPlot;

public interface IOHLC
{
    double Open { get; set; }
    double High { get; set; }
    double Low { get; set; }
    double Close { get; set; }
    DateTime DateTime { get; set; }
    TimeSpan TimeSpan { get; set; }
}