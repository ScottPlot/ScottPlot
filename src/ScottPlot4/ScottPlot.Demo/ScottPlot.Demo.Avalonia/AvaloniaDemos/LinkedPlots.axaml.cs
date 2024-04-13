using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class LinkedPlots : Window
    {
        public LinkedPlots()
        {
            InitializeComponent();

            avaPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51), color: System.Drawing.Color.Blue);
            avaPlot2.Plot.AddSignal(ScottPlot.DataGen.Cos(51), color: System.Drawing.Color.Red);

            avaPlot1.Refresh();
            avaPlot2.Refresh();

            avaPlot1.Configuration.AddLinkedControl(avaPlot2); // update plot 2 when plot 1 changes
            avaPlot2.Configuration.AddLinkedControl(avaPlot1); // update plot 1 when plot 2 changes
        }
    }
}
