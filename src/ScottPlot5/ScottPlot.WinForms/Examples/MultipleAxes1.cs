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
    public partial class MultipleAxes1 : Form
    {
        public MultipleAxes1()
        {
            InitializeComponent();
        }

        private void MultipleAxes1_Load(object sender, EventArgs e)
        {

            double[] xs = Generate.Consecutive(51);
            double[] ys1 = Generate.Sin(51, mult: 1e3);
            double[] ys2 = Generate.Cos(51, mult: 1e-3);

            // customize styling of the default axes
            interactivePlot1.Plot.AxisLeft.Label = "Primary Y Axis";
            interactivePlot1.Plot.AxisLeft.Color = Colors.Red;
            interactivePlot1.Plot.AxisBottom.Label = "Primary X Axis";
            interactivePlot1.Plot.AxisTop.Label = "Multiple Y Axis Demo"; // appears as title

            // create scatter plots and give them to different Y axis indexes
            var scatter1 = interactivePlot1.Plot.PlotScatter(xs, ys1);
            scatter1.YAxisIndex = 0;
            scatter1.Color = Colors.Red;

            var scatter2 = interactivePlot1.Plot.PlotScatter(xs, ys2);
            scatter2.YAxisIndex = 1;
            scatter2.Color = Colors.Blue;

            // add a second Y axis
            var secondYAxis = new AxisLeft()
            {
                Label = "Secondary Y Axis",
                YAxisIndex = 1,
                Color = Colors.Blue
            };

            interactivePlot1.Plot.AddAxis(secondYAxis);
        }

        private void MouseControlY1_CheckedChanged(object sender, EventArgs e) => SetActiveAxesBasedOnCheckboxes();
        private void MouseControlY2_CheckedChanged(object sender, EventArgs e) => SetActiveAxesBasedOnCheckboxes();
        private void MouseControlX_CheckedChanged(object sender, EventArgs e) => SetActiveAxesBasedOnCheckboxes();

        private void SetActiveAxesBasedOnCheckboxes()
        {
            List<int> activeXs = new List<int>();
            List<int> activeYs = new List<int>();

            if (MouseControlX.Checked) activeXs.Add(0);
            if (MouseControlY1.Checked) activeYs.Add(0);
            if (MouseControlY2.Checked) activeYs.Add(1);

            interactivePlot1.MouseTracker.SetActiveAxes(activeXs.ToArray(), activeYs.ToArray());
        }
    }
}
