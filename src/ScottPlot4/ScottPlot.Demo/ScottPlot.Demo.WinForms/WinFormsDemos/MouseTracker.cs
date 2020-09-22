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
    public partial class MouseTracker : Form
    {
        PlottableVLine vLine;
        PlottableHLine hLine;

        public MouseTracker()
        {
            InitializeComponent();
            formsPlot1.plt.PlotSignal(DataGen.RandomWalk(null, 100));

            vLine = formsPlot1.plt.PlotVLine(0, color: Color.Red, lineStyle: LineStyle.Dash);
            hLine = formsPlot1.plt.PlotHLine(0, color: Color.Red, lineStyle: LineStyle.Dash);

            formsPlot1.Render();
        }

        private void formsPlot1_MouseMoved_1(object sender, MouseEventArgs e)
        {
            int pixelX = e.X;
            int pixelY = e.Y;

            (double coordinateX, double coordinateY) = formsPlot1.GetMouseCoordinates();

            XPixelLabel.Text = $"{e.X:0.000}";
            YPixelLabel.Text = $"{e.X:0.000}";

            XCoordinateLabel.Text = $"{coordinateX:0.00000000}";
            YCoordinateLabel.Text = $"{coordinateY:0.00000000}";

            vLine.position = coordinateX;
            hLine.position = coordinateY;

            formsPlot1.Render(skipIfCurrentlyRendering: true);
        }
    }
}
