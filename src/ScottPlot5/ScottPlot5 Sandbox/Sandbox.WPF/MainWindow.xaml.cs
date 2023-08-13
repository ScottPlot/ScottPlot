using System.Linq;
using System.Windows;
using ScottPlot;

#nullable enable

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Loaded += (s, e) =>
        {
            // customize the layout
            MultiWpfPlot1.Multiplot.Layout = new ScottPlot.MultiplotLayouts.Grid(2, 3);

            // create plots and add them to the multiplot
            RandomDataGenerator gen = new();
            for (int i = 0; i < 6; i++)
            {
                Plot plot = new();
                plot.Add.Signal(gen.RandomWalk(100));
                plot.Title($"Plot {i + 1}");
                MultiWpfPlot1.Multiplot.Add(plot);
            }

            // apply the layout from the first plot to all subplots
            MultiWpfPlot1.Multiplot.SharedLayoutSourcePlot = MultiWpfPlot1.Multiplot.Plots.First();

            MultiWpfPlot1.Refresh();
        };
    }
}
