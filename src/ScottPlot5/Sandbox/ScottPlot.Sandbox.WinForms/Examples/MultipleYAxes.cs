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
            button1.Click += (s, e) => AddYAxis();
            button2.Click += (s, e) => AddYAxis();
            formsPlot1.Plot.Plottables.AddSignal(GetRandomData());
            formsPlot1.Refresh();
        }

        private void AddYAxis()
        {
            // create a new axis and add it to the plot
            var yAxis = new ScottPlot.Axis.StandardAxes.LeftAxis();
            formsPlot1.Plot.YAxes.Add(yAxis);

            // add a new plottable and tell it to use our custom Y axis
            var sig = formsPlot1.Plot.Plottables.AddSignal(GetRandomData());
            sig.Axes.YAxis = yAxis;

            formsPlot1.Refresh();
        }

        private double[] GetRandomData()
        {
            return Generate.Sin(51,
                mult: Math.Pow(10, Rand.NextDouble() * 6),
                phase: Rand.NextDouble() * Math.PI * 2);
        }
    }
}
