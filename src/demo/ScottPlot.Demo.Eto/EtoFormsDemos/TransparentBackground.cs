using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Eto.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class TransparentBackground : Form
    {
        public TransparentBackground()
        {
            InitializeComponent();

            // move content onto a backgroundimagecontrol for image support
            this.Content = new BackgroundImageControl() { Content = this.Content };

            this.button1.Click += this.button1_Click;
            this.button2.Click += this.button2_Click;
            this.button3.Click += this.button3_Click;
            this.button4.Click += this.button4_Click;
            this.button5.Click += this.button5_Click;
            this.button6.Click += this.button6_Click;

            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            formsPlot1.Plot.AddScatter(x, sin);
            formsPlot1.Plot.AddScatter(x, cos);

            formsPlot1.Plot.Style(figureBackground: System.Drawing.Color.Transparent, dataBackground: System.Drawing.Color.Transparent);
            formsPlot1.BackgroundColor = Colors.Transparent;
            button6_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetBackground(Colors.Red);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetBackground(Colors.Green);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetBackground(Colors.Blue);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetBackground(Colors.White);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetBackground(SystemColors.Control);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // apply a Bitmap to the background of this form
            Random rand = new Random();
            Bitmap bmp = new Bitmap(200, 200, PixelFormat.Format24bppRgb);
            Graphics gfx = new Graphics(bmp);
            gfx.AntiAlias = true;
            gfx.Clear(SystemColors.Control);
            Pen pen = new Pen(Color.FromGrayscale(40));
            Size circleSize = new Size(20, 20);
            for (int i = 0; i < 100; i++)
            {
                Point randomPoint = new Point(rand.Next(bmp.Width - circleSize.Width), rand.Next(bmp.Height - circleSize.Height));
                Rectangle rect = new Rectangle(randomPoint, circleSize);
                gfx.DrawEllipse(pen, rect);
            }

            (Content as BackgroundImageControl).BackgroundImage = bmp;
            Content.Invalidate();
        }

        private void SetBackground(Color bgcolor)
        {
            (Content as BackgroundImageControl).BackgroundImage = null;
            BackgroundColor = bgcolor;
            Content.Invalidate();
        }
    }
}
