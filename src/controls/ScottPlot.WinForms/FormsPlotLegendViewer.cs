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
                PictureBoxLegend.Image = legend.GetBitmap();
                var frmmaxwidth = PictureBoxLegend.Image.Width + 2 * SystemInformation.VerticalScrollBarWidth;
                var frmminwidth = frmmaxwidth;
                var frmmaxheight = PictureBoxLegend.Image.Height + 3 * SystemInformation.HorizontalScrollBarHeight;
                var frmminheight = frmmaxheight > 500 ? 500 : frmmaxheight;
                MinimumSize = new(frmminwidth, frmminheight);
                MaximumSize = new(frmmaxwidth, frmmaxheight);
                Size = new(frmminwidth, frmminheight);
                SizeGripStyle = SizeGripStyle.Show;
            }
            else
            {
                //this.Close();
                MessageBox.Show("Current legend has no items", "Detached Legend", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}