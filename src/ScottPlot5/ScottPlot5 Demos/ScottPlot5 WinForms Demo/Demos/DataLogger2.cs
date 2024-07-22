using ScottPlot;
using ScottPlot.AxisPanels;
using ScottPlot.Collections;
using ScottPlot.DataSources;

namespace WinForms_Demo.Demos;

public partial class DataLogger2 : Form, IDemoWindow
{
    public string Title => "Data Streamer (Experimental)";
    public string Description => "Experimental data streamer that uses a circular buffer for improved performance.";

    readonly System.Windows.Forms.Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottables.Experimental.DataStreamer2 Logger1;
    readonly ScottPlot.Plottables.Experimental.DataStreamer2 Logger2;
    readonly ScottPlot.Plottables.Experimental.DataStreamer2 Logger3;
    readonly ScottPlot.Plottables.Experimental.DataStreamer2 Logger4;

    readonly ScottPlot.DataGenerators.RandomWalker Walker1 = new(0, multiplier: 0.01);
    readonly ScottPlot.DataGenerators.RandomWalker Walker2 = new(1, multiplier: 1000);

    public DataLogger2()
    {
        InitializeComponent();

        // disable interactivity by default
        formsPlotHorz.Interaction.Disable();

        // create two horizontal loggers and add them to the plot
        var data1 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
        Logger1 = new ScottPlot.Plottables.Experimental.DataStreamer2(data1) { Color = Colors.C0 };
        formsPlotHorz.Plot.Add.Plottable(Logger1);

        var data2 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
        Logger2 = new ScottPlot.Plottables.Experimental.DataStreamer2(data2) { Color = Colors.C1 };
        formsPlotHorz.Plot.Add.Plottable(Logger2);

        // use the right axis (already there by default) for the first logger
        RightAxis axis1 = (RightAxis)formsPlotHorz.Plot.Axes.Right;
        Logger1.Axes.YAxis = axis1;
        axis1.Color(Logger1.Color);

        // create and add a secondary right axis to use for the other logger
        RightAxis axis2 = formsPlotHorz.Plot.Axes.AddRightAxis();
        Logger2.Axes.YAxis = axis2;
        axis2.Color(Logger2.Color);

        // create two vertical loggers and add them to the plot
        var data3 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
        Logger3 = new ScottPlot.Plottables.Experimental.DataStreamer2(data3) { Color = Colors.C0, Rotated = true };
        formsPlotVert.Plot.Add.Plottable(Logger3);

        var data4 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
        Logger4 = new ScottPlot.Plottables.Experimental.DataStreamer2(data4) { Color = Colors.C1, Rotated = true };
        formsPlotVert.Plot.Add.Plottable(Logger4);

        // use the bottom axis (already there by default) for the first vertical logger
        BottomAxis axis3 = (BottomAxis)formsPlotVert.Plot.Axes.Bottom;
        Logger3.Axes.XAxis = axis3;
        axis3.Color(Logger3.Color);
        
        // create and add a secondary bottom axis to use for the other vertical logger
        BottomAxis axis4 = formsPlotVert.Plot.Axes.AddBottomAxis();
        Logger4.Axes.XAxis = axis4;
        axis4.Color(Logger4.Color);

        formsPlotHorz.Plot.Axes.AutoScaler.InvertedY = true;

        AddNewDataTimer.Tick += (s, e) =>
        {
            int count = 5;
            for (int i = 0; i < count; i++)
            {
                var val1 = Walker1.Next();
                Logger1.Add(val1);
                Logger3.Add(val1);

                var val2 = Walker2.Next();
                Logger2.Add(val2);
                Logger4.Add(val2);
            }
        };

        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (Logger1.HasNewData || Logger2.HasNewData)
                formsPlotHorz.Refresh();
            if (Logger3.HasNewData || Logger4.HasNewData)
                formsPlotVert.Refresh();
        };

        // wire our buttons to change the view modes of each logger
        btnFull.Click += (s, e) => { Logger1.ViewFull(); Logger2.ViewFull(); Logger3.ViewFull(); Logger4.ViewFull(); };
        btnJump.Click += (s, e) => { Logger1.ViewJump(); Logger2.ViewJump(); Logger3.ViewJump(); Logger4.ViewJump(); };
        btnSlide.Click += (s, e) => { Logger1.ViewSlide(); Logger2.ViewSlide(); Logger3.ViewSlide(); Logger4.ViewSlide(); };
    }
}
