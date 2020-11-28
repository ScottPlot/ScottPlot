using ScottPlot.Plottable;
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

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        private readonly HLine HLine;
        private readonly VLine VLine;
        private readonly SignalPlot Signal;

        public Form1()
        {
            InitializeComponent();

            // simulate 10 seconds of 48 kHz audio
            int sampleRate = 48_000;
            Random rand = new Random(0);
            double[] data = ScottPlot.DataGen.RandomWalk(rand, sampleRate * 10);
            Signal = formsPlot1.plt.PlotSignal(data, sampleRate);

            // markers to indicate where the mouse is
            HLine = formsPlot1.plt.PlotHLine(0, Color.Red, lineStyle: ScottPlot.LineStyle.Dash);
            VLine = formsPlot1.plt.PlotVLine(0, Color.Red, lineStyle: ScottPlot.LineStyle.Dash);

            formsPlot1.Render();
        }

        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            double mouseX = formsPlot1.plt.CoordinateFromPixelX(e.X);
            double mouseY = formsPlot1.plt.CoordinateFromPixelY(e.Y);

            (double pointX, double pointY, int pointIndex) = Signal.GetPointNearestX(mouseX);
            Text = $"Mouse is over point {pointIndex} ({pointX}, {pointY})";

            HLine.position = pointY;
            VLine.position = pointX;
            formsPlot1.Render();
        }
    }
}
