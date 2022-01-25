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
    public partial class SplineInterpolation : Form
    {
        public SplineInterpolation()
        {
            InitializeComponent();

            UpdateOriginalData();
        }

        private void UpdateOriginalData(int count = 20)
        {
            Random Rand = new Random();
            double[] xs = ScottPlot.DataGen.RandomWalk(Rand, count);
            double[] ys = ScottPlot.DataGen.RandomWalk(Rand, count);

            var sp = new ScottPlot.Plottable.ScatterPlotDraggable(xs, ys);
            sp.MarkerSize = 10;
            sp.DragEnabled = true;
            sp.Color = formsPlot1.Plot.Palette.GetColor(0);
            sp.Dragged += OnPointDragged;

            formsPlot1.Plot.Clear();
            formsPlot1.Plot.Add(sp);

            UpdateInterpolation();
        }

        private void OnPointDragged(object sender, EventArgs e)
        {
            UpdateInterpolation();
        }

        private void UpdateInterpolation()
        {
            var sp = (ScottPlot.Plottable.ScatterPlotDraggable)formsPlot1.Plot.GetPlottables().First();

            (double[] interpXs, double[] interpYs) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(sp.Xs, sp.Ys, trackBar1.Value);

            while (formsPlot1.Plot.GetPlottables().Count() > 1)
                formsPlot1.Plot.RemoveAt(1);

            formsPlot1.Plot.AddScatter(interpXs, interpYs);
            formsPlot1.Refresh(skipIfCurrentlyRendering: true);
            label1.Text = $"Points: {trackBar1.Value}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateOriginalData();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateInterpolation();
        }
    }
}
