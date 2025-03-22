using Avalonia.Controls;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.Demos;

public class ShowValueUnderMouseScatterDemo : IDemo
{
    public string Title => "Show Value Under Mouse, Scatter";
    public string Description => "How to sense where the mouse is in coordinate space " +
        "and retrieve information about the plotted data the cursor is hovering over";

    public Window GetWindow()
    {
        return new ShowValueUnderMouseScatterWindow();
    }
}

public class ShowValueUnderMouseScatterWindow : ShowValueUnderMouseWindow
{
    protected override Scatter Plottable { get; }
    public ShowValueUnderMouseScatterWindow()
    {
        double[] xs = Generate.RandomSample(100);
        double[] ys = Generate.RandomSample(100);

        Plottable = AvaPlot.Plot.Add.Scatter(xs, ys);
        Plottable.LineWidth = 0;
    }
}
