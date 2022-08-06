using ScottPlot;
using System;
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

            VLine = formsPlot1.Plot.AddVerticalLine(23, Color.Blue);
            VLine.DragEnabled = true;
            VLine.DragSnapX = SnapToX;

            HLine = formsPlot1.Plot.AddHorizontalLine(.2, Color.Green);
            HLine.DragEnabled = true;
            HLine.DragSnapY = SnapToY;

            formsPlot1.Refresh();
        }

        private double SnapToX(double mousePosition)
        {
            return GetClosestPosition(mousePosition, XSnapPositions);
        }

        private double SnapToY(double mousePosition)
        {
            return GetClosestPosition(mousePosition, YSnapPositions);
        }

        private double GetClosestPosition(double mouse, double[] snaps)
        {
            double closestDistance = double.MaxValue;
            double closestPosition = double.MaxValue;

            for (int i = 0; i < snaps.Length; i++)
            {
                double distance = Math.Abs(mouse - snaps[i]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = snaps[i];
                }
            }

            return closestPosition;
        }
    }
}
