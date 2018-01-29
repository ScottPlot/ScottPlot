using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScottPlot;

namespace Simple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Figure fig = new Figure(pictureBox1.Width, pictureBox1.Height);
            fig.colorBg = SystemColors.Control;
            fig.title = "ScottPlot Demonstration";
            fig.xLabel = "Elapsed Time (years)";
            fig.yLabel = "Total Awesomeness (cool units)";
            fig.Axis(-15, 35, -10, 110); // x1, x2, y1, y2

            // update the Frame (axis labels, tick marks, etc)
            fig.RedrawFrame();

            // draw a line directly on the Graphics object in AXIS units
            Point pt1 = new Point(fig.xAxis.UnitToPx(0), fig.yAxis.UnitToPx(13));
            Point pt2 = new Point(fig.xAxis.UnitToPx(32), fig.yAxis.UnitToPx(98));
            fig.gfxGraph.DrawLine(new Pen(new SolidBrush(Color.Blue), 5), pt1, pt2);

            pictureBox1.Image = fig.Render();
        }
    }
}
