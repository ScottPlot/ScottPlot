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
    public partial class FormExperimentalTick : Form
    {
        ScottPlot.plottables.ExperimentalTicks ticks;
        public FormExperimentalTick()
        {
            InitializeComponent();

            formsPlot1.plt.Layout(yScaleWidth: 100, y2ScaleWidth: 100);
            formsPlot1.Configure(recalculateLayoutOnMouseUp: false);
            formsPlot1.plt.Axis(-10, 10, -15, 15);

            ticks = new ScottPlot.plottables.ExperimentalTicks();
            formsPlot1.plt.GetPlottables().Add(ticks);
            nudLineThickness_ValueChanged(null, null);
        }

        private void FormExperimentalTick_Load(object sender, EventArgs e)
        {
        }

        private void nudLineThickness_ValueChanged(object sender, EventArgs e)
        {
            ticks.lineWidth = (float)nudLineThickness.Value;
            formsPlot1.Render();
        }
    }
}
