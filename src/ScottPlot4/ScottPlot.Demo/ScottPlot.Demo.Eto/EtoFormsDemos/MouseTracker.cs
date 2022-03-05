using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using ScottPlot.Plottable;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class MouseTracker : Form
    {
        private readonly Crosshair Crosshair;

        public MouseTracker()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(DataGen.RandomWalk(null, 100));
            Crosshair = formsPlot1.Plot.AddCrosshair(0, 0);

            formsPlot1.MouseEnter += formsPlot1_MouseEnter;
            formsPlot1.MouseLeave += formsPlot1_MouseLeave;
            this.Content.MouseMove += formsPlot1_MouseMoved_1;
        }

        private void formsPlot1_MouseMoved_1(object sender, MouseEventArgs e)
        {
            this.Content.SuspendLayout();

            (double coordinateX, double coordinateY) = formsPlot1.GetMouseCoordinates();

            XPixelLabel.Text = $"{e.Location.X:0.000}";
            YPixelLabel.Text = $"{e.Location.Y:0.000}";

            XCoordinateLabel.Text = $"{coordinateX:0.00000000}";
            YCoordinateLabel.Text = $"{coordinateY:0.00000000}";

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            formsPlot1.Refresh();

            this.Content.ResumeLayout();
        }

        private void formsPlot1_MouseEnter(object sender, EventArgs e)
        {
            lblMouse.Text = "Mouse ENTERED the plot";
            Crosshair.IsVisible = true;
        }

        private void formsPlot1_MouseLeave(object sender, EventArgs e)
        {
            lblMouse.Text = "Mouse LEFT the plot";

            XPixelLabel.Text = $"--";
            YPixelLabel.Text = $"--";
            XCoordinateLabel.Text = $"--";
            YCoordinateLabel.Text = $"--";

            Crosshair.IsVisible = false;
            formsPlot1.Refresh();
        }
    }
}
