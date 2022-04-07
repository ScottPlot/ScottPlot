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

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Prepare separate Y axes
            formsPlot1.Plot.YAxis.Color(Color.Magenta);
            var yAxis1 = formsPlot1.Plot.YAxis;
            var yAxis2 = formsPlot1.Plot.AddAxis(ScottPlot.Renderable.Edge.Left, 2, "", Color.Navy);
            var yAxis3 = formsPlot1.Plot.AddAxis(ScottPlot.Renderable.Edge.Left, 3, "", Color.Green);

            // Create data and add it to the plot
            Random rand = new(0);
            double[] data1 = DataGen.RandomWalk(rand, 100, mult: 1);
            double[] data2 = DataGen.RandomWalk(rand, 100, mult: 10);
            double[] data3 = DataGen.RandomWalk(rand, 100, mult: 100);
            double avg1 = data1.Sum() / data1.Length;
            double avg2 = data2.Sum() / data2.Length;
            double avg3 = data3.Sum() / data3.Length;

            var plt1 = formsPlot1.Plot.AddSignal(data1);
            plt1.YAxisIndex = 0;
            plt1.LineWidth = 3;
            plt1.Color = Color.Magenta;

            var plt2 = formsPlot1.Plot.AddSignal(data2);
            plt2.YAxisIndex = 3;
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
            hline1.PositionLabelBackground = plt1.Color;
            hline1.PositionFormatter = (double position) => position.ToString("N0");
            hline1.PositionLabel = true;
            hline1.PositionLabelAxis = yAxis1;

            var hline2 = formsPlot1.Plot.AddHorizontalLine(avg3);
            hline2.DragEnabled = true;
            hline2.Color = plt3.Color;
            hline2.LineStyle = LineStyle.Dash;
            hline2.YAxisIndex = 2;
            hline2.PositionLabelBackground = plt3.Color;
            hline2.PositionFormatter = (double position) => position.ToString("N0");
            hline2.PositionLabel = true;
            hline2.PositionLabelAxis = yAxis2;

            var hline3 = formsPlot1.Plot.AddHorizontalLine(avg2);
            hline3.DragEnabled = true;
            hline3.Color = plt2.Color;
            hline3.LineStyle = LineStyle.Dash;
            hline3.YAxisIndex = 3;
            hline3.PositionLabelBackground = plt2.Color;
            hline3.PositionFormatter = (double position) => position.ToString("N0");
            hline3.PositionLabel = true;
            hline3.PositionLabelAxis = yAxis3;

            // update the plot
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }
    }
}
