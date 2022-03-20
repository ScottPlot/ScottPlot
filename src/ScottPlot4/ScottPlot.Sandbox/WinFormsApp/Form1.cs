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
        public Form1()
        {
            InitializeComponent();

            Random rand = new(0);
            double?[,] data = new double?[1000, 1000];
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (rand.NextDouble() > .98)
                        data[y, x] = rand.NextDouble();
                    else
                        data[y, x] = null;
                }
            }

            formsPlot1.Plot.AddHeatmap(data, lockScales: false);
            formsPlot1.Refresh();
        }
    }
}
