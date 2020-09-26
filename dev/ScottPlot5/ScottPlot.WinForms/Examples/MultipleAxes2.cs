using ScottPlot.Renderable;
using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScottPlot.WinForms.Examples
{
    public partial class MultipleAxes2 : Form
    {
        public MultipleAxes2()
        {
            InitializeComponent();
        }

        private void MultipleAxes2_Load(object sender, EventArgs e)
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);
            interactivePlot1.Plot.PlotScatter(xs, ys);
        }

        private void AddYLeft_Click(object sender, EventArgs e)
        {
            var newAxis = new AxisLeft() { Color = Renderer.Color.Random(), Label = "Additional Axis" };
            interactivePlot1.Plot.Axes.Add(newAxis);
            interactivePlot1.Render();
        }

        private void AddYRight_Click(object sender, EventArgs e)
        {
            var newAxis = new AxisRight() { Color = Renderer.Color.Random(), Label = "Additional Axis" };
            interactivePlot1.Plot.Axes.Add(newAxis);
            interactivePlot1.Render();
        }

        private void AddXTop_Click(object sender, EventArgs e)
        {
            var newAxis = new AxisTop() { Color = Renderer.Color.Random(), Label = "Additional Axis" };
            interactivePlot1.Plot.Axes.Add(newAxis);
            interactivePlot1.Render();
        }

        private void AddXBottom_Click(object sender, EventArgs e)
        {
            var newAxis = new AxisBottom() { Color = Renderer.Color.Random(), Label = "Additional Axis" };
            interactivePlot1.Plot.Axes.Add(newAxis);
            interactivePlot1.Render();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            interactivePlot1.Plot.ResetAxisStyles();
            interactivePlot1.Render();
        }
    }
}
