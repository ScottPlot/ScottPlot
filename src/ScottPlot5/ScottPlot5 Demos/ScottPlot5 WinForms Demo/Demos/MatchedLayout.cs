using ScottPlot;
using ScottPlot.WinForms;
using System.Diagnostics;

namespace WinForms_Demo.Demos;

public partial class MatchedLayout : Form, IDemoWindow
{
    public string Title => "Matched Axes and Layouts";

    public string Description => "Connect two controls together so they share an axis and have aligned layouts";

    System.Windows.Forms.Timer LayoutTimer = new() { Interval = 10 };

    IPlotControl? ActivePlotControl = null;

    public MatchedLayout()
    {
        InitializeComponent();

        // plot sample data
        formsPlot1.Plot.Add.Signal(Generate.Sin(51, mult: 1));
        formsPlot2.Plot.Add.Signal(Generate.Sin(51, mult: 100_000));

        // use a fixed size for the left axis panel to ensure it's always aligned
        float leftAxisSize = 70;
        formsPlot1.Plot.LeftAxis.MinimumSize = leftAxisSize;
        formsPlot1.Plot.LeftAxis.MaximumSize = leftAxisSize;
        formsPlot2.Plot.LeftAxis.MinimumSize = leftAxisSize;
        formsPlot2.Plot.LeftAxis.MaximumSize = leftAxisSize;

        // use events to determine what plot the mouse is interacting with
        formsPlot1.MouseDown += (s, e) => { ActivePlotControl = formsPlot1; };
        formsPlot2.MouseDown += (s, e) => { ActivePlotControl = formsPlot2; };

        // periodically update the inactive control to match the axis limits of the active one
        LayoutTimer.Tick += (s, e) =>
        {
            if (ActivePlotControl is null)
            {
                Text = "null";
                return;
            }

            AxisLimits limts = ActivePlotControl.Plot.GetAxisLimits();
            foreach (IPlotControl inactivePlot in tableLayoutPanel1.Controls.OfType<IPlotControl>().Where(x => x != ActivePlotControl))
            {
                inactivePlot.Plot.SetAxisLimitsX(limts);
                inactivePlot.Refresh();
            }
        };
        LayoutTimer.Start();

        // initial render
        formsPlot1.Refresh();
        formsPlot2.Refresh();
    }
}
