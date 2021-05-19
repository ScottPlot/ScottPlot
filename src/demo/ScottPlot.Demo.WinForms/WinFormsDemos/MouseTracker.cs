using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot.Plottable;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class MouseTracker : Form
    {
        private readonly VLine vLine;
        private readonly HLine hLine;

        public MouseTracker()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(DataGen.RandomWalk(null, 100));
            vLine = formsPlot1.Plot.AddVerticalLine(0, Color.Red, 1, LineStyle.Dash);
            hLine = formsPlot1.Plot.AddHorizontalLine(0, Color.Red, 1, LineStyle.Dash);
        }

        private void formsPlot1_MouseMoved_1(object sender, MouseEventArgs e)
        {
            (double coordinateX, double coordinateY) = formsPlot1.GetMouseCoordinates();

            XPixelLabel.Text = $"{e.X:0.000}";
            YPixelLabel.Text = $"{e.Y:0.000}";

            XCoordinateLabel.Text = $"{coordinateX:0.00000000}";
            YCoordinateLabel.Text = $"{coordinateY:0.00000000}";

            vLine.X = coordinateX;
            hLine.Y = coordinateY;

            formsPlot1.Render();
        }

        private void formsPlot1_MouseEnter(object sender, EventArgs e)
        {
            lblMouse.Text = "Mouse ENTERED the plot";
            hLine.IsVisible = true;
            vLine.IsVisible = true;
        }

        private void formsPlot1_MouseLeave(object sender, EventArgs e)
        {
            lblMouse.Text = "Mouse LEFT the plot";

            XPixelLabel.Text = $"--";
            YPixelLabel.Text = $"--";
            XCoordinateLabel.Text = $"--";
            YCoordinateLabel.Text = $"--";

            hLine.IsVisible = false;
            vLine.IsVisible = false;
            formsPlot1.Render();
        }
    }
}
