using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    class ScottPlotTester
    {
        readonly int pointCount = 100;
        readonly double[] xs;
        readonly double[] ys;
        readonly double[] signal;
        int demoSizeX = 600;
        int demoSizeY = 400;

        public ScottPlotTester()
        {
            // create the data to be used for plotting
            xs = DataGenerator.EvenlySpaced(pointCount);
            ys = DataGenerator.RandomWalk(pointCount);
            signal = DataGenerator.RandomWalk(1_000_000);
        }

        public void RunAllTests()
        {
            // delete old test files
            CleanFolder();

            // run all the tests
            Demo_scatter();
            Demo_addMarkers();
            Demo_labels();
            Demo_axisLimits();
            Demo_scatterStyling();
            Demo_dataMargins();
            Demo_scatterMultiPlot();
            Demo_axisLines();
            Demo_signal();
            Demo_signalZoom();
            Demo_signalZoom2();
            Demo_markersOnly();
            Demo_linesOnly();
            Demo_padding();
            Demo_backgroundFigure();
            Demo_backgroundData();
            Demo_black();
            Demo_frameless();

            // create a report
            Report.MakeHTML();
            Report.MakeMarkdown();

            // figure out where the doc folder is
            string docFolder = System.IO.Path.GetFullPath("./");
            for (int i = 0; i < 5; i++)
                docFolder = System.IO.Path.GetDirectoryName(docFolder);
            docFolder += "/doc/";

            // clear old files in the doc folder
            foreach (var fname in System.IO.Directory.GetFiles(docFolder))
                System.IO.File.Delete(fname);
            foreach (var fname in System.IO.Directory.GetFiles(docFolder + "images/"))
                System.IO.File.Delete(fname);

            // copy files from the output folder to the docs folder
            foreach (var fname in System.IO.Directory.GetFiles("tests/images", "*.*"))
                System.IO.File.Move(fname, docFolder + "images/" + System.IO.Path.GetFileName(fname));
            foreach (var fname in System.IO.Directory.GetFiles("tests/", "*.*"))
                System.IO.File.Move(fname, docFolder + System.IO.Path.GetFileName(fname));
        }

        void CleanFolder()
        {
            foreach (var fname in System.IO.Directory.GetFiles("tests/", "*.*"))
                System.IO.File.Delete(fname);
        }

        void Demo_scatter()
        {
            // Creates a scatter plot from two double arrays
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.settings.title = "Scatter Plot";
            plt.figure.Save("tests/images/scatter.png");
        }

        void Demo_addMarkers()
        {
            // manually place markers at specific points
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.data.AddPoint(20, 1.5, markerSize: 10);
            plt.data.AddPoint(45, 0, markerSize: 20);
            plt.data.AddPoint(65, -1.5, markerSize: 5);
            plt.settings.AxisFit();
            plt.settings.title = "Adding Points";
            plt.figure.Save("tests/images/addPoints.png");
        }

        void Demo_markersOnly()
        {
            // create a plot with only markers (no lines)
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys, lineWidth: 0);
            plt.settings.AxisFit();
            plt.settings.title = "Scatter Plot (markers only)";
            plt.figure.Save("tests/images/markersOnly.png");
        }

        void Demo_linesOnly()
        {
            // create a plot with only lines (no markers)
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys, markerSize: 0);
            plt.settings.AxisFit();
            plt.settings.title = "Scatter Plot (lines only)";
            plt.figure.Save("tests/images/linesOnly.png");
        }

        void Demo_axisLimits()
        {
            // manually define axis limits 
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxesSet(40, 60, 0, 2);
            plt.settings.title = "Axis Limits Set Manually";
            plt.figure.Save("tests/images/axisLimits.png");
        }

        void Demo_labels()
        {
            // axis labels can be easily changed
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.settings.title = "Changing Labels";
            plt.settings.axisLabelY = "Brightness (AFU)";
            plt.settings.axisLabelX = "Experiment Duration (minutes)";
            plt.figure.Save("tests/images/labels.png");
        }

        void Demo_scatterStyling()
        {
            // styling is easy with named arguments
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys, lineWidth: 3, markerSize: 5, lineColor: Color.Blue, markerColor: Color.Red);
            plt.settings.AxisFit();
            plt.settings.title = "Styled Scatter Plot";
            plt.figure.Save("tests/images/scatterStyling.png");
        }

        void Demo_dataMargins()
        {
            // The arguments of AxisFit() are X and Y margins
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit(0, .5);
            plt.settings.title = "Margin Adjustments";
            plt.figure.Save("tests/images/margins.png");
        }

        void Demo_scatterMultiPlot()
        {
            // multiple plots can be added to the same graph
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            for (int i = 0; i < 10; i++)
            {
                var ys = DataGenerator.RandomWalk(pointCount, seed: i);
                plt.data.AddScatter(xs, ys);
            }
            plt.settings.AxisFit();
            plt.settings.title = "Multiple Plots";
            plt.figure.Save("tests/images/multiPlot.png");
        }

        void Demo_axisLines()
        {
            // horizontal and vertical axis lines extend to infinity
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.data.AddHorizLine(1.57, lineWidth: 2, lineColor: Color.Blue);
            plt.data.AddVertLine(38.76, lineWidth: 3, lineColor: Color.Red);
            plt.settings.title = "Drawing Axis Lines";
            plt.figure.Save("tests/images/axisLines.png");
        }

        void Demo_signal()
        {
            // signal plotting is ideal for large arrays of evenly-spaced data
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddSignal(signal, sampleRateHz: 20_000);
            plt.settings.AxisFit(0, .1);
            plt.settings.title = "Plotting Signals (High Density Data)";
            plt.figure.Save("tests/images/signal.png");
        }

        void Demo_signalZoom()
        {
            // demonstrate how to zoom in on portion of a signal
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddSignal(signal, sampleRateHz: 20_000);
            plt.settings.AxisFit(0, .1);
            plt.settings.AxesZoom(20, 1);
            plt.settings.title = "Signal Zoomed";
            plt.figure.Save("tests/images/signalZoomed.png");
        }

        void Demo_signalZoom2()
        {
            // at high magnification signals are representated as step plots
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddSignal(signal, sampleRateHz: 20_000);
            plt.settings.AxisFit(0, .1);
            plt.settings.AxesSet(25.055, 25.057, 338, 345);
            plt.settings.title = "Signal Zoomed More";
            plt.figure.Save("tests/images/demo_signalZoomed2.png");
        }

        void Demo_padding()
        {
            // padding can be changed to accomodate larger labels or ticks
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.settings.SetDataPadding(150, 50, 100, 100);
            plt.settings.title = "Custom Padding";
            plt.figure.Save("tests/images/padding.png");
        }

        void Demo_frameless()
        {
            // one can plot just the data area without labels or ticks
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.settings.SetDataPadding(0, 0, 0, 0);
            plt.settings.axisLabelX = "";
            plt.settings.axisLabelY = "";
            plt.settings.title = "";
            plt.settings.drawAxes = false;
            plt.figure.Save("tests/images/frameless.png");
        }


        void Demo_backgroundFigure()
        {
            // the figure background color can be defined
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.settings.figureBgColor = Color.LightPink;
            plt.settings.title = "Custom Figure Background Color";
            plt.figure.Save("tests/images/backgroundFigure.png");
        }

        void Demo_backgroundData()
        {
            // the data area background color can be defined
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.settings.dataBgColor = Color.LightPink;
            plt.settings.title = "Custom Data Background Color";
            plt.figure.Save("tests/images/backgroundData.png");
        }

        void Demo_black()
        {
            // demonstrate how to change all sorts of colors
            var plt = new ScottPlot.Plot(demoSizeX, demoSizeY);
            plt.data.AddScatter(xs, ys);
            plt.settings.AxisFit();
            plt.settings.figureBgColor = Color.Black;
            plt.settings.dataBgColor = Color.Black;
            plt.settings.gridColor = Color.Maroon;
            plt.settings.tickColor = Color.Gray;
            plt.settings.labelColor = Color.LightGray;
            plt.settings.title = "Dark Styling";
            plt.figure.Save("tests/images/black.png");
        }

        void EndOfSource()
        {
            // this function simplifies source code parsing
        }
    }
}
