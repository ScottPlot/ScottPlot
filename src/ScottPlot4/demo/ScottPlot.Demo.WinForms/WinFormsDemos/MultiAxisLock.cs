using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class MultiAxisLock : Form
    {
        private readonly ScottPlot.Renderable.Axis YAxis3;

        public MultiAxisLock()
        {
            InitializeComponent();

            Random rand = new Random();
            double[] data1 = DataGen.RandomWalk(rand, 100, mult: 1);
            double[] data2 = DataGen.RandomWalk(rand, 100, mult: 10);
            double[] data3 = DataGen.RandomWalk(rand, 100, mult: 100);
            double avg1 = data1.Sum() / data1.Length;
            double avg2 = data2.Sum() / data2.Length;
            double avg3 = data3.Sum() / data3.Length;

            // Add signals specifying the vertical axis index for each
            var plt1 = formsPlot1.Plot.AddSignal(data1);
            plt1.YAxisIndex = 0;
            plt1.LineWidth = 3;
            plt1.Color = Color.Magenta;

            var plt2 = formsPlot1.Plot.AddSignal(data2);
            plt2.YAxisIndex = 1;
            plt2.LineWidth = 3;
            plt2.Color = Color.Green;

            var plt3 = formsPlot1.Plot.AddSignal(data3);
            plt3.YAxisIndex = 2;
            plt3.LineWidth = 3;
            plt3.Color = Color.Navy;

            // Add draggable horizontal lines specifying the vertical axis index for each
            var hline1 = formsPlot1.Plot.AddHorizontalLine(avg1);
            hline1.DragEnabled = true;
            hline1.Color = plt1.Color;
            hline1.LineStyle = LineStyle.Dash;
            hline1.YAxisIndex = 0;

            var hline2 = formsPlot1.Plot.AddHorizontalLine(avg2);
            hline2.DragEnabled = true;
            hline2.Color = plt2.Color;
            hline2.LineStyle = LineStyle.Dash;
            hline2.YAxisIndex = 1;

            var hline3 = formsPlot1.Plot.AddHorizontalLine(avg3);
            hline3.DragEnabled = true;
            hline3.Color = plt3.Color;
            hline3.LineStyle = LineStyle.Dash;
            hline3.YAxisIndex = 2;

            // The horizontal axis is shared by these signal plots (XAxisIndex defaults to 0)
            formsPlot1.Plot.XAxis.Label("Horizontal Axis");

            // Customize the primary (left) and secondary (right) axes
            formsPlot1.Plot.YAxis.Color(Color.Magenta);
            formsPlot1.Plot.YAxis.Label("Primary Axis");
            formsPlot1.Plot.YAxis2.Color(Color.Green);
            formsPlot1.Plot.YAxis2.Label("Secondary Axis");

            // the secondary (right) axis ticks are hidden by default so enable them
            formsPlot1.Plot.YAxis2.Ticks(true);

            // Create an additional vertical axis and customize it
            YAxis3 = formsPlot1.Plot.AddAxis(Renderable.Edge.Left, 2);
            YAxis3.Color(Color.Navy);
            YAxis3.Label("Tertiary Axis");

            // adjust axis limits to fit the data once before locking them based on initial check state
            formsPlot1.Plot.AxisAuto();
            SetLocks();

            formsPlot1.Refresh();
        }

        private void SetLocks()
        {
            formsPlot1.Plot.YAxis.LockLimits(!cbPrimary.Checked);
            formsPlot1.Plot.YAxis2.LockLimits(!cbSecondary.Checked);
            YAxis3.LockLimits(!cbTertiary.Checked);
        }

        private void cbPrimary_CheckedChanged(object sender, EventArgs e) => SetLocks();
        private void cbSecondary_CheckedChanged(object sender, EventArgs e) => SetLocks();
        private void cbTertiary_CheckedChanged(object sender, EventArgs e) => SetLocks();
    }
}
