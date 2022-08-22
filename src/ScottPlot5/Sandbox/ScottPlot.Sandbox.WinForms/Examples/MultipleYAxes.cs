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

namespace WinForms.Examples
{
    public partial class MultipleYAxes : Form, IExampleForm
    {
        Random Rand = new();
        public string SandboxTitle => "Multiple Y Axes";
        public string SandboxDescription => "Plot data with greatly different scales using separate Y axes";

        public MultipleYAxes()
        {
            InitializeComponent();
            btnAddAxis.Click += (s, e) => AddYAxis();
            btnRemoveAxis.Click += (s, e) => RemoveYAxis();

            var sig = formsPlot1.Plot.Plottables.AddSignal(ScaledSine());
            formsPlot1.Plot.YAxis.Label.Text = $"Y Axis 1";
            formsPlot1.Plot.YAxis.Label.Color = sig.Color;

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
            var yAxis = new ScottPlot.Axis.StandardAxes.LeftAxis();
            formsPlot1.Plot.YAxes.Add(yAxis);
            yAxis.Label.Text = $"Y Axis {formsPlot1.Plot.YAxes.Count()}";

            // add a new plottable and tell it to use our custom Y axis
            var sig = formsPlot1.Plot.Plottables.AddSignal(ScaledSine());
            sig.Axes.YAxis = yAxis;
            yAxis.Label.Color = sig.Color;

            formsPlot1.Refresh();
            SetButtonEnabledStates();
        }

        private void RemoveYAxis()
        {
            IPlottable plottableToRemove = formsPlot1.Plot.Plottables.Last();
            formsPlot1.Plot.Plottables.Remove(plottableToRemove);
            formsPlot1.Plot.YAxes.Remove(plottableToRemove.Axes.YAxis);

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
