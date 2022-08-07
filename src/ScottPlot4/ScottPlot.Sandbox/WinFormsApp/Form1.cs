using ScottPlot;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        ScottPlot.Plottable.VLine VLine;
        ScottPlot.Plottable.HLine HLine;

        double[] YSnapPositions = DataGen.Consecutive(11, 0.2, -1);
        double[] XSnapPositions = DataGen.Consecutive(11, 5D);

        public Form1()
        {
            InitializeComponent();

            formsPlot1.Plot.AddSignal(DataGen.Sin(51), 1, Color.Black);
            formsPlot1.Plot.AddSignal(DataGen.Cos(51), 1, Color.Gray);

            var snapSmooth = new ScottPlot.SnapLogic.Smooth();
            var snapX = new ScottPlot.SnapLogic.NearestPosition(XSnapPositions);
            var snapY = new ScottPlot.SnapLogic.NearestPosition(YSnapPositions);

            VLine = formsPlot1.Plot.AddVerticalLine(23, Color.Blue);
            VLine.DragEnabled = true;
            VLine.DragSnap = new ScottPlot.SnapLogic.Independent2D(snapX, snapSmooth);

            HLine = formsPlot1.Plot.AddHorizontalLine(.2, Color.Green);
            HLine.DragEnabled = true;
            HLine.DragSnap = new ScottPlot.SnapLogic.Independent2D(snapSmooth, snapY);

            formsPlot1.Refresh();
        }
    }
}
