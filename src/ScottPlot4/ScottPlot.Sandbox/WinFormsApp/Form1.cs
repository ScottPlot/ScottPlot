using ScottPlot;
using System.Drawing;
using System.Windows.Forms;
using ScottPlot.SnapLogic;

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

            VLine = formsPlot1.Plot.AddVerticalLine(23, Color.Blue);
            VLine.DragEnabled = true;
            VLine.DragSnapX = new NearestPosition(XSnapPositions);

            HLine = formsPlot1.Plot.AddHorizontalLine(.2, Color.Green);
            HLine.DragEnabled = true;
            HLine.DragSnapY = new NearestPosition(YSnapPositions);

            formsPlot1.Refresh();
        }
    }
}
