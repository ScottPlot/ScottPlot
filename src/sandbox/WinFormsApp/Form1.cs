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
        private readonly ScottPlot.FormsPlot formsPlot1;

        public Form1()
        {
            InitializeComponent();

            // add this manually because the .NET winforms designer is still in preview mode
            // and the FormsPlot user control typically does not appear in the toolbox
            formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };
            Controls.Add(formsPlot1);

            double[] ys = { 200, 150, 1100, 100, 125, 175, 125, 450, 250, 1000, 150, 450, 50, 50, 200, 400, 150, 100 };
            double[] xs = ScottPlot.DataGen.Consecutive(ys.Length);
            formsPlot1.plt.AddFillAboveAndBelow(xs, ys, 200);
            formsPlot1.Render();
        }
    }
}
