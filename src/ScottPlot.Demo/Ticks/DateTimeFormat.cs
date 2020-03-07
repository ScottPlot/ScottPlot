using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Ticks
{
    class DateTimeFormat
    {
        public class DateAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "DateTime Axis";
            public string description { get; } = "Axis tick labels can be set to display date and time format if the values (double[]) are OATime values.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] temperature = DataGen.RandomWalk(rand, 60 * 8);
                DateTime start = new DateTime(2019, 08, 25, 8, 30, 00);
                double pointsPerDay = 24 * 60;

                plt.PlotSignal(temperature, sampleRate: pointsPerDay, xOffset: start.ToOADate());
                plt.Ticks(dateTimeX: true);
                plt.YLabel("Temperature (C)");
            }
        }
    }
}
