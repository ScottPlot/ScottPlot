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
        readonly ScottPlot.Plottable.Heatmap MyHeatmap;
        readonly ScottPlot.Plottable.ScatterPlot SquareOutline;
        readonly double[,] MyHeatmapData = DataGen.SampleImageData();

        public Form1()
        {
            InitializeComponent();

            MyHeatmap = formsPlot1.Plot.AddHeatmap(MyHeatmapData, lockScales: false);
            MyHeatmap.CellWidth = 10;
            MyHeatmap.CellHeight = .1;
            MyHeatmap.OffsetX = 100;
            MyHeatmap.OffsetY = 200;

            formsPlot1.Plot.AddColorbar(MyHeatmap);

            SquareOutline = formsPlot1.Plot.AddScatterLines(
                xs: new double[] { 0, 0, MyHeatmap.CellWidth, MyHeatmap.CellWidth, 0 },
                ys: new double[] { 0, MyHeatmap.CellHeight, MyHeatmap.CellHeight, 0, 0 },
                lineWidth: 2,
                color: Color.Magenta);
            SquareOutline.IsVisible = false;

            formsPlot1.LeftClicked += FormsPlot1_LeftClicked; ;
            formsPlot1.Refresh();
        }

        private void FormsPlot1_LeftClicked(object sender, EventArgs e)
        {
            (double x, double y) = formsPlot1.GetMouseCoordinates();
            (int? xIndex, int? yIndex) = MyHeatmap.GetCellIndexes(x, y);

            System.Diagnostics.Debug.WriteLine($"Clicked heatmap cell ");

            if (xIndex.HasValue)
                SquareOutline.OffsetX = xIndex.Value * MyHeatmap.CellWidth + MyHeatmap.OffsetX;

            if (yIndex.HasValue)
            {
                SquareOutline.OffsetY = yIndex.Value * MyHeatmap.CellHeight + MyHeatmap.OffsetY;
                formsPlot1.Plot.Title($"Selected X={xIndex} Y={yIndex} Value={MyHeatmapData[yIndex.Value, xIndex.Value]}");
            }
            else
            {
                formsPlot1.Plot.Title($"No Heatmap Cell Selected");
            }

            SquareOutline.IsVisible = xIndex.HasValue && yIndex.HasValue;

            formsPlot1.Render();
        }
    }
}
