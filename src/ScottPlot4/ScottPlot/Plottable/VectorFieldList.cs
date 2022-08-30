using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// The VectorField displays arrows representing a 2D array of 2D vectors
    /// </summary>
    public class VectorFieldList : IPlottable
    {
        public string Label { get; set; } = null;
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public readonly List<(Coordinate coordinate, CoordinateVector vector)> RootedVectors = new();

        public readonly Renderable.ArrowStyle ArrowStyle = new();

        public VectorFieldList()
        {

        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(Label) ? "" : $" ({Label})";
            return $"PlottableVectorField{label} with {RootedVectors.Count} vectors";
        }

        public LegendItem[] GetLegendItems() => Array.Empty<LegendItem>();

        public void ValidateData(bool deep = false) { }

        public AxisLimits GetAxisLimits()
        {
            if (!RootedVectors.Any())
                return AxisLimits.NoLimits;
            else
                return new AxisLimits(
                    xMin: RootedVectors.Select(x => x.coordinate.X - x.vector.X).Min(),
                    xMax: RootedVectors.Select(x => x.coordinate.X + x.vector.X).Max(),
                    yMin: RootedVectors.Select(x => x.coordinate.Y - x.vector.Y).Min(),
                    yMax: RootedVectors.Select(x => x.coordinate.Y + x.vector.Y).Max());
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            foreach (var arrow in RootedVectors)
            {
                Color color = Color.Blue;
                ArrowStyle.RenderArrow(dims, gfx, arrow.coordinate, arrow.vector, color);
            }
        }
    }
}
