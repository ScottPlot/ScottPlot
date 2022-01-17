using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.WinFormsFramework
{
    public partial class Form1 : Form
    {
        readonly Plot Plot = new();

        public Form1()
        {
            InitializeComponent();
            Plot.AddDemoSinAndCos();
        }

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };
            Plot.Draw(canvas, skglControl1.Width, skglControl1.Height);
        }
    }
}
