using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] legendLocationStrings = Enum.GetNames(typeof(ScottPlot.legendLocation));
            cbLocations.Items.AddRange(legendLocationStrings);
            cbLocations.SelectedItem = cbLocations.Items[0];

            string[] dropShadowDirectionStrings = Enum.GetNames(typeof(ScottPlot.shadowDirection));
            cbShadowDirection.Items.AddRange(dropShadowDirectionStrings);
            cbShadowDirection.SelectedItem = cbShadowDirection.Items[0];

            string[] markerStrings = Enum.GetNames(typeof(ScottPlot.MarkerShape));
            cbMarker.Items.AddRange(markerStrings);
            cbMarker.SelectedItem = cbMarker.Items[0];

            Application.DoEvents();

            UpdatePlot();
        }

        private void UpdatePlot()
        {
            // create random data
            Random rand = new Random();
            int pointCount = 100;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] ys1 = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            double[] ys2 = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            double[] ys3 = ScottPlot.DataGen.RandomWalk(rand, pointCount);

            // plot the data
            ScottPlot.MarkerShape markerShape = ScottPlot.MarkerShape.filledCircle;
            if (cbMarker.SelectedItem != null)
                markerShape = (ScottPlot.MarkerShape)Enum.Parse(typeof(ScottPlot.MarkerShape), cbMarker.SelectedItem.ToString());

            scottPlotUC1.plt.Clear();
            scottPlotUC1.plt.PlotScatter(xs, ys1, label: "one", markerShape: markerShape, markerSize: (double)nudMarkerSize.Value);
            scottPlotUC1.plt.PlotScatter(xs, ys2, label: "two", markerShape: markerShape, markerSize: (double)nudMarkerSize.Value);
            scottPlotUC1.plt.PlotScatter(xs, ys3, label: "three", markerShape: markerShape, markerSize: (double)nudMarkerSize.Value);


            // optionally use a legend
            if (cbLocations.SelectedItem != null && cbShadowDirection.SelectedItem != null)
            {
                string locationString = cbLocations.SelectedItem.ToString();
                ScottPlot.legendLocation location = (ScottPlot.legendLocation)Enum.Parse(typeof(ScottPlot.legendLocation), locationString);
                string dropShadowString = cbShadowDirection.SelectedItem.ToString();
                ScottPlot.shadowDirection dropShadowDirection = (ScottPlot.shadowDirection)Enum.Parse(typeof(ScottPlot.shadowDirection), dropShadowString);
                scottPlotUC1.plt.Legend(location: location, shadowDirection: dropShadowDirection);
            }

            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void Cboxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePlot();
        }

        private void CbMarker_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePlot();
        }

        private void NudMarkerSize_ValueChanged(object sender, EventArgs e)
        {
            UpdatePlot();
        }
    }
}
