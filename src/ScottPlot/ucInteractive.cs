using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class ucInteractive : UserControl
    {
        public ScottPlot.Figure SP;
        public double[] Xs, Ys; // this is where data is stored for interactive plotting

        public ucInteractive()
        {
            InitializeComponent();
            this.pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            SP = new ScottPlot.Figure();
            GraphResize();
        }

        public void GraphResize()
        {
            //Console.WriteLine("resizing to ({0}, {1})", pictureBox1.Width, pictureBox1.Height);
            SP.Resize(pictureBox1.Width, pictureBox1.Height);
        }

        public void GraphRender()
        {
            if (Xs.Length < 2 || Ys.Length < 2) return;
            SP.Clear();
            SP.Grid();
            SP.PlotLine(Xs, Ys);
            pictureBox1.BackgroundImage = SP.Render();
            this.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("pictureBox1_Click");
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("pictureBox1_MouseClick");
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                Console.WriteLine("mouse wheel zoom out");
                SP.AxisZoom(1.5, 1.5);
                GraphRender();
            }
            else if (e.Delta > 0)
            {
                Console.WriteLine("mouse wheel zoom in");
                SP.AxisZoom(.5, .5);
                GraphRender();
            }
            

        }
    }
}
