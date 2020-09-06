using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class ShowValueOnHover2 : Form
    {
        public ShowValueOnHover2()
        {
            InitializeComponent();
        }

        ScottPlot.PlottableScatterHighlight sph;
        private void ShowValueOnHover2_Load(object sender, EventArgs e)
        {
            int pointCount = 100;
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(pointCount, 0.1);
            double[] ys = DataGen.NoisySin(rand, pointCount);

            sph = formsPlot1.plt.PlotScatterHighlight(xs, ys);
            formsPlot1.Render();
        }

        ToolTip tooltip = new ToolTip();
        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            double mouseX = formsPlot1.plt.CoordinateFromPixelX(e.Location.X);
            double mouseY = formsPlot1.plt.CoordinateFromPixelY(e.Location.Y);

            sph.HighlightClear();
            var (x, y, index) = sph.HighlightPointNearest(mouseX, mouseY);
            formsPlot1.Render();

            label1.Text = $"Closest point to ({mouseX:N2}, {mouseY:N2}) " +
                $"is index {index} ({x:N2}, {y:N2})";

            if (cbTooltip.Checked)
            {
                PointF highlightedPoint = formsPlot1.plt.CoordinateToPixel(x, y);
                double dX = e.Location.X - highlightedPoint.X;
                double dY = e.Location.Y - highlightedPoint.Y;
                double distanceToPoint = Math.Sqrt(dX * dX + dY * dY);
                if (distanceToPoint < 15)
                    tooltip.Show($"{x}, {y}", this,
                        (int)highlightedPoint.X + formsPlot1.Location.X,
                        (int)highlightedPoint.Y + formsPlot1.Location.Y);
                else
                    tooltip.Hide(this);
            }
        }
    }
}
