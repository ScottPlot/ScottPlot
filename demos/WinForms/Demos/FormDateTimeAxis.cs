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
    public partial class FormDateTimeAxis : Form
    {
        public FormDateTimeAxis()
        {
            InitializeComponent();
        }

        private void SamplePlotWithDateAxis()
        {
            formsPlot1.plt.Ticks(dateTimeX: cbHorizontal.Checked);
            Random rand = new Random();
            formsPlot1.plt.PlotSignal(
                ys: ScottPlot.DataGen.RandomWalk(rand, 100_000), // 100k points
                sampleRate: 1000, // 1k points per day
                xOffset: 43787 // 2019
                );
            formsPlot1.plt.YLabel("awesomeness");
            formsPlot1.plt.XLabel("experimental date");
            formsPlot1.Render();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //SamplePlotWithDateAxis();
            SamplePlotChris();
        }

        private void SamplePlotChris()
        {
            // https://github.com/swharden/ScottPlot/issues/269

            int pointCount = 10;
            double[] ys = ScottPlot.DataGen.RandomWalk(null, pointCount, 100);
            double[] xs = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                xs[i] = DateTime.Now.AddSeconds(i).ToOADate();

            formsPlot1.plt.PlotScatter(xs, ys);
            formsPlot1.plt.Ticks(dateTimeX: true);
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void formsPlot1_MouseMoved(object sender, EventArgs e)
        {
            Point mousePixel = formsPlot1.PointToClient(Cursor.Position);
            double xCoordiante = formsPlot1.plt.CoordinateFromPixelX(mousePixel.X);
            double yCoordinate = formsPlot1.plt.CoordinateFromPixelY(mousePixel.Y);
            DateTime dt = DateTime.FromOADate(xCoordiante);
            mouseTextbox.Text = $"Time (X) = {dt}\r\nValue (Y) = {yCoordinate}";
        }

        private void CbHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(dateTimeX: cbHorizontal.Checked);
            formsPlot1.Render();
        }

        private void CbVertical_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(dateTimeY: cbVertical.Checked);
            formsPlot1.Render();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // DateTime can't handle negative years!
            formsPlot1.plt.Axis(
                new DateTime(100, 1, 1).ToOADate(), 
                new DateTime(5000, 1, 1).ToOADate());
            formsPlot1.Render();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(1492, 1, 1).ToOADate(), 
                new DateTime(1985, 1, 1).ToOADate());
            formsPlot1.Render();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(1985, 9, 24).ToOADate(), 
                new DateTime(2020, 1, 1).ToOADate());
            formsPlot1.Render();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(2019, 9, 24).ToOADate(), 
                new DateTime(2020, 1, 1).ToOADate());
            formsPlot1.Render();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(2019, 9, 24).ToOADate(), 
                new DateTime(2019, 9, 30).ToOADate());
            formsPlot1.Render();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(2019, 9, 24).ToOADate(), 
                new DateTime(2019, 9, 25).ToOADate());
            formsPlot1.Render();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(2019, 9, 24, 12, 00, 00).ToOADate(), 
                new DateTime(2019, 9, 24, 20, 00, 00).ToOADate());
            formsPlot1.Render();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(2019, 9, 24, 12, 00, 00).ToOADate(), 
                new DateTime(2019, 9, 24, 12, 10, 00).ToOADate());
            formsPlot1.Render();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            formsPlot1.plt.Axis(
                new DateTime(2019, 9, 24, 12, 34, 56).ToOADate(), 
                new DateTime(2019, 9, 24, 12, 34, 57).ToOADate());
            formsPlot1.Render();
        }
    }
}
