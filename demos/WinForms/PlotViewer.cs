using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class PlotViewer : Form
    {
        public PlotViewer(ScottPlot.Plot plt)
        {
            InitializeComponent();
            Controls.Add(new ScottPlot.FormsPlot(plt) { Dock = DockStyle.Fill, Name = "formsPlot1" });
        }
    }
}
