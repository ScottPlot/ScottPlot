using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class MarkerQuickstart : IRecipe
    {
        public string Category => "Plottable: Marker";
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
        public string Category => "Plottable: Marker";
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
                MarkerSize = 25,
            };

            plt.Add(myDraggableMarker);
        }
    }
}
