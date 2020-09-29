using System;

namespace ScottPlot.Config.DateTimeTickUnits
{
    public interface IDateTimeUnit
    {
        (double[] Ticks, string[] Labels) GetTicksAndLabels(DateTime from, DateTime to, string format);
    }
}
