using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class PlotsInScrollViewer : Form
    {
        Random rand = new Random();

        public PlotsInScrollViewer()
        {
            InitializeComponent();

            FormsPlot[] formsPlots = { formsPlot1, formsPlot2, formsPlot3 };

            foreach (FormsPlot formsPlot in formsPlots)
            {
                for (int i = 0; i < 3; i++)
                    formsPlot.Plot.AddSignal(DataGen.RandomWalk(rand, 100));

                formsPlot.Configuration.ScrollWheelZoom = false;

                formsPlot.Render();
            }
        }
    }
}
