using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.WinForms
{
    public partial class InteractivePlot : UserControl
    {
        public Plot plt = new Plot();

        public InteractivePlot()
        {
            InitializeComponent();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e) => Render();

        public void Render()
        {
            if (pictureBox1.Image is null || pictureBox1.Image.Size != pictureBox1.Size)
            {
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            }

            plt.GetBitmapV41((Bitmap)pictureBox1.Image);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
        }
    }
}
