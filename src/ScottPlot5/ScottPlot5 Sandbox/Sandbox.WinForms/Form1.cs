using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        cbManual.Checked = true;
        cbInterior.Checked = true;
        cbRotated.Checked = true;

        cbInterior.CheckedChanged += (s, e) => SetupPlot();
        cbManual.CheckedChanged += (s, e) => SetupPlot();
        cbRotated.CheckedChanged += (s, e) => SetupPlot();

        SetupPlot();
    }

    private void SetupPlot()
    {
        formsPlot1.Reset();

        // plot sample data
        Coordinates[] dataPoints = GetSampleData();
        for (int i = 0; i < dataPoints.Length; i++)
        {
            var marker = formsPlot1.Plot.Add.Marker(dataPoints[i]);
            marker.Label = $"#{i + 1}";
        }

        // add a legend
        // TODO: add row and column support to legend
        // https://github.com/ScottPlot/ScottPlot/issues/3531
        //formsPlot1.Plot.ShowLegend(Alignment.UpperLeft);
        //formsPlot1.Plot.Legend.Orientation = ScottPlot.Orientation.Horizontal;

        // create isolines, style them, and add them to the plot
        ScottPlot.Plottables.IsoLines isoLines = new();
        isoLines.LineStyle.Color = Colors.Blue.WithAlpha(.5);
        isoLines.TickLabelStyle.ForeColor = Colors.Blue.WithAlpha(.5);
        isoLines.TickLabelStyle.FontSize = 16;
        isoLines.RotateLabels = cbRotated.Checked;
        isoLines.ExteriorTickLabels = !cbInterior.Checked;
        formsPlot1.Plot.Add.Plottable(isoLines);

        if (cbManual.Checked)
        {
            // if manual isoline positions are defined, only those positions will be used.
            // Values are the Y position when X is zero.
            isoLines.ManualPositions.Add((14, "0.01 pM"));
            isoLines.ManualPositions.Add((10, "100 pM"));
            isoLines.ManualPositions.Add((6, "1 µM"));
            isoLines.ManualPositions.Add((2, "10 mM"));
            isoLines.ManualPositions.Add((-2, "100 M"));
            isoLines.ManualPositions.Add((-6, "1e6 M"));
            isoLines.ManualPositions.Add((-10, "1e10 M"));
            isoLines.ManualPositions.Add((-14, "1e14 M"));
        }
        else
        {
            static string IsolineLabelFormatter(double value)
            {
                return (value >= 0)
                    ? $"1E+{Math.Round(value, 0)}"
                    : $"1E{Math.Round(value, 0)}";
            }
            isoLines.TickLabelFormatter = IsolineLabelFormatter;
        }

        // space major ticks farther apart so isolines aren't too dense
        ScottPlot.TickGenerators.NumericAutomatic tickGenX = new() { MinimumTickSpacing = 50 };
        formsPlot1.Plot.Axes.Bottom.TickGenerator = tickGenX;
        ScottPlot.TickGenerators.NumericAutomatic tickGenY = new() { MinimumTickSpacing = 50 };
        formsPlot1.Plot.Axes.Left.TickGenerator = tickGenY;

        // configure minor tick marks to display with log-scaling distribution
        ScottPlot.TickGenerators.LogMinorTickGenerator logMinorTickGen = new();
        tickGenX.MinorTickGenerator = logMinorTickGen;
        tickGenY.MinorTickGenerator = logMinorTickGen;

        if (!cbInterior.Checked)
        {
            // add more space for ticks outside the data area
            PixelPadding padding = new(50, 100, 50, 70);
            formsPlot1.Plot.Layout.Fixed(padding);
        }

        // set axis limits to mimic the original screenshot
        formsPlot1.Plot.Axes.SetLimits(-8, 6, -10, 10);

        // style the plot and update the display
        formsPlot1.Plot.YLabel("log kOn (1/s)");
        formsPlot1.Plot.XLabel("log kOff (1/Ms)");
        formsPlot1.Refresh();
    }

    private static Coordinates[] GetSampleData()
    {
        // note points manually
        Coordinates[] points =
        {
            new(13, -8),
            new(37, -10),
            new(45, -6),
            new(16, -12),
            new(49, -13),
            new(66, -13),
            new(36, -13),
            new(64, -14),
            new(61, -14),
            new(60, -16),
            new(61, -15),
            new(66, -18),
            new(64, -18),
            new(65, -20),
            new(66, -20),
            new(62, -21),
            new(64, -25),
            new(68, -29),
            new(67, -42),
            new(69, -47),
            new(70, -54),
        };

        // scale 0-1
        double[] ys = points.Select(p => p.Y).ToArray();
        double minY = ys.Min();
        ys = ys.Select(y => y - minY).ToArray();
        double maxY = ys.Max();
        ys = ys.Select(y => y / maxY).ToArray();

        // scale 0-1
        double[] xs = points.Select(p => p.X).ToArray();
        double minX = xs.Min();
        xs = xs.Select(x => x - minX).ToArray();
        double maxX = xs.Max();
        xs = xs.Select(x => x / maxX).ToArray();

        // scale to custom range
        double targetMinY = -7;
        double targetMaxY = 8.5;
        double targetSpanY = targetMaxY - targetMinY;
        ys = ys.Select(y => y * targetSpanY + targetMinY).ToArray();

        // scale to custom range
        double targetMinX = -6;
        double targetMaxX = 1;
        double targetSpanX = targetMaxX - targetMinX;
        xs = xs.Select(x => x * targetSpanX + targetMinX).ToArray();

        return Enumerable
            .Range(0, xs.Length)
            .Select(i => new Coordinates(xs[i], ys[i]))
            .ToArray();
    }
}
