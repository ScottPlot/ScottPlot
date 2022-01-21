using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class FormsPlotLegendViewer : Form
    {
        public FormsPlotLegendViewer(ScottPlot.Plot plot, string windowTitle = "Detached Legend")
        {
            InitializeComponent();
            var legend = plot.Legend(false);
            var li = legend.GetItems();
            if (legend.HasItems)
            {
                this.Show();
                legend.OutlineColor = Color.White;
                legend.ShadowColor = Color.White;
                pictureBoxLegend.Image = legend.GetBitmap();
                var width = pictureBoxLegend.Image.Width + 2*SystemInformation.HorizontalScrollBarHeight;
                var height = pictureBoxLegend.Image.Height + 2*SystemInformation.CaptionHeight;
                MinimumSize = new(width, height);
                MaximumSize = new(width, height);
            }
            else
            {
                //this.Close();
                MessageBox.Show("Current legend has no items", "Detached Legend", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}