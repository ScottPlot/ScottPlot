using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            double[] values = DataGen.RandomWalk(new Random(0), 100);
            formsPlot1.Plot.AddSignal(values);

            formsPlot1.Plot.SetViewLimits(yMin: 0);
        }
    }
}
