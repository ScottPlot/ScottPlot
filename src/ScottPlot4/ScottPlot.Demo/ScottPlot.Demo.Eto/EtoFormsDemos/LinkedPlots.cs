using Eto.Forms;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class LinkedPlots : Form
    {
        public LinkedPlots()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(DataGen.Sin(51), color: System.Drawing.Color.Blue);
            formsPlot2.Plot.AddSignal(DataGen.Cos(51), color: System.Drawing.Color.Red);

            formsPlot1.Refresh();
            formsPlot2.Refresh();

            formsPlot1.Configuration.AddLinkedControl(formsPlot2); // update plot 2 when plot 1 changes
            formsPlot2.Configuration.AddLinkedControl(formsPlot1); // update plot 1 when plot 2 changes
        }
    }
}
