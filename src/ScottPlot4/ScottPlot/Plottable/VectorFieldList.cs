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

        /// <summary>
        /// Tuples define location and direction of vectors to display as arrows.
        /// Users may manipulate this List to add/remove their own vectors.
        /// </summary>
        public readonly List<(Coordinate coordinate, CoordinateVector vector)> RootedVectors = new();

        /// <summary>
        /// Advanced configuration options that control how vectors are drawn as arrows
        /// </summary>
        public readonly Renderable.ArrowStyle ArrowStyle = new();

        /// <summary>
        /// Color to draw the arrows (if <see cref="Colormap"/> is null)
        /// </summary>
        public Color Color { get; set; } = Color.Blue;

        /// <summary>
        /// If defined, this colormap is used to color each arrow based on its magnitude
        /// </summary>
        public Colormap Colormap { get; set; } = null;

        /// <summary>
        /// If <see cref="Colormap"/> is defined, each arrow's magnitude 
        /// is run through this function to get the fraction (from 0 to 1) 
        /// along the colormap to sample from.
        /// </summary>
        public Func<double, double> ColormapScaler = (magnitude) => magnitude;

        public VectorFieldList()
        {

        }

        public VectorFieldList(List<(Coordinate coordinate, CoordinateVector vector)> rootedVectors)
        {
            RootedVectors = rootedVectors;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(Label) ? "" : $" ({Label})";
            return $"PlottableVectorField{label} with {RootedVectors.Count} vectors";
        }

        public LegendItem[] GetLegendItems() => LegendItem.None;

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
            foreach ((Coordinate coordinate, CoordinateVector vector) in RootedVectors)
            {
                Color color = Colormap is null
                    ? Color
                    : Colormap.GetColor(ColormapScaler.Invoke(vector.Magnitude));

                ArrowStyle.RenderArrow(dims, gfx, coordinate, vector, color);
            }
        }
    }
}
