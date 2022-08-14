using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class Quickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_quickstart";
        public string Title => "Scatter Plot Quickstart";
        public string Description =>
            "Scatter plots are best for small numbers of paired X/Y data points. " +
            "For evenly-spaced data points Signal is much faster.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample X/Y data
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            // add scatter plots
            plt.AddScatter(x, sin);
            plt.AddScatter(x, cos);
        }
    }

    public class CustomizeMarkers : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_markers";
        public string Title => "Custom markers";
        public string Description => "Markers can be customized using optional arguments and public fields.";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            // add scatter plots and customize markers
            var sp1 = plt.AddScatter(x, sin, markerSize: 15);
            sp1.MarkerShape = MarkerShape.openCircle;

            var sp2 = plt.AddScatter(x, cos, markerSize: 7);
            sp2.MarkerShape = MarkerShape.filledSquare;
        }
    }

    public class AllMarkers : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_markerShape";
        public string Title => "All marker shapes";
        public string Description => "Legend indicates names of all available marker shapes";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] xs = DataGen.Consecutive(pointCount);

            string[] markerShapeNames = Enum.GetNames(typeof(MarkerShape));
            for (int i = 0; i < markerShapeNames.Length; i++)
            {
                Enum.TryParse(markerShapeNames[i], out MarkerShape ms);
                double[] ys = DataGen.Sin(pointCount, 2, -i);
                var sp = plt.AddScatter(xs, ys);
                sp.LineWidth = 2;
                sp.LineColor = Color.FromArgb(50, sp.LineColor);
                sp.MarkerSize = 7;
                sp.MarkerShape = ms;
                sp.Label = ms.ToString();
            }

            plt.Grid(enable: false);
            var legend = plt.Legend();
            legend.FontSize = 10;
        }
    }

    public class CustomizeLines : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_lineStyle";
        public string Title => "Custom lines";
        public string Description =>
            "Line color, size, and style can be customized. " +
            "Setting markerSize to 0 prevents markers from being rendered.";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);
            double[] cos2 = DataGen.Cos(pointCount, mult: -1);

            plt.AddScatter(x, sin, color: Color.Magenta, lineWidth: 0, markerSize: 10);
            plt.AddScatter(x, cos, color: Color.Green, lineWidth: 5, markerSize: 0);
            plt.AddScatter(x, cos2, color: Color.Blue, lineWidth: 3, markerSize: 0, lineStyle: LineStyle.DashDot);

            var legend = plt.Legend();
            legend.FixedLineWidth = false;
        }
    }

    public class RandomXY : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_random";
        public string Title => "Random X/Y Points";
        public string Description =>
            "X data for scatter plots does not have to be evenly spaced, " +
            "making scatter plots are ideal for displaying random data like this.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 51;
            double[] xs1 = DataGen.RandomNormal(rand, pointCount, 1);
            double[] xs2 = DataGen.RandomNormal(rand, pointCount, 3);
            double[] ys1 = DataGen.RandomNormal(rand, pointCount, 5);
            double[] ys2 = DataGen.RandomNormal(rand, pointCount, 7);

            plt.AddScatter(xs1, ys1, markerSize: 0, label: "lines only");
            plt.AddScatter(xs2, ys2, lineWidth: 0, label: "markers only");
            plt.Legend();
        }
    }

    public class ErrorBars : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_errorbar";
        public string Title => "Scatter Plot with Errorbars";
        public string Description =>
            "An array of values can be supplied for error bars " +
            "and redering options can be customized as desired";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 20;
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.RandomWalk(rand, pointCount);
            double[] xErr = DataGen.RandomNormal(rand, pointCount, .2);
            double[] yErr = DataGen.RandomNormal(rand, pointCount);

            var sp = plt.AddScatter(xs, ys);
            sp.XError = xErr;
            sp.YError = yErr;
            sp.ErrorCapSize = 3;
            sp.ErrorLineWidth = 1;
            sp.LineStyle = LineStyle.Dot;
        }
    }

    public class ShadedError : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_shaded_error";
        public string Title => "Scatter Plot with Shaded Error";
        public string Description =>
            "A semitransparent polygon can be created and placed behind the scatter plot " +
            "to represent standard deviation or standard error.";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 20;
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.RandomWalk(rand, pointCount, 2.0);
            double[] yErr = DataGen.Random(rand, pointCount, 1.0, 1.0);

            plt.AddScatter(xs, ys, Color.Blue);
            plt.AddFillError(xs, ys, yErr, Color.FromArgb(50, Color.Blue));
        }
    }

    public class LinePlot : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_lineplot";
        public string Title => "Lines Only";
        public string Description => "A shortcut method makes it easy " +
            "to create a scatter plot with just lines (no markers)";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] ys = DataGen.Sin(51);

            plt.AddScatterLines(xs, ys, Color.Red, 3);
        }
    }

    public class PointsPlot : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_pointsplot";
        public string Title => "Markers Only";
        public string Description => "A shortcut method makes it easy to create a scatter plot " +
            "where markers are displayed at every point (without any connecting lines)";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] ys = DataGen.Sin(51);

            plt.AddScatterPoints(xs, ys, Color.Navy, 10, MarkerShape.filledDiamond);
        }
    }

    public class StepPlot : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_stepplot";
        public string Title => "Step Plot";
        public string Description => "A step plot is a special type of scatter plot where points " +
            "are connected by right angles instead of straight lines.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] ys = DataGen.Sin(51);

            plt.AddScatterStep(xs, ys);
        }
    }

    public class AddMarker : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_AddMarker";
        public string Title => "Add markers";
        public string Description => "Want to place a marker at a position in X/Y space? " +
            "AddMarker() will create a scatter plot with a single point.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(0);
            for (int i = 0; i < 100; i++)
            {
                double x = rand.Next(100);
                double y = rand.Next(100);
                double fraction = rand.NextDouble();
                double size = (fraction + .1) * 30;
                var color = Drawing.Colormap.Turbo.GetColor(fraction, alpha: .8);
                var shape = Marker.Random(rand);
                plt.AddMarker(x, y, shape, size, color);
            }
        }
    }

    public class ScatterPlotDraggable : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_draggable_vertical";
        public string Title => "Draggable Scatter Plot";
        public string Description => "Want to modify the scatter points interactively? " +
            "A ScatterPlotDraggable lets you move the points around with the mouse. " +
            "As you move the points around, the values in the original arrays change to " +
            "reflect their new positions.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] x = ScottPlot.DataGen.Consecutive(50);
            double[] y = ScottPlot.DataGen.Cos(50);

            var scatter = new ScottPlot.Plottable.ScatterPlotDraggable(x, y)
            {
                DragCursor = Cursor.Crosshair,
                DragEnabled = true,
            };

            plt.Add(scatter);
        }
    }

    public class ScatterPlotDraggableVertical : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_draggable";
        public string Title => "Draggable Scatter Plot Vertical";
        public string Description => "You can restrict dragging to just X or Y directions.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] x = ScottPlot.DataGen.Consecutive(50);
            double[] y = ScottPlot.DataGen.Cos(50);

            var scatter = new ScottPlot.Plottable.ScatterPlotDraggable(x, y)
            {
                DragCursor = Cursor.Crosshair,
                DragEnabled = true,   // controls whether anything can be dragged
                DragEnabledX = false, // controls whether points can be dragged horizontally 
                DragEnabledY = true,  // controls whether points can be dragged vertically
            };

            plt.Add(scatter);
        }
    }

    public class ScatterPlotForestPlot : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_forest";
        public string Title => "Forest Plot";
        public string Description => "Scatter plots can be used to create forest plots, which are useful for showing the agreement between multiple estimates.";

        public void ExecuteRecipe(Plot plt)
        {
            var plot1 = plt.AddScatter(new double[] { 2.5 }, new double[] { 5 }, label: "John Doe et al.");
            plot1.XError = new double[] { 0.2 };

            var plot2 = plt.AddScatter(new double[] { 2.7 }, new double[] { 4 }, label: "Jane Doe et al.");
            plot2.XError = new double[] { 0.3 };

            var plot3 = plt.AddScatter(new double[] { 2.3 }, new double[] { 3 }, label: "Jim Doe et al.");
            plot3.XError = new double[] { 0.6 };

            var plot4 = plt.AddScatter(new double[] { 2.8 }, new double[] { 2 }, label: "Joel Doe et al.");
            plot4.XError = new double[] { 0.3 };

            var plot5 = plt.AddScatter(new double[] { 2.5 }, new double[] { 1 }, label: "Jacqueline Doe et al.");
            plot5.XError = new double[] { 0.2 };

            plt.AddVerticalLine(2.6, style: LineStyle.Dash);

            plt.SetAxisLimits(0, 5, 0, 6);
            plt.Legend();
        }
    }

    public class ScatterSmooth : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_smooth";
        public string Title => "Scatter Plot with Smooth Lines";
        public string Description =>
            "Lines drawn between scatter plot points are typically connected with straight lines, " +
            "but the Smooth property can be enabled to connect points with curves instead.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(1234);
            double[] xs = DataGen.RandomWalk(rand, 20);
            double[] ys = DataGen.RandomWalk(rand, 20);
            plt.Palette = new ScottPlot.Palettes.ColorblindFriendly();

            var sp1 = plt.AddScatter(xs, ys, label: "default");
            sp1.Smooth = true;

            var sp2 = plt.AddScatter(xs, ys, label: "high tension");
            sp2.Smooth = true;
            sp2.SmoothTension = 1.0f;

            var sp3 = plt.AddScatter(xs, ys, label: "low tension");
            sp3.Smooth = true;
            sp3.SmoothTension = 0.2f;

            plt.Legend();
        }
    }

    public class ScatterNaNIgnore : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_nan_ignore";
        public string Title => "NaN Values Ignored";
        public string Description =>
            "When the OnNaN field is set to Ignore, points containing NaN X or Y values are skipped, " +
            "and the scatter plot is drawn as one continuous line.";

        public void ExecuteRecipe(Plot plt)
        {
            // create data that does NOT contain NaN
            double[] xs = ScottPlot.DataGen.Consecutive(51);
            double[] ys = ScottPlot.DataGen.Sin(51);

            // plot it the traditional way
            plt.AddScatter(xs, ys, Color.FromArgb(50, Color.Black));

            // create new data that contains NaN
            double[] ysWithNan = ScottPlot.DataGen.Sin(51);
            static void FillWithNan(double[] values, int start, int count)
            {
                for (int i = 0; i < count; i++)
                    values[start + i] = double.NaN;
            }
            FillWithNan(ysWithNan, 5, 15);
            FillWithNan(ysWithNan, 25, 1);
            FillWithNan(ysWithNan, 30, 15);
            ysWithNan[10] = ys[10];

            // add a scatter plot and customize NaN behavior
            var sp2 = plt.AddScatter(xs, ysWithNan, Color.Black);
            sp2.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
            plt.Title($"OnNaN = {sp2.OnNaN}");
        }
    }

    public class ScatterNaNGap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Scatter();
        public string ID => "scatter_nan_gap";
        public string Title => "NaN Values Break the Line";
        public string Description =>
            "When the OnNaN field is set to Gap, points containing NaN X or Y values break the line. " +
            "This results in a scatter plot appearing as multiple lines, with gaps representing missing data.";

        public void ExecuteRecipe(Plot plt)
        {
            // create data that does NOT contain NaN
            double[] xs = ScottPlot.DataGen.Consecutive(51);
            double[] ys = ScottPlot.DataGen.Sin(51);

            // plot it the traditional way
            plt.AddScatter(xs, ys, Color.FromArgb(50, Color.Black));

            // create new data that contains NaN
            double[] ysWithNan = ScottPlot.DataGen.Sin(51);
            static void FillWithNan(double[] values, int start, int count)
            {
                for (int i = 0; i < count; i++)
                    values[start + i] = double.NaN;
            }
            FillWithNan(ysWithNan, 5, 15);
            FillWithNan(ysWithNan, 25, 1);
            FillWithNan(ysWithNan, 30, 15);
            ysWithNan[10] = ys[10];

            // add a scatter plot and customize NaN behavior
            var sp2 = plt.AddScatter(xs, ysWithNan, Color.Black);
            sp2.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            plt.Title($"OnNaN = {sp2.OnNaN}");
        }
    }
}
