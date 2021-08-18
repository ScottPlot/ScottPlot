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
    public partial class MultiAxis : Form
    {
        private readonly ScottPlot.Renderable.Axis YAxis3;

        public MultiAxis()
        {
            InitializeComponent();
            Random rand = new Random();

            // Add 3 signals each with a different vertical axis index.
            // Each signal defaults to X axis index 0 so their horizontal axis will be shared.

            var plt1 = formsPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 1));
            plt1.YAxisIndex = 0;
            plt1.LineWidth = 3;
            plt1.Color = Color.Red;

            var plt2 = formsPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 10));
            plt2.YAxisIndex = 1;
            plt2.LineWidth = 3;
            plt2.Color = Color.Green;

            var plt3 = formsPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 100));
            plt3.YAxisIndex = 2;
            plt3.LineWidth = 3;
            plt3.Color = Color.Blue;

            // The horizontal axis is shared by these signal plots (XAxisIndex defaults to 0)
            formsPlot1.Plot.XAxis.Label("Horizontal Axis");

            // Customize the primary (left) and secondary (right) axes
            formsPlot1.Plot.YAxis.Color(Color.Red);
            formsPlot1.Plot.YAxis.Label("Primary Axis");
            formsPlot1.Plot.YAxis2.Color(Color.Green);
            formsPlot1.Plot.YAxis2.Label("Secondary Axis");

            // the secondary (right) axis ticks are hidden by default so enable them
            formsPlot1.Plot.YAxis2.Ticks(true);

            // Create an additional vertical axis and customize it
            YAxis3 = formsPlot1.Plot.AddAxis(Renderable.Edge.Left, 2);
            YAxis3.Color(Color.Blue);
            YAxis3.Label("Tertiary Axis");
        }

        private void UpdateLocks()
        {
            // set axis lock status based on radio button check state
        }

        private void rbPrimary_CheckedChanged(object sender, EventArgs e) => UpdateLocks();
        private void rbSecondary_CheckedChanged(object sender, EventArgs e) => UpdateLocks();
        private void rbTertiary_CheckedChanged(object sender, EventArgs e) => UpdateLocks();
        private void rbAll_CheckedChanged(object sender, EventArgs e) => UpdateLocks();
    }
}
