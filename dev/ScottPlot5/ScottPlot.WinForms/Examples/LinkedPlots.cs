using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.WinForms.Examples
{
    public partial class LinkedPlots : Form
    {
        public LinkedPlots()
        {
            InitializeComponent();
            interactivePlot1.OnRender += OnRendered1;
            interactivePlot2.OnRender += OnRendered2;
        }

        private void OnRendered1()
        {
            interactivePlot2.Plot.Scale(interactivePlot1.Plot.Scale());
            interactivePlot2.Render(invokeOnRender: false);
        }

        private void OnRendered2()
        {
            interactivePlot1.Plot.Scale(interactivePlot2.Plot.Scale());
            interactivePlot1.Render(invokeOnRender: false);
        }

        private void LinkedPlots_Load(object sender, EventArgs e)
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys1 = Generate.Sin(51);
            double[] ys2 = Generate.Cos(51);

            var sp1 = interactivePlot1.Plot.PlotScatter(xs, ys1);
            sp1.Color = ColorPalette.Category10.GetColor(0);

            var sp2 = interactivePlot2.Plot.PlotScatter(xs, ys2);
            sp2.Color = ColorPalette.Category10.GetColor(1);

            interactivePlot2.Render();
        }
    }
}
