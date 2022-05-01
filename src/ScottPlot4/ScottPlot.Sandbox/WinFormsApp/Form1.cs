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
        public Form1()
        {
            InitializeComponent();

            Random rand = new();

            for (int i = 0; i < 10; i++)
            {
                formsPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100), label: $"line {i + 1}");
            }

            formsPlot1.Refresh();
        }
    }
}
