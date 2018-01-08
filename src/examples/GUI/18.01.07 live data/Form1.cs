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
        private const int GRAPHSIZE = 200;
        private double[] Xs = new double[GRAPHSIZE];
        private double[] Ys = new double[GRAPHSIZE];
        private Random rand = new Random();

        public Form1()
        {
            InitializeComponent();


            // fill the data window with something random
            for (int i = 0; i < GRAPHSIZE; i++)
            {
                Xs[i] = i;
                Ys[i] = rand.NextDouble() * 10;
            }
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
        /// each timer tick automatically clicks the extend button
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            btn_extend_Click(null, null);
        }

        private void scottPlotUC1_Load(object sender, EventArgs e)
        {

        }
    }
}
