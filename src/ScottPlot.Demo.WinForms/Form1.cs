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
            var demo = new Demo.General.Plots.SinAndCos();
            var plt = new Plot();
            demo.Render(plt);
            Controls.Add(new FormsPlot(plt) { Dock = DockStyle.Fill, Name = "formsPlot1" });
        }
    }
}
