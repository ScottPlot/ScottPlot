using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ScottPlot.Plottable;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for ShowValueOnHover.xaml
    /// </summary>
    public partial class ShowValueOnHover : Window
    {
        private readonly ScatterPlot MyScatterPlot;
        private readonly MarkerPlot HighlightedPoint;
        private int LastHighlightedIndex = -1;

        public ShowValueOnHover()
        {
            InitializeComponent();

            // create a scatter plot from some random data and save it
            Random rand = new Random(0);
            int pointCount = 20;
            double[] xs = DataGen.Random(rand, pointCount);
            double[] ys = DataGen.Random(rand, pointCount, multiplier: 1_000);
            MyScatterPlot = wpfPlot1.Plot.AddScatterPoints(xs, ys);

            // Add a red circle we can move around later as a highlighted point indicator
            HighlightedPoint = wpfPlot1.Plot.AddPoint(0, 0);
            HighlightedPoint.Color = System.Drawing.Color.Red;
            HighlightedPoint.MarkerSize = 10;
            HighlightedPoint.MarkerShape = ScottPlot.MarkerShape.openCircle;
            HighlightedPoint.IsVisible = false;

            // perform an initial render
            wpfPlot1.Refresh();
        }

        private void wpfPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            // determine point nearest the cursor
            (double mouseCoordX, double mouseCoordY) = wpfPlot1.GetMouseCoordinates();
            double xyRatio = wpfPlot1.Plot.XAxis.Dims.PxPerUnit / wpfPlot1.Plot.YAxis.Dims.PxPerUnit;
            (double pointX, double pointY, int pointIndex) = MyScatterPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);

            // place the highlight over the point of interest
            HighlightedPoint.X = pointX;
            HighlightedPoint.Y = pointY;
            HighlightedPoint.IsVisible = true;

            // render if the highlighted point chnaged
            if (LastHighlightedIndex != pointIndex)
            {
                LastHighlightedIndex = pointIndex;
                wpfPlot1.Refresh();
            }

            // update the GUI to describe the highlighted point
            double mouseX = e.GetPosition(this).X;
            double mouseY = e.GetPosition(this).Y;
            label1.Content = $"Closest point to ({mouseX:N2}, {mouseY:N2}) " +
                $"is index {pointIndex} ({pointX:N2}, {pointY:N2})";
        }
    }
}
