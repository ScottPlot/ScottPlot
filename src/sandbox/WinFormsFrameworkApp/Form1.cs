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

            double[,] data = DataGen.Random2D(new Random(), 10, 10);
            var hm = formsPlot1.Plot.AddHeatmap(data, lockScales: false);
            formsPlot1.Plot.AddColorbar(hm.Colormap);
            formsPlot1.Plot.SetAxisLimits(2.5, 2.6, 2.5, 2.6);
        }
    }
}
