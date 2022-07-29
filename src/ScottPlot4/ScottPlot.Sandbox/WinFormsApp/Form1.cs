using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        Random Rand = new(0);
        double[] Xs;
        double[] Ys;
        ScottPlot.Plottable.MarkerPlot Marker;
        double SnapDistancePx = 50;

        public Form1()
        {
            InitializeComponent();
            (Xs, Ys) = DataGen.RandomWalk2D(Rand, 50);
            formsPlot1.Plot.AddScatter(Xs, Ys);
            Marker = formsPlot1.Plot.AddMarker(0, 0, MarkerShape.openCircle, 20, Color.Red);
            formsPlot1.LeftClicked += FormsPlot1_LeftClicked;
            formsPlot1.Plot.Title("Waiting for click...");
            formsPlot1.Refresh();
        }

        private void FormsPlot1_LeftClicked(object sender, EventArgs e)
        {
            (double mousePixelX, double mousePixelY) = formsPlot1.GetMousePixel();
            (double mouseX, double mouseY) = formsPlot1.GetMouseCoordinates();

            // determine the point in the scatter plot closest to the mouse
            double closestDistance = double.PositiveInfinity;
            int closestIndex = 0;
            for (int i = 0; i < Xs.Length; i++)
            {
                (double x, double y) = formsPlot1.Plot.GetPixel(Xs[i], Ys[i]);
                double dX = mousePixelX - x;
                double dY = mousePixelY - y;
                double distance = Math.Sqrt(dX * dX + dY * dY);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            // take action based on whether the click engaged a point
            if (closestDistance < SnapDistancePx)
            {
                double x = Xs[closestIndex];
                double y = Ys[closestIndex];
                formsPlot1.Plot.Title($"Clicked point [{closestIndex}] (X={x:0.00}, Y={y:0.00})");
                Marker.IsVisible = true;
                Marker.X = x;
                Marker.Y = y;
            }
            else
            {
                formsPlot1.Plot.Title($"Clicked empty space (X={mouseX:0.00}, Y={mouseY:0.00})");
                Marker.IsVisible = false;
            }

            formsPlot1.Refresh();
        }
    }
}
