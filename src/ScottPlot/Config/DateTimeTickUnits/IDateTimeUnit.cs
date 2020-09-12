using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config.DateTimeTickUnits
{
    public interface IDateTimeUnit
    {
        (DateTime[] Ticks, string[] Labels) GetTicksAndLabels(DateTime from, DateTime to);
    }
}
