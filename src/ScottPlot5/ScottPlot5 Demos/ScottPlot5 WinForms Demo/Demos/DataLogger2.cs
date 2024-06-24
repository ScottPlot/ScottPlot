﻿using ScottPlot;
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

    readonly ScottPlot.DataGenerators.RandomWalker Walker1 = new(0, multiplier: 0.01);
    readonly ScottPlot.DataGenerators.RandomWalker Walker2 = new(1, multiplier: 1000);

    public DataLogger2()
    {
        InitializeComponent();

        // disable interactivity by default
        formsPlot1.Interaction.Disable();

        // create two loggers and add them to the plot
        var data1 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
        Logger1 = new ScottPlot.Plottables.Experimental.DataStreamer2(data1);
        Logger1.Color = Colors.C0;
        formsPlot1.Plot.Add.Plottable(Logger1);

        var data2 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
        Logger2 = new ScottPlot.Plottables.Experimental.DataStreamer2(data2);
        Logger2.Color = Colors.C1;
        formsPlot1.Plot.Add.Plottable(Logger2);

        // use the right axis (already there by default) for the first logger
        RightAxis axis1 = (RightAxis)formsPlot1.Plot.Axes.Right;
        Logger1.Axes.YAxis = axis1;
        axis1.Color(Logger1.Color);

        // create and add a secondary right axis to use for the other logger
        RightAxis axis2 = formsPlot1.Plot.Axes.AddRightAxis();
        Logger2.Axes.YAxis = axis2;
        axis2.Color(Logger2.Color);

        AddNewDataTimer.Tick += (s, e) =>
        {
            int count = 5;
            Logger1.Add(Walker1.Next(count));
            Logger2.Add(Walker2.Next(count));
        };

        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (Logger1.HasNewData || Logger2.HasNewData)
                formsPlot1.Refresh();
        };

        // wire our buttons to change the view modes of each logger
        btnFull.Click += (s, e) => { Logger1.ViewFull(); Logger2.ViewFull(); };
        btnJump.Click += (s, e) => { Logger1.ViewJump(); Logger2.ViewJump(); };
        btnSlide.Click += (s, e) => { Logger1.ViewSlide(); Logger2.ViewSlide(); };
    }
}
