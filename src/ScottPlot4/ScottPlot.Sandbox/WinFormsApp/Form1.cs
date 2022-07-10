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
            byte[] ys = DataGen.Sin(100_000, 5_000, 100, 100).Select(x => (byte)x).ToArray();
            formsPlot1.Plot.AddSignalConst(ys);
            formsPlot1.Refresh();
        }
    }
}
