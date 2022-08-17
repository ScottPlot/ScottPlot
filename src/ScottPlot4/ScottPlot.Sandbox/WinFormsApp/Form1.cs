using System;
using ScottPlot;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        ScottPlot.Plottable.MarkerPlot Marker;
        ScottPlot.Plottable.ScatterPlot Scatter;

        public Form1()
        {
            InitializeComponent();

            double[] xs = DataGen.Consecutive(51);
            double[] ys = DataGen.Sin(51);
            ys = DataGen.InsertNanRanges(ys, new Random(0));

            Scatter = formsPlot1.Plot.AddScatter(xs, ys);
            Scatter.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            Marker = formsPlot1.Plot.AddMarker(0, 0, MarkerShape.openCircle, 20, Color.Red);
            formsPlot1.MouseMove += FormsPlot1_MouseMove;
            formsPlot1.Refresh();
        }

        private void FormsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            (double x, double y) = formsPlot1.GetMouseCoordinates();
            double xyRatio = formsPlot1.Plot.XAxis.Dims.UnitsPerPx / formsPlot1.Plot.YAxis.Dims.UnitsPerPx;
            (double ptX, double ptY, int ptIndex) = Scatter.GetPointNearest(x, y, xyRatio);

            if (double.IsNaN(ptX) || double.IsNaN(ptY))
                return;

            Marker.X = ptX;
            Marker.Y = ptY;
            formsPlot1.Refresh();
        }
    }
}
