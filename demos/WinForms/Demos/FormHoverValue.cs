using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormHoverValue : Form
    {
        public FormHoverValue()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int pointCount = 50;
            double[] Ys = ScottPlot.DataGen.Sin(pointCount, 2);
            double[] Xs = ScottPlot.DataGen.Consecutive(pointCount);
            scottPlotUC1.plt.PlotScatter(Xs, Ys, color: Color.Blue);
            scottPlotUC1.plt.PlotPoint(Xs[10], Ys[10], markerSize: 10, color: Color.Red);
            scottPlotUC1.plt.PlotText("asdf", Xs[10], Ys[10], fontSize: 10, color: Color.Black);
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void UpdateHover()
        {
            var plottables = scottPlotUC1.plt.GetPlottables();
            var scatterPlot = (PlottableScatter)plottables[0];
            var highlightScatter = (PlottableScatter)plottables[1];
            var highlightText = (PlottableText)plottables[2];

            // get mouse position on the screen
            Point mouseLoc = new Point(Cursor.Position.X, Cursor.Position.Y);

            // modify it to be mouse position on the ScottPlot
            mouseLoc.X -= this.PointToScreen(scottPlotUC1.Location).X;
            mouseLoc.Y -= this.PointToScreen(scottPlotUC1.Location).Y;

            // determine the position on the screen
            //PointF mousePos = scottPlotUC1.plt.CoordinateFromPixel(mouseLoc);

            // determine which scatter point is closest to the mouse
            int closestIndex = 0;
            double closestDistance = double.PositiveInfinity;
            for (int i = 0; i < scatterPlot.ys.Length; i++)
            {
                double dx = mouseLoc.X - scottPlotUC1.plt.CoordinateToPixel(scatterPlot.xs[i], 0).X;
                double dy = mouseLoc.Y - scottPlotUC1.plt.CoordinateToPixel(0, scatterPlot.ys[i]).Y;
                double distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (closestIndex < 0)
                {
                    closestDistance = distance;
                }
                else if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            highlightText.x = scatterPlot.xs[closestIndex];
            highlightText.y = scatterPlot.ys[closestIndex];
            highlightScatter.xs[0] = scatterPlot.xs[closestIndex];
            highlightScatter.ys[0] = scatterPlot.ys[closestIndex];

            if (closestDistance < 20)
            {
                highlightText.text = string.Format(
                        "   ({0}, {1})",
                        Math.Round(scatterPlot.xs[closestIndex], 3),
                        Math.Round(scatterPlot.ys[closestIndex], 3)
                    );
                highlightScatter.markerSize = 10;
            }
            else
            {
                highlightText.text = "";
                highlightScatter.markerSize = 0;
            }
            scottPlotUC1.Render();
        }

        private void ScottPlotUC1_MouseMoved(object sender, EventArgs e)
        {
            UpdateHover();
        }
    }
}
