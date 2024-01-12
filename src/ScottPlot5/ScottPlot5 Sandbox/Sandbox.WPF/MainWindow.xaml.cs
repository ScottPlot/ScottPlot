using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ScottPlot;
using ScottPlot.WPF;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ShowCharts();
    }

    private void ShowCharts()
    {
        List<WpfPlot> wpfPlotList = new();

        for (int i = 0; i < 11; i++)
        {
            double[] doubles = Generate.RandomWalk(7_016_960);
            float[] floats = doubles.Select(x => (float)x).ToArray();

            var plot = new WpfPlot();
            plot.Plot.Add.Signal(doubles, 256);
            wpfPlotList.Add(plot);
        }

        for (int sourceIndex = 0; sourceIndex < wpfPlotList.Count; sourceIndex++)
        {
            var sourceChart = wpfPlotList[sourceIndex];
            sourceChart.Height = 200;

            for (int targetIndex = 0; targetIndex < wpfPlotList.Count; targetIndex++)
            {
                if (sourceIndex == targetIndex)
                    continue;
                var targetChart = wpfPlotList[targetIndex];
                sourceChart.MouseUp += (s, e) =>
                {
                    ApplyLayoutToOtherPlot(sourceChart, targetChart);
                };
            }

            var rule = new ScottPlot.AxisRules.LockedVertical(sourceChart.Plot.Axes.Left);
            sourceChart.Plot.Axes.Rules.Clear();
            sourceChart.Plot.Axes.Rules.Add(rule);
            sourceChart.Refresh();

            charts.Children.Add(sourceChart);
        }
    }

    private void ApplyLayoutToOtherPlot(IPlotControl source, IPlotControl target)
    {
        AxisLimits axesBefore = target.Plot.Axes.GetLimits();
        target.Plot.Axes.SetLimitsX(source.Plot.Axes.GetLimits());
        AxisLimits axesAfter = target.Plot.Axes.GetLimits();
        if (axesBefore != axesAfter)
        {
            target.Refresh();
        }
    }
}
