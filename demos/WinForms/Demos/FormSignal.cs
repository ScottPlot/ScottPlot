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
    public partial class FormSignal : Form
    {
        public FormSignal()
        {
            InitializeComponent();
            Random rand = new Random();

            int pointsPerSignal = 1_000_000;
            int signalCount = 5;

            for (int i=0; i<signalCount; i++)
                formsPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, pointsPerSignal));

            formsPlot1.plt.Title("5,000,000 Data Points");
            formsPlot1.plt.YLabel("Random Value");
            formsPlot1.plt.XLabel("Array Index");
            formsPlot1.Render();
        }

        private void FormSignal_Load(object sender, EventArgs e)
        {

        }
    }
}
