using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormScottPlot41 : Form
    {
        public FormScottPlot41()
        {
            InitializeComponent();
            interactivePlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(51));
            interactivePlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(51));
            interactivePlot1.Render();
        }
    }
}
