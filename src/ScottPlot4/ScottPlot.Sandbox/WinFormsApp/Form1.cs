using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        readonly ScottPlot.Plottable.Crosshair MyCrosshair;

        public Form1()
        {
            InitializeComponent();
            Width = 1200;
            Height = 1000;
            MyCrosshair = formsPlot1.Plot.AddCrosshair(0, 0);
            formsPlot1.Refresh();
            formsPlot1.MouseMove += FormsPlot1_MouseMove;
        }

        private void FormsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            (MyCrosshair.X, MyCrosshair.Y) = formsPlot1.GetMouseCoordinates();
            formsPlot1.Refresh(skipIfCurrentlyRendering: true);

            int depth = new System.Diagnostics.StackTrace().FrameCount;
            label1.Text = $"Stack depth: {depth}";
            progressBar1.Value = depth;
        }
    }
}
