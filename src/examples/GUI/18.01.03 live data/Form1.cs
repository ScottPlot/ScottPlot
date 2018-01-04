using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _18._01._03_live_data
{
    public partial class Form1 : Form
    {
        private const int GRAPHSIZE = 50;
        private double[] Xs = new double[GRAPHSIZE];
        private double[] Ys = new double[GRAPHSIZE];
        private Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// initialize the graph with a full amount of random data
        /// </summary>
        private void btn_fill_Click(object sender, EventArgs e)
        {            
            for (int i=0; i< GRAPHSIZE; i++)
            {
                Xs[i] = i;
                Ys[i] = rand.NextDouble()*10;
            }
            update_graph();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            update_graph();
        }

        /// <summary>
        /// update the interactive graph with the new Xs and Ys
        /// </summary>
        private void update_graph()
        {
            if (scottPlotUC1.Xs.Length != Xs.Length)
            {
                // if the first run, reset the axis limits
                scottPlotUC1.SetData(Xs, Ys);
            }
            else
            {
                // if not the first run, respect current axis limits
                scottPlotUC1.Xs = Xs;
                scottPlotUC1.Ys = Ys;
                scottPlotUC1.UpdateGraph();
            }
            
        }

        /// <summary>
        /// "roll" the data to the left, putting new data to the right
        /// </summary>
        private void btn_extend_Click(object sender, EventArgs e)
        {
            // roll the data left by copying it in place
            Array.Copy(Ys, 1, Ys, 0, Ys.Length - 1);

            // set the last data point to something new
            Ys[Ys.Length-1] = rand.NextDouble() * 10;

            // now update the graph
            update_graph();
        }

        /// <summary>
        /// reset axis to optimal axis limits
        /// </summary>
        private void btn_auto_axis_Click(object sender, EventArgs e)
        {
            scottPlotUC1.SetData(Xs, Ys);
        }

        /// <summary>
        /// each timer tick automatically clicks the extend button
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            btn_extend_Click(null, null);
        }

        /// <summary>
        /// toggle the timer on and off (controlls scrolling)
        /// </summary>
        private void btn_continuous_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
            }
            else
            {
                timer1.Enabled = true;
            }
        }
    }
}
