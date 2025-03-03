using Avalonia.Controls;
using Avalonia.Input;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.Demos;

public class ContionuouslyAutoscaleDemo : IDemo
{
    public string Title => "Continuously Autoscale";

    public string Description => "Custom axis scale logic may be applied at the start of each render";

    public Window GetWindow()
    {
        return new ContinuouslyAutoscaleWindow();
    }

}

public class ContinuouslyAutoscaleWindow : SimpleDemoWindow
{
    public ContinuouslyAutoscaleWindow() : base("Continuously Autoscale")
    {

    }

    protected override void StartDemo()
    {
        // simulate 1 minute of a 1kHz siganl
        double[] dataValues = Generate.RandomWalk(60_000);
        double sampleRate = 1000;
        double samplePeriod = 1.0 / sampleRate;

        // display the data using a signal plot
        var sig = AvaPlot.Plot.Add.Signal(dataValues, samplePeriod);

        // enable continuous autoscaling with a custom action
        AvaPlot.Plot.Axes.ContinuouslyAutoscale = true;
        AvaPlot.Plot.Axes.ContinuousAutoscaleAction = (RenderPack rp) =>
        {
            // determine the left and right data index in view
            AxisLimits limits = AvaPlot.Plot.Axes.GetLimits();
            int indexLeft = (int)(limits.Left * sampleRate);
            int indexRight = (int)(limits.Right * sampleRate);

            // insure indexes are valid
            indexLeft = NumericConversion.Clamp(indexLeft, 0, dataValues.Length - 1);
            indexRight = NumericConversion.Clamp(indexRight, 0, dataValues.Length - 1);
            if (indexLeft == indexRight)
                return;

            // determine the vertical range of values in the visible range of indexes
            double min = dataValues[indexLeft];
            double max = dataValues[indexLeft];
            for (int i = indexLeft; i <= indexRight; i++)
            {
                min = Math.Min(min, dataValues[i]);
                max = Math.Max(max, dataValues[i]);
            }

            // set vertical axis limits to that range
            rp.Plot.Axes.SetLimitsY(min, max);
        };
    }
}
