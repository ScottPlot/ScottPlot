using System;
using ScottPlot;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var func1 = new Func<double, double?>((x) => 5 * Math.Sin(x) * Math.Sin(x / 2));

            ScottPlot.Plottable.FunctionPlot f = formsPlot1.Plot.AddFunction(func1);

            formsPlot1.Refresh();
        }
    }
}
