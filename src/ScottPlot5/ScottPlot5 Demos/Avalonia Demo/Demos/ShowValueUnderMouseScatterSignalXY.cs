using Avalonia.Controls;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.Demos;

public class ShowValueUnderMouseSignalXYDemo : IDemo
{
    public string Title => "Show Value Under Mouse, SignalXY";
    public string Description => "How to sense where the mouse is in coordinate space " +
        "and retrieve information about the plotted data the cursor is hovering over";

    public Window GetWindow()
    {
        return new ShowValueUnderMouseSignalXYWindow();
    }
}

public class ShowValueUnderMouseSignalXYWindow : ShowValueUnderMouseWindow
{
    protected override SignalXY Plottable { get; }
    public ShowValueUnderMouseSignalXYWindow()
    {
        double[] xs = Generate.Consecutive(1000);
        double[] ys = Generate.RandomWalk(1000);

        Plottable = AvaPlot.Plot.Add.SignalXY(xs, ys);
    }
}
