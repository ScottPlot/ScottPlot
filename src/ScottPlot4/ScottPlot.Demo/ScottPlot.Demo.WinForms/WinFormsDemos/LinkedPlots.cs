using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class LinkedPlots : Form
    {
        public LinkedPlots()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(DataGen.Sin(51, 2), color: System.Drawing.Color.Blue);
            formsPlot2.Plot.AddSignal(DataGen.Cos(51, 2), color: System.Drawing.Color.Red);

            formsPlot1.Refresh();
            formsPlot2.Refresh();

            cbLinked_CheckedChanged(null, null);
        }

        private void cbLinked_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLinked.Checked)
            {
                formsPlot1.Configuration.AddLinkedControl(formsPlot2); // update plot 2 when plot 1 changes
                formsPlot2.Configuration.AddLinkedControl(formsPlot1); // update plot 1 when plot 2 changes
            }
            else
            {
                formsPlot1.Configuration.ClearLinkedControls();
                formsPlot2.Configuration.ClearLinkedControls();
            }
        }
    }
}
