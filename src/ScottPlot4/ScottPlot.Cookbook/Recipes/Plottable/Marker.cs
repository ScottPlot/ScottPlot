using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class MarkerQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Marker();
        public string ID => "marker_quickstart";
        public string Title => "Marker";
        public string Description =>
            "You can place individual markers anywhere on the plot. ";
        public void ExecuteRecipe(Plot plt)
        {
            var colormap = ScottPlot.Drawing.Colormap.Turbo;
            Random rand = new(0);
            for (int i = 0; i < 100; i++)
            {
                plt.AddMarker(
                    x: rand.NextDouble(),
                    y: rand.NextDouble(),
                    size: 5 + rand.NextDouble() * 20,
                    shape: Marker.Random(rand),
                    color: colormap.RandomColor(rand));
            }
        }
    }

    public class MarkerDraggable : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Marker();
        public string ID => "marker_draggable";
        public string Title => "Draggable Marker";
        public string Description =>
            "A special type of marker exists which allows dragging with the mouse.";
        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            var myDraggableMarker = new ScottPlot.Plottable.DraggableMarkerPlot()
            {
                X = 25,
                Y = .57,
                Color = Color.Magenta,
                MarkerShape = MarkerShape.filledDiamond,
                MarkerSize = 15,
                Text = "drag the point!",
            };

            myDraggableMarker.TextFont.Size = 16;

            plt.Add(myDraggableMarker);
        }
    }

    public class MarkerDraggableInVector : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Marker();
        public string ID => "marker_draggableinvector";
        public string Title => "Draggable Marker Snap";
        public string Description =>
            "This is a type of marker which can be dragged with the mouse, " +
            "but is restricted to to X/Y positions defined by two arrays.";
        public void ExecuteRecipe(Plot plt)
        {
            // create random data and display it with a scatter plot
            double[] xs = DataGen.Consecutive(50);
            double[] ys = DataGen.Random(new Random(0), 50);
            plt.AddScatter(xs, ys, label: "data");

            // add a draggable marker that "snaps" to data values in that scatter plot
            var dmpv = new ScottPlot.Plottable.DraggableMarkerPlotInVector()
            {
                Xs = xs,
                Ys = ys,
                DragEnabled = true,
                IsVisible = true,
                MarkerSize = 15,
                MarkerShape = MarkerShape.filledDiamond,
                MarkerColor = Color.Magenta,
                Label = "marker",
            };
            plt.Add(dmpv);

            plt.Legend();
        }
    }

    public class MarkerLabeled : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Marker();
        public string ID => "marker_labeled";
        public string Title => "Labeled Marker";
        public string Description =>
            "Markers have an optional text label.";
        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));

            var marker = plt.AddMarker(35, 0.6);
            marker.Text = "Interesting Point";
            marker.TextFont.Color = Color.Magenta;
            marker.TextFont.Alignment = Alignment.UpperCenter;
            marker.TextFont.Size = 28;
        }
    }

    public class MarkerLineWidth : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Marker();
        public string ID => "marker_linewidth";
        public string Title => "Marker Line Width";
        public string Description =>
            "Markers have options that can be customized, such as line width.";
        public void ExecuteRecipe(Plot plt)
        {
            double[] ys1 = DataGen.Sin(30);
            var cmap1 = ScottPlot.Drawing.Colormap.Viridis;

            double[] ys2 = DataGen.Cos(30);
            var cmap2 = ScottPlot.Drawing.Colormap.Turbo;

            for (int i = 0; i < ys1.Length; i++)
            {
                double frac = i / (ys1.Length - 1f);

                var circle = plt.AddMarker(i, ys1[i]);
                circle.MarkerShape = MarkerShape.openCircle;
                circle.MarkerSize = i + 5;
                circle.MarkerLineWidth = 1 + i / 2;
                circle.MarkerColor = cmap1.GetColor(1 - frac, .8);

                var triangle = plt.AddMarker(i, ys2[i]);
                triangle.MarkerShape = MarkerShape.openTriangleUp;
                triangle.MarkerSize = i + 5;
                triangle.MarkerLineWidth = 1 + i / 4;
                triangle.MarkerColor = cmap2.GetColor(frac, .8);
            }

            plt.Margins(.2, .2);
        }
    }
}
