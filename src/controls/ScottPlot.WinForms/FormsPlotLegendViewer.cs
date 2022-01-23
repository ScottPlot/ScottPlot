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
        public FormsPlotLegendViewer(FormsPlot Fplot, string windowTitle = "Detached Legend")
        {
            InitializeComponent();
            Legend = Fplot.Plot.Legend(false);
            Fplot.Refresh();
            if (Legend.HasItems)
            {
                this.Show();
                Legend.OutlineColor = Color.White;
                Legend.ShadowColor = Color.White;
                PictureBoxLegend.Image = Legend.GetBitmap(false);
                PictureBoxLegend.Click += delegate (object sender, EventArgs e) { PictureBoxLegend_Click(sender, (MouseEventArgs)e, Fplot); };
                var frmmaxwidth = PictureBoxLegend.Image.Width + 2 * SystemInformation.VerticalScrollBarWidth;
                var frmminwidth = frmmaxwidth;
                var frmmaxheight = PictureBoxLegend.Image.Height + 3 * SystemInformation.HorizontalScrollBarHeight;
                var frmminheight = frmmaxheight > 500 ? 500 : frmmaxheight;
                MinimumSize = new(frmminwidth, frmminheight);
                MaximumSize = new(frmmaxwidth, frmmaxheight);
                Size = new(frmminwidth, frmminheight);
            }
            else
            {
                //this.Close();
                MessageBox.Show("Current legend has no items", "Detached Legend", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PictureBoxLegend_Click(object sender, MouseEventArgs e, FormsPlot Fplot)
        {
            var plottable_array = Fplot.Plot.GetPlottables();
            Legend.UpdateLegendItems(plottable_array, true);
            var li = Legend.GetItems();
            double totheight = PictureBoxLegend.Image.Height;
            double singleheight = (totheight / ((double)li.Length));
            double proxindex = Math.Floor(e.Y / singleheight);

            if (plottable_array[(int)proxindex].IsVisible)
            {
                plottable_array[(int)proxindex].IsVisible = false;
            }
            else
            {
                plottable_array[(int)proxindex].IsVisible = true;
            }
            Fplot.Refresh();
            Legend.UpdateLegendItems(plottable_array, true);
            PictureBoxLegend.Image = Legend.GetBitmap(false);
        }

        private Renderable.Legend Legend;
    }
}
