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

namespace FormsPlotCoreSandbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ScottPlot.PlottableVLine vline;
        ScottPlot.PlottableHLine hline;
        ScottPlot.PlottableScatterHighlight sph;
        private void Form1_Load(object sender, EventArgs e)
        {
            vline = formsPlot1.plt.PlotVLine(1);
            hline = formsPlot1.plt.PlotHLine(1);

            formsPlot1.plt.PlotHSpan(10, 20, draggable: true);
            formsPlot1.plt.PlotVSpan(5, 10, draggable: true);

            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(100);
            double[] ys = DataGen.RandomWalk(rand, 100);
            sph = formsPlot1.plt.PlotScatterHighlight(xs, ys);

            formsPlot1.Render();
        }

        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            (double x, double y) = formsPlot1.GetMouseCoordinates();
            vline.position = x;
            hline.position = y;
            sph.HighlightClear();
            sph.HighlightPointNearest(x, y);
            formsPlot1.Render();
        }
    }
}
