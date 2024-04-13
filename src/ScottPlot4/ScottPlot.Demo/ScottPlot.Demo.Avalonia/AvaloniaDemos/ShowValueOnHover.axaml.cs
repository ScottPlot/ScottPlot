using System;

using Avalonia.Controls;
using Avalonia.Input;

using ScottPlot.Plottable;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class ShowValueOnHover : Window
    {
        private readonly ScatterPlot MyScatterPlot;
        private readonly MarkerPlot HighlightedPoint;
        private int LastHighlightedIndex = -1;

        public ShowValueOnHover()
        {
            this.InitializeComponent();

            // create a scatter plot from some random data and save it
            Random rand = new Random(0);
            const int pointCount = 20;
            double[] xs = DataGen.Random(rand, pointCount);
            double[] ys = DataGen.Random(rand, pointCount, multiplier: 1_000);
            MyScatterPlot = avaPlot1.Plot.AddScatterPoints(xs, ys);

            // Add a red circle we can move around later as a highlighted point indicator
            HighlightedPoint = avaPlot1.Plot.AddPoint(0, 0);
            HighlightedPoint.Color = System.Drawing.Color.Red;
            HighlightedPoint.MarkerSize = 10;
            HighlightedPoint.MarkerShape = ScottPlot.MarkerShape.openCircle;
            HighlightedPoint.IsVisible = false;

            avaPlot1.PointerMoved += MouseMove;
        }

        private void MouseMove(object sender, PointerEventArgs e)
        {
            // determine point nearest the cursor
            (double mouseCoordX, double mouseCoordY) = avaPlot1.GetMouseCoordinates();
            double xyRatio = avaPlot1.Plot.XAxis.Dims.PxPerUnit / avaPlot1.Plot.YAxis.Dims.PxPerUnit;
            (double pointX, double pointY, int pointIndex) = MyScatterPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);

            // place the highlight over the point of interest
            HighlightedPoint.X = pointX;
            HighlightedPoint.Y = pointY;
            HighlightedPoint.IsVisible = true;

            // render if the highlighted point chnaged
            if (LastHighlightedIndex != pointIndex)
            {
                LastHighlightedIndex = pointIndex;
                avaPlot1.Refresh();
            }

            // update the GUI to describe the highlighted point
            (double mouseX, double mouseY) = avaPlot1.GetMouseCoordinates();
            this.label1.Text = $"Closest point to ({mouseX:N0}, {mouseY:N0}) " +
                $"is index {pointIndex} ({pointX:N2}, {pointY:N2})";
        }
    }
}
