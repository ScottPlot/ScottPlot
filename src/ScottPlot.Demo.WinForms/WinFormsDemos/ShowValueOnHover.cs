using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class ShowValueOnHover : Form
    {
        public ShowValueOnHover()
        {
            InitializeComponent();
        }

        PlottableScatter scatterPlot;
        PlottableScatter highlightedPoint;
        PlottableText highlightedText;

        private void ShowValueOnHover_Load(object sender, EventArgs e)
        {
            int pointCount = 51;
            double[] Xs = DataGen.Consecutive(pointCount);
            double[] Ys = DataGen.Sin(pointCount, 2);
            scatterPlot = formsPlot1.plt.PlotScatter(Xs, Ys, color: Color.Blue);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();

            // create a point and text that will be moved around to highlight the point under the cursor
            highlightedPoint = formsPlot1.plt.PlotPoint(0, 0, markerSize: 10, color: Color.Red);
            highlightedText = formsPlot1.plt.PlotText("asdf", 0, 0, fontSize: 10, color: Color.Black);
        }

        private void formsPlot1_MouseMoved_1(object sender, MouseEventArgs e)
        {
            // don't attempt to change things while we are click-dragging
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
                return;

            // determine where the mouse is in coordinate space
            (double mouseX, double mouseY) = formsPlot1.GetMouseCoordinates();

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

                label1.Text = $"Mouse is over point index {closestIndex} ({x}, {y})";
            }
            else
            {
                highlightedPoint.visible = false;
                highlightedText.visible = false;
                label1.Text = "Mouse is not over a point";
            }

            formsPlot1.Render();
        }
    }
}
