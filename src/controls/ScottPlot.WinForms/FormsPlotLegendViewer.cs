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
                PictureBoxLegend.Image = Legend.GetBitmap(false, 5);
                var frmmaxwidth = PictureBoxLegend.Image.Width + 2 * SystemInformation.VerticalScrollBarWidth;
                var frmminwidth = frmmaxwidth;
                var frmmaxheight = PictureBoxLegend.Image.Height + 3 * SystemInformation.HorizontalScrollBarHeight;
                var frmminheight = frmmaxheight > 500 ? 500 : frmmaxheight;
                MinimumSize = new(frmminwidth, frmminheight);
                MaximumSize = new(frmmaxwidth, frmmaxheight);
                Size = new(frmminwidth, frmminheight);
                SizeGripStyle = SizeGripStyle.Show;
                PictureBoxLegend.Cursor = Cursors.Hand;
            }
            else
            {
                //this.Close();
                MessageBox.Show("Current legend has no items", "Detached Legend", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

        private Renderable.Legend Legend;
    }
}
