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
        public MouseTracker()
        {
            InitializeComponent();
            formsPlot1.plt.PlotSignal(DataGen.RandomWalk(null, 100));
            formsPlot1.Render();
        }

        private void formsPlot1_MouseMoved(object sender, EventArgs e)
        {
            double pixelX = formsPlot1.PointToClient(Cursor.Position).X;
            double pixelY = formsPlot1.PointToClient(Cursor.Position).Y;

            XPixelLabel.Text = $"{pixelX:0.000}";
            YPixelLabel.Text = $"{pixelY:0.000}";

            XCoordinateLabel.Text = $"{formsPlot1.plt.CoordinateFromPixelX(pixelX):0.00000000}";
            YCoordinateLabel.Text = $"{formsPlot1.plt.CoordinateFromPixelY(pixelY):0.00000000}";
        }
    }
}
