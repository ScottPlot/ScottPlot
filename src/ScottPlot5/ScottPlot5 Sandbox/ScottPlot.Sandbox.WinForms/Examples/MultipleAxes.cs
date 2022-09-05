using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace WinForms.Examples
{
    public partial class MultipleAxes : Form, IExampleForm
    {
        public string SandboxTitle => "Multiple Axes";
        public string SandboxDescription => "Plot data with greatly different scales using separate axes";

        public MultipleAxes()
        {
            InitializeComponent();
            btnAddAxis.Click += (s, e) => AddYAxis();
            btnRemoveAxis.Click += (s, e) => RemoveYAxis();

            var sig = formsPlot1.Plot.Plottables.AddSignal(ScaledSine());
            formsPlot1.Plot.YAxis.Label.Text = $"Y Axis 1";
            formsPlot1.Plot.XAxis.Label.Text = $"X Axis 1";
            formsPlot1.Plot.YAxis.Label.Color = sig.Color;
            formsPlot1.Plot.XAxis.Label.Color = sig.Color;

            formsPlot1.Refresh();
            SetButtonEnabledStates();
        }

        private void SetButtonEnabledStates()
        {
            btnAddAxis.Enabled = formsPlot1.Plot.YAxes.Count <= 3;
            btnRemoveAxis.Enabled = formsPlot1.Plot.YAxes.Count > 1;
        }

        private void AddYAxis()
        {
            // create a new axis and add it to the plot
            ScottPlot.Axis.IYAxis yAxis = checkBox1.Checked
                ? new ScottPlot.Axis.StandardAxes.RightAxis()
                : new ScottPlot.Axis.StandardAxes.LeftAxis();

            formsPlot1.Plot.YAxes.Add(yAxis);
            yAxis.Label.Text = $"Y Axis {formsPlot1.Plot.YAxes.Count()}";

            ScottPlot.Axis.IXAxis xAxis = checkBox1.Checked
                ? new ScottPlot.Axis.StandardAxes.TopAxis()
                : new ScottPlot.Axis.StandardAxes.BottomAxis();

            formsPlot1.Plot.XAxes.Add(xAxis);
            xAxis.Label.Text = $"X Axis {formsPlot1.Plot.XAxes.Count()}";

            // add a new plottable and tell it to use our custom Y axis
            var sig = formsPlot1.Plot.Plottables.AddSignal(ScaledSine());
            sig.Data.Period = Math.Pow(10, formsPlot1.Plot.YAxes.Count() - 1);
            sig.Axes.YAxis = yAxis;
            sig.Axes.XAxis = xAxis;
            yAxis.Label.Color = sig.Color;
            xAxis.Label.Color = sig.Color;

            formsPlot1.Refresh();
            SetButtonEnabledStates();
        }

        private void RemoveYAxis()
        {
            IPlottable plottableToRemove = formsPlot1.Plot.Plottables.Last();
            formsPlot1.Plot.Plottables.Remove(plottableToRemove);
            formsPlot1.Plot.YAxes.Remove(plottableToRemove.Axes.YAxis);
            formsPlot1.Plot.XAxes.Remove(plottableToRemove.Axes.XAxis);

            formsPlot1.Refresh();
            SetButtonEnabledStates();
        }

        private double[] ScaledSine()
        {
            int count = formsPlot1.Plot.YAxes.Count();
            return Generate.Sin(51,
                mult: Math.Pow(10, count),
                phase: -count / 20.0);
        }
    }
}
