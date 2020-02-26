using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var plt = ScottPlot.Demo.Plots.SinAndCos();
            Controls.Add(new ScottPlot.FormsPlot(plt) { Dock = DockStyle.Fill, Name = "formsPlot1" });
        }
    }
}
