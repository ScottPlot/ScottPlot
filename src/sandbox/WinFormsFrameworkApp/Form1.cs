using ScottPlot.Plottable;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        private readonly ScatterPlot MyScatterPlot;
        private readonly ScatterPlot HighlightedPoint;
        private int LastHighlightedIndex = -1;

        public Form1()
        {
            InitializeComponent();

            // create some random data to display
            Random rand = new Random(0);
            int pointCount = 20;
            double[] xs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(rand, pointCount);

            // Add a scatter plot and a point (keeps the objects that are created)
            MyScatterPlot = formsPlot1.Plot.AddScatterPoints(xs, ys);
            HighlightedPoint = formsPlot1.Plot.AddPoint(0, 0);
            HighlightedPoint.Color = Color.Red;
            HighlightedPoint.MarkerSize = 10;
            HighlightedPoint.MarkerShape = ScottPlot.MarkerShape.openCircle;
            HighlightedPoint.IsVisible = false;
        }

        private void FormsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            // determine where the cursor is
            double mousePixelX = e.X;
            double mousePixelY = e.Y;
            (double mouseCoordX, double mouseCoordY) = formsPlot1.GetMouseCoordinates();

            // determine point nearest the cursor
            (double pointCoordX, double pointCoordY, int pointIndex) = MyScatterPlot.GetPointNearest(mouseCoordX, mouseCoordY);
            (double pointPixelX, double pointPixelY) = formsPlot1.Plot.GetPixel(pointCoordX, pointCoordY);

            // highlight the point of interest
            HighlightedPoint.Xs[0] = pointCoordX;
            HighlightedPoint.Ys[0] = pointCoordY;
            HighlightedPoint.IsVisible = true;

            // display details in GUI
            tbCursorCoord.Text = $"{mouseCoordX:N1}, {mouseCoordY:N1}";
            tbCursorPixel.Text = $"{mousePixelX:N1}, {mousePixelY:N1}";
            tbPointCoord.Text = $"{pointCoordX:N1}, {pointCoordY:N1}";
            tbPointPixel.Text = $"{pointPixelX:N1}, {pointPixelY:N1}";

            // render if the highlighted point chnaged
            if (LastHighlightedIndex != pointIndex)
            {
                LastHighlightedIndex = pointIndex;
                formsPlot1.Render();
            }
        }

        private void btnSimilar_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int pointCount = 20;
            double[] newXs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] newYs = ScottPlot.DataGen.Random(rand, pointCount);
            MyScatterPlot.Update(newXs, newYs);
            HighlightedPoint.IsVisible = false;
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Render();
        }

        private void btnDifferent_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int pointCount = 20;
            double[] newXs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] newYs = ScottPlot.DataGen.Random(rand, pointCount, multiplier: 1000);
            MyScatterPlot.Update(newXs, newYs);
            HighlightedPoint.IsVisible = false;
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Render();
        }
    }
}
