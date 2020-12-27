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
    public partial class LinkedPlots : Form
    {
        public LinkedPlots()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(DataGen.Sin(51));
            formsPlot2.Plot.AddSignal(DataGen.Cos(51));
        }

        private void formsPlot1_AxesChanged(object sender, EventArgs e)
        {
            if (cbLinked.Checked == false)
                return;

            formsPlot2.Configuration.AxesChangedEventEnabled = false; // disable this to avoid an infinite loop
            formsPlot2.Plot.SetAxisLimits(formsPlot1.Plot.GetAxisLimits());
            formsPlot2.Render();
            formsPlot2.Configuration.AxesChangedEventEnabled = true;
        }
        
        private void formsPlot2_AxesChanged(object sender, EventArgs e)
        {
            if (cbLinked.Checked == false)
                return;

            formsPlot1.Configuration.AxesChangedEventEnabled = false; // disable this to avoid an infinite loop
            formsPlot1.Plot.SetAxisLimits(formsPlot2.Plot.GetAxisLimits());
            formsPlot1.Render();
            formsPlot1.Configuration.AxesChangedEventEnabled = true;
        }
    }
}
