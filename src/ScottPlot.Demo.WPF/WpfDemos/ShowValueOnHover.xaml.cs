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

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for ShowValueOnHover.xaml
    /// </summary>
    public partial class ShowValueOnHover : Window
    {
        PlottableScatter scatterPlot;
        PlottableScatter highlightedPoint;
        PlottableText highlightedText;

        public ShowValueOnHover()
        {
            InitializeComponent();

            // plot some data
            int pointCount = 51;
            double[] Xs = DataGen.Consecutive(pointCount);
            double[] Ys = DataGen.Sin(pointCount, 2);
            scatterPlot = wpfPlot1.plt.PlotScatter(Xs, Ys, color: System.Drawing.Color.Blue);
            wpfPlot1.plt.AxisAuto();
            wpfPlot1.Render();

            // create a point and text that will be moved around to highlight the point under the cursor
            highlightedPoint = wpfPlot1.plt.PlotPoint(0, 0, markerSize: 10, color: System.Drawing.Color.Red);
            highlightedText = wpfPlot1.plt.PlotText("asdf", 0, 0, fontSize: 10, color: System.Drawing.Color.Black);
            highlightedPoint.visible = false;
            highlightedText.visible = false;
        }

        private void wpfPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            // don't attempt to change things while we are click-dragging
            if (e.LeftButton == MouseButtonState.Pressed) return;
            if (e.RightButton == MouseButtonState.Pressed) return;
            if (e.MiddleButton == MouseButtonState.Pressed) return;

            // determine where the mouse is in coordinate space
            int pixelX = (int)e.MouseDevice.GetPosition(wpfPlot1).X;
            int pixelY = (int)e.MouseDevice.GetPosition(wpfPlot1).Y;
            (double mouseX, double mouseY) = wpfPlot1.GetMouseCoordinates();

            // determine which point is closest to the mouse
            int closestIndex = 0;
            double closestDistance = double.PositiveInfinity;
            for (int i = 0; i < scatterPlot.ys.Length; i++)
            {
                double dx = scatterPlot.xs[i] - mouseX;
                double dy = scatterPlot.ys[i] - mouseY;
                double distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }

            if (closestDistance < 1)
            {
                double x = scatterPlot.xs[closestIndex];
                double y = scatterPlot.ys[closestIndex];

                highlightedPoint.visible = true;
                highlightedPoint.xs[0] = x;
                highlightedPoint.ys[0] = y;

                highlightedText.visible = true;
                highlightedText.text = $"  ({Math.Round(x, 3)}, {Math.Round(y, 3)})";
                highlightedText.x = x;
                highlightedText.y = y;

                label1.Content = $"Mouse is over point index {closestIndex} ({x}, {y})";
            }
            else
            {
                highlightedPoint.visible = false;
                highlightedText.visible = false;
                label1.Content = "Mouse is not over a point";
            }

            wpfPlot1.Render();
        }
    }
}
