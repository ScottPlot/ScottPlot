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
        }

        private void SplineInterpolation_Load(object sender, EventArgs e)
        {
            cbInterpolationType.Items.Add("Bezier");
            cbInterpolationType.Items.Add("CatmullRom");
            cbInterpolationType.Items.Add("Chaikin");
            cbInterpolationType.Items.Add("Cubic");
            cbInterpolationType.SelectedIndex = 0;
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
            // get original points from plottable
            var plottables = formsPlot1.Plot.GetPlottables();
            if (plottables.Length == 0)
                return;
            var sp = (ScottPlot.Plottable.ScatterPlotDraggable)plottables.First();

            // interpolate using the selected method
            double[] interpXs;
            double[] interpYs;

            double frac = (double)trackBar1.Value / trackBar1.Maximum;

            int mult;
            switch (cbInterpolationType.Text)
            {
                case "Bezier":
                    frac = Math.Max(.2 * (1 - frac), .002);
                    (interpXs, interpYs) = ScottPlot.Statistics.Interpolation.Bezier.InterpolateXY(sp.Xs, sp.Ys, frac);
                    break;

                case "CatmullRom":
                    mult = Math.Max((int)(frac * 10), 1);
                    (interpXs, interpYs) = ScottPlot.Statistics.Interpolation.CatmullRom.InterpolateXY(sp.Xs, sp.Ys, mult);
                    break;

                case "Chaikin":
                    mult = Math.Max((int)(frac * 5), 1);
                    (interpXs, interpYs) = ScottPlot.Statistics.Interpolation.Chaikin.InterpolateXY(sp.Xs, sp.Ys, mult);
                    break;

                case "Cubic":
                    mult = Math.Max((int)(frac * 500), 2);
                    (interpXs, interpYs) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(sp.Xs, sp.Ys, mult);
                    break;

                default:
                    label1.Text = $"ERROR";
                    return;

            }

            // update the interpolation plot
            while (formsPlot1.Plot.GetPlottables().Count() > 1)
                formsPlot1.Plot.RemoveAt(1);
            formsPlot1.Plot.AddScatter(interpXs, interpYs);
            formsPlot1.Refresh(skipIfCurrentlyRendering: true);
            label1.Text = $"Points: {interpXs.Length}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateOriginalData();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateInterpolation();
        }

        private void cbInterpolationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterpolation();
        }
    }
}
