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

            formsPlot1.Plot.AddLine(-1e5, -1e10, 1e5, 1e10);
            formsPlot1.Plot.XAxis.TickLabelNotation(multiplier: true);
            formsPlot1.Plot.YAxis.TickLabelNotation(multiplier: true);
            formsPlot1.Render();
        }
    }
}
