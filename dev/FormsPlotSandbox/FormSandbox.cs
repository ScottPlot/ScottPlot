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
            formsPlot1.plt.PlotSignal(DataGen.Sin(51));
            formsPlot1.plt.PlotSignal(DataGen.Cos(51));

            formsPlot1.plt.Ticks(false, false);
            formsPlot1.plt.Frame(false);
            formsPlot1.plt.LayoutFrameless();
            formsPlot1.Configure(recalculateLayoutOnMouseUp: false);
            formsPlot1.plt.Style(Style.Gray2);

            formsPlot1.Render();
        }
    }
}
