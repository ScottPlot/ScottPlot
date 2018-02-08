using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotUC_Demo
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            scottPlotUC1.fig.labelTitle = string.Format("ScottPlot User Control Demo");
            scottPlotUC1.showBenchmark = true;
        }      
        
        private void btn_xy_mode(object sender, EventArgs e)
        {
            timer1.Enabled = false; // turn off live mode
            scottPlotUC1.PlotXY(scottPlotUC1.fig.gen.Sequence(50), scottPlotUC1.fig.gen.RandomWalk(50,100), color: scottPlotUC1.fig.gen.randomColor);
            scottPlotUC1.AxisAuto();
        }
        
        private void btn_animated_sine(object sender, EventArgs e)
        {
            scottPlotUC1.fig.AxisSet(0, .05, -1.1, 1.1); // we know what the limits should be
            timer1.Enabled = true; // start automatic updates
        }

        private bool busyPlotting = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (busyPlotting) return;
            busyPlotting = true;
            scottPlotUC1.Clear();
            scottPlotUC1.PlotSignal(scottPlotUC1.fig.gen.SineAnimated(20000), 20000);
            Application.DoEvents();
            busyPlotting = false;
        }
        
        private void btn_oneMillionPoints(object sender, EventArgs e)
        {
            timer1.Enabled = false; // turn off live mode
            scottPlotUC1.PlotSignal(scottPlotUC1.fig.gen.RandomWalk(1_000_000, startRandom: true), 20_000, color: scottPlotUC1.fig.gen.randomColor);
            scottPlotUC1.AxisAuto();
        }

        private void btn_clear(object sender, EventArgs e)
        {
            timer1.Enabled = false; // turn off live mode
            scottPlotUC1.Clear(true);
        }
    }
}
