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

namespace FormsPlotSandbox
{
    public partial class FormSandbox : Form
    {
        public FormSandbox()
        {
            InitializeComponent();
        }

        private void FormSandbox_Load(object sender, EventArgs e)
        {
            formsPlot1.plt.YLabel("Primary Vertical Axis");
            formsPlot1.plt.YLabel2("Secondary Vertical Axis");

            var YAxis3 = new ScottPlot.Renderable.AdditionalRightAxis(2, true);
            YAxis3.Title.Label = "Tertiary Vertical Axis";
            formsPlot1.plt.GetSettings(false).Axes.Add(YAxis3);

            var sig1 = formsPlot1.plt.PlotSignal(DataGen.Sin(51, mult: 1, phase: 0));
            var sig2 = formsPlot1.plt.PlotSignal(DataGen.Sin(51, mult: 10, phase: .2));
            var sig3 = formsPlot1.plt.PlotSignal(DataGen.Sin(51, mult: 100, phase: .4));

            sig1.VerticalAxisIndex = 0;
            sig2.VerticalAxisIndex = 1;
            sig3.VerticalAxisIndex = 2;

            formsPlot1.Render();
        }
    }
}
